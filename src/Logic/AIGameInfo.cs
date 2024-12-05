using System;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Logic;

public class AIGameInfo
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

    private string minimaxUrl => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development" 
    ? "http://localhost:7071/api/minimax" 
    : "https://icy-sea-07449320f.5.azurestaticapps.net/api/minimax";

    public Players NextPlayer { get; private set; }
    public BoardIndex[] NextBoards { get; private set; }
    public String CurrentGameMode { get; private set; }

    public AIGameInfo(String mode)
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
       
        NextPlayer = new Random().Next(0, 2) == 0 ? Players.X : Players.O;

        // At the start, all boards are available for play
        NextBoards = Boards(GameResult.InProgress);

        if (NextPlayer == Players.O)
        {
            if (CurrentGameMode == "hard")
            {
                Play(BoardIndex.Board5, CellIndex.Cell5);
            }
            else if (CurrentGameMode == "medium")
            {
                var cellIndex = new Random().Next(1, 10);
                Play(BoardIndex.Board5, (CellIndex)cellIndex);
            }
            else
            {
                var cellIndex = new Random().Next(1, 10);
                var boardIndex = new Random().Next(1, 10);
                Play((BoardIndex)boardIndex, (CellIndex)cellIndex);
            }
            
        }
    }

    public Boolean  Play(BoardIndex boardIndex, CellIndex cellIndex)
    {
        if (!CanPlay(boardIndex, cellIndex))
        {
            //throw new Exception("Selected play is invalid.");
            return true;
        }
        else
        {
            var board = boards[boardIndex];
            board.Play(cellIndex, NextPlayer);  // Perform a move on the specified board


            // Check if the current board has ended (there is a winner or it's full)
            if (board.Winner != GameResult.InProgress)
            {
                // Let the player choose from other unfinished boards
                NextBoards = Boards(GameResult.InProgress);  // Switch to other unfinished boards

                // If all boards are completed, end the game
                Winner = CalculateOverallWinner();
                if (Winner != GameResult.InProgress)
                {
                    return false;
                }
            }
            else
            {
                // If the current board is not finished, restrict the next move to this board
                NextBoards = new[] { boardIndex };
            }

            // Switch player
            NextPlayer = NextPlayer == Players.X ? Players.O : Players.X;

            // Send the current game state to the API after a valid move
            //Task.Run(SendBoardStateToFunctionAsync);
            //Console.WriteLine("1234");
            return true ;
        }

        

    }

public async Task PlayAIAsync(BoardIndex boardIndex)
{
    if (boards[boardIndex].Winner != GameResult.InProgress)
    {
        await aiHelper();
    }
    else
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
            //{ "next_player", Players.O.ToString() },
            { "difficulty_level", CurrentGameMode }
        };

        //var nextMoveIndex = await SendBoardStateToFunctionAsync(boardState);

        var nextMoveIndex = -1;

        try
        {
            nextMoveIndex = await SendBoardStateToFunctionAsync(boardState);
        }
        catch
        {
            Console.WriteLine("Error in AI function");
        }

        // If the AI function returns a valid move, play it
        if (nextMoveIndex != -1)
        {
            board.Play((CellIndex)nextMoveIndex, NextPlayer);
        }
        else
        {
            var boardArray = boardStateString.ToCharArray();

                var nextIndex = boardArray
                    .Select((value, index) => new { value, index })
                    .Where(x => x.value == '-')
                    .Select(x => x.index + 1)
                    .OrderBy(_ => Guid.NewGuid())
                    .FirstOrDefault();

            board.Play((CellIndex)nextIndex, NextPlayer);
        }
        
        

        // Check if the current board has ended (there is a winner or it's full)
        if (board.Winner != GameResult.InProgress)
        {
            // Let the player choose from other unfinished boards
            NextBoards = Boards(GameResult.InProgress);  // Switch to other unfinished boards

            // If all boards are completed, end the game
            Winner = CalculateOverallWinner();
            if (Winner != GameResult.InProgress)
            {
                return;
            }
        }
        else
        {
            // If the current board is not finished, restrict the next move to this board
            NextBoards = new[] { boardIndex };
        }

        // Switch player
        NextPlayer = NextPlayer == Players.X ? Players.O : Players.X;

    }

    
}

    public async Task aiHelper()
    {
        /*
        NextBoards = Boards(GameResult.InProgress);  // Switch to other unfinished boards
        var nextIndex = NextBoards[new Random().Next(0, NextBoards.Length)];

        await PlayAIAsync((BoardIndex)nextIndex);
        */
        // go through all boards and create a string representation of the overall board state
        string overallBoardStateString = "";
        foreach (var board in boards.Values)
        {
            overallBoardStateString += board.Winner switch
            {
                GameResult.XWins => "X",
                GameResult.OWins => "O",
                GameResult.Cat => "C",
                _ => "_"
            };
        }

        var boardState = new Dictionary<string, string>
        {
            { "board_state", overallBoardStateString },
            //{ "next_player", Players.O.ToString() },
            { "difficulty_level", CurrentGameMode }
        };

        // Send the overall board state to the AI function

        var nextMoveIndex = -1;

        try
        {
            nextMoveIndex = await SendBoardStateToFunctionAsync(boardState);
        }
        catch
        {
            Console.WriteLine("Error in AI function");
        }
        

        // If the AI function returns a valid move, play it
        if (nextMoveIndex != -1)
        {
            await PlayAIAsync((BoardIndex)nextMoveIndex);
        }
        else
        {
            NextBoards = Boards(GameResult.InProgress);  // Switch to other unfinished boards
            var nextIndex = NextBoards[new Random().Next(0, NextBoards.Length)];

            await PlayAIAsync((BoardIndex)nextIndex);
        } 
}

    public async Task<int> SendBoardStateToFunctionAsync(Dictionary<string, string> boardState)
    {
        var content = JsonContent.Create(boardState);
        var response = await httpClient.PostAsync(minimaxUrl, content);
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