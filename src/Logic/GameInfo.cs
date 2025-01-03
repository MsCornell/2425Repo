﻿using System;
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

    public GameInfo()
    {
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
    }

    public void Play(BoardIndex boardIndex, CellIndex cellIndex)
    {
        if (!CanPlay(boardIndex, cellIndex))
        {
            throw new Exception("Selected play is invalid.");
        }

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

        // Send the current game state to the API after a valid move
        //Task.Run(SendBoardStateToFunctionAsync);
        //Console.WriteLine("1234");

    }

    /*
    public async Task SendBoardStateToFunctionAsync()
    {
        // var gameState = new
        // {
        //     Winner = Winner.ToString(),
        //     NextPlayer = NextPlayer.ToString(),
        //     Boards = boards.ToDictionary(b => b.Key.ToString(), b => b.Value.Winner.ToString())
        // };

        Dictionary<string, string> boardState = new Dictionary<string, string>
            {
                { "board", "XOXOXOXOX" },
                { "next", "O" }
            };

        var node = JsonSerializer.SerializeToNode(boardState);
        node?.AsObject();
        var content = JsonContent.Create(node);
        Console.WriteLine(content);

        var response = await httpClient.PostAsync("https://localhost:7071/api/minimax", content);
        response.EnsureSuccessStatusCode();

        // try
        // {
        //     var response = await httpClient.PostAsync("https://localhost:7071/api/minimax", content);
        //     response.EnsureSuccessStatusCode(); // Throw if not a success code.
        // }
        // catch (Exception ex)
        // {
        //     // Handle exceptions (e.g., logging)
        //     Console.WriteLine($"Error sending game state: {ex.Message}");
        // }
    }
    */

    // public async Task<String> CreateBoardAsync()
    // {
    //     //ArgumentNullException.ThrowIfNull(board);

    //     Dictionary<string, string> boardState = new Dictionary<string, string>
    //         {
    //             { "board", "XOXOXOXOX" },
    //             { "next", "O" }
    //         };

    //     var node = JsonSerializer.SerializeToNode(boardState);
    //     node?.AsObject();
    //     var content = JsonContent.Create(node);
    //     var url = $"http://localhost:7071/api/minimax";
    //     var response = await http.PostAsync(url, content);
    //     var root = await GetRootFromResponseAsync(response);
    //     return root.NextMove.Single();
    // }

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