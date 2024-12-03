using System;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Logic;

public class GameInfo
{
    private readonly Dictionary<BoardIndex, BoardInfo> boards;

    private readonly HttpClient httpClient;

    public event EventHandler<GameResult>? WinnerChanged;

    private GameResult winner = GameResult.InProgress;
    public GameResult Winner
    {
        get => winner;
        private set
        {
            if (winner != value)
            {
                winner = value;
                WinnerChanged?.Invoke(this, winner);
            }
        }
    }

    public Players NextPlayer { get; private set; }
    public BoardIndex[] NextBoards { get; private set; }
    public GameMode CurrentGameMode { get; private set; }

    public GameInfo(GameMode mode)
    {
        CurrentGameMode = mode;
        httpClient = new HttpClient();

        boards = new Dictionary<BoardIndex, BoardInfo>
        {
            { BoardIndex.Board1, new BoardInfo() },
            { BoardIndex.Board2, new BoardInfo() },
            { BoardIndex.Board3, new BoardInfo() },
            { BoardIndex.Board4, new BoardInfo() },
            { BoardIndex.Board5, new BoardInfo() },
            { BoardIndex.Board6, new BoardInfo() },
            { BoardIndex.Board7, new BoardInfo() },
            { BoardIndex.Board8, new BoardInfo() },
            { BoardIndex.Board9, new BoardInfo() }
        };

        foreach (var board in boards.Values)
        {
            board.WinnerChanged += (s, e) => Winner = CalculateOverallWinner();
        }

        // Randomly assign the starting player & initialize
        if (mode == GameMode.SinglePlayer)
        {
            NextPlayer = Players.X;
        }
        else
        {
        NextPlayer = new Random().Next(0, 2) == 0 ? Players.X : Players.O;
        }

        // At the start, all boards are available for play
        NextBoards = Boards(GameResult.InProgress);
    }

    public async Task PlayAsync(BoardIndex boardIndex, CellIndex cellIndex)
{
    if (!CanPlay(boardIndex, cellIndex))
    {
        throw new Exception("Selected play is invalid.");
    }

    var board = boards[boardIndex];
    board.Play(cellIndex, NextPlayer);

    if (board.Winner != GameResult.InProgress)
    {
        NextBoards = Boards(GameResult.InProgress);
        Winner = CalculateOverallWinner();
        if (Winner != GameResult.InProgress)
        {
            return;
        }
    }
    else
    {
        NextBoards = new[] { boardIndex };
    }

    NextPlayer = Players.O;

    if (CurrentGameMode == GameMode.SinglePlayer && NextPlayer == Players.O)
    {
        await PlayAIAsync(boardIndex);
        NextPlayer = Players.X;
        if (board.Winner != GameResult.InProgress)
        {
            NextBoards = Boards(GameResult.InProgress);
            Winner = CalculateOverallWinner();
            if (Winner != GameResult.InProgress)
            {
                NextPlayer = Players.X;
                return;
            }
        }
        else
        {
            NextBoards = new[] { boardIndex };
            NextPlayer = Players.X;
        }
    }
}

private async Task PlayAIAsync(BoardIndex boardIndex)
{
    var board = boards[boardIndex];
    string boardStateString = "";
    for (var cell = 1; cell <= 9; cell++)
    {
        string cellString = board.GetCell((CellIndex)cell).Value.ToString();
        Console.WriteLine(cellString);
        boardStateString += cellString == "Blank" ? "_" : cellString;
    }
    //Console.WriteLine(boardStateString);
    var boardState = new Dictionary<string, string>
    {
        { "board_state", boardStateString },
        { "next_player", Players.O.ToString() }
    };

    var nextMoveIndex = await SendBoardStateToFunctionAsync(boardState);

    if (nextMoveIndex != -1 && CanPlay(boardIndex, (CellIndex)nextMoveIndex))
    {
        board.Play((CellIndex)nextMoveIndex, Players.O);
            
    }
}




    public async Task<int> SendBoardStateToFunctionAsync(Dictionary<string, string> boardState)
    {
        var content = JsonContent.Create(boardState);
        var response = await httpClient.PostAsync("http://localhost:7071/api/minimax", content);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var nextMoveData = JsonSerializer.Deserialize<Dictionary<string, int>>(responseContent);
        return nextMoveData != null && nextMoveData.ContainsKey("next_move") ? nextMoveData["next_move"] : -1;
    }


    public bool CanPlay(BoardIndex boardIndex, CellIndex cellIndex)
    {
        // Check if the board index is valid
        if (!Enum.IsDefined(typeof(BoardIndex), boardIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(boardIndex), "Invalid board index.");
        }

        // Check if the cell index is valid
        if (!Enum.IsDefined(typeof(CellIndex), cellIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(cellIndex), "Invalid cell index.");
        }

        // If the game already has a winner, no further moves are allowed
        if (Winner != GameResult.InProgress)
        {
            return false;  // The game is over
        }

        // Ensure the current board is one of the playable boards
        if (!NextBoards.Contains(boardIndex))
        {
            return false;  // The current board is not in the selectable boards
        }

        // Check if the board exists
        if (!boards.TryGetValue(boardIndex, out BoardInfo? board))
        {
            return false;  // The specified board could not be found
        }

        // Check if the board already has a winner
        if (board.Winner != GameResult.InProgress)
        {
            return false;  // The board is already finished
        }

        // Check if the specified cell is available
        return board.CanPlay(cellIndex);  // Ensure the cell is empty
    }

    public BoardInfo GetBoard(BoardIndex boardIndex)
    {
        return boards[boardIndex];
    }

    // Get all boards that match the specified game result (e.g., InProgress)
    private BoardIndex[] Boards(GameResult? gameResult = null)
    {
        return boards
            .Where(x => gameResult is null || x.Value.Winner == gameResult)
            .Select(x => x.Key)
            .ToArray();
    }

    private GameResult CalculateOverallWinner()
    {
        // First, check if there is a winning combination of three small boards
        // Check if any row has a winner
        if (IsWinningCombination(BoardIndex.Board1, BoardIndex.Board2, BoardIndex.Board3)) return GetBoardWinner(BoardIndex.Board1);
        if (IsWinningCombination(BoardIndex.Board4, BoardIndex.Board5, BoardIndex.Board6)) return GetBoardWinner(BoardIndex.Board4);
        if (IsWinningCombination(BoardIndex.Board7, BoardIndex.Board8, BoardIndex.Board9)) return GetBoardWinner(BoardIndex.Board7);

        // Check if any column has a winner
        if (IsWinningCombination(BoardIndex.Board1, BoardIndex.Board4, BoardIndex.Board7)) return GetBoardWinner(BoardIndex.Board1);
        if (IsWinningCombination(BoardIndex.Board2, BoardIndex.Board5, BoardIndex.Board8)) return GetBoardWinner(BoardIndex.Board2);
        if (IsWinningCombination(BoardIndex.Board3, BoardIndex.Board6, BoardIndex.Board9)) return GetBoardWinner(BoardIndex.Board3);

        // Check if any diagonal has a winner
        if (IsWinningCombination(BoardIndex.Board1, BoardIndex.Board5, BoardIndex.Board9)) return GetBoardWinner(BoardIndex.Board1);
        if (IsWinningCombination(BoardIndex.Board3, BoardIndex.Board5, BoardIndex.Board7)) return GetBoardWinner(BoardIndex.Board3);

        // If all boards are completed and there is no winner, declare a draw
        if (boards.Values.All(b => b.Winner != GameResult.InProgress))
        {
            return GameResult.Cat;  // It's a draw
        }

        // The game is still in progress
        return GameResult.InProgress;
    }

    private bool IsWinningCombination(BoardIndex a, BoardIndex b, BoardIndex c)
    {
        // Check if all three small boards have the same winner, and the winner is not a draw or in progress
        return boards[a].Winner != GameResult.InProgress &&
               boards[a].Winner == boards[b].Winner &&
               boards[a].Winner == boards[c].Winner;
    }

    private GameResult GetBoardWinner(BoardIndex board)
    {
        return boards[board].Winner;
    }
}