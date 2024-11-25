using System;
using System.Net.Http.Json;
using System.Text.Json;

namespace Logic;

public class BoardInfo
{
    private readonly Dictionary<CellIndex, CellInfo> cells;

    private readonly HttpClient http = new();

    public event EventHandler<GameResult>? WinnerChanged;

    private GameResult winner = GameResult.Unknown;
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

    public BoardInfo()
    {
        cells = new Dictionary<CellIndex, CellInfo>
        {
            { CellIndex.Cell1, new CellInfo() },
            { CellIndex.Cell2, new CellInfo() },
            { CellIndex.Cell3, new CellInfo() },
            { CellIndex.Cell4, new CellInfo() },
            { CellIndex.Cell5, new CellInfo() },
            { CellIndex.Cell6, new CellInfo() },
            { CellIndex.Cell7, new CellInfo() },
            { CellIndex.Cell8, new CellInfo() },
            { CellIndex.Cell9, new CellInfo() }
        };

        foreach (var cell in cells.Values)
        {
            // Subscribe to the cell value change to recalculate the winner
            cell.ValueChanged += (s, e) => Winner = CalculateWinner();
        }

        // Initialize the winner state
        Winner = CalculateWinner();
    }

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

    // private static async Task<MLRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    // {
    //     var json = await response.Content.ReadAsStringAsync();
    //     var error = JsonSerializer.Deserialize<ErrorRoot>(json);
    //     if (error is not null && error.Error is not null)
    //     {
    //         throw new Exception(error.Error.Message);
    //     }
    //     var root = JsonSerializer.Deserialize<MLRoot>(json);
    //     if (root is null)
    //     {
    //         throw new Exception("Json is invalid.");
    //     }
    //     return root;
    // }

    internal void Play(CellIndex cellIndex, Players player)
    {
        if (!Enum.IsDefined(typeof(CellIndex), cellIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(cellIndex), "Invalid cell index.");
        }

        if (!CanPlay(cellIndex))
        {
            throw new InvalidOperationException("Cell is already occupied.");
        }

        // Set the cell value based on the player
        var playerValue = player == Players.X ? CellValue.X : CellValue.O;

        // Update the cell value
        cells[cellIndex].Value = playerValue;

        Winner = CalculateWinner();
    }

    public bool CanPlay(CellIndex cellIndex)
    {
        if (!Enum.IsDefined(typeof(CellIndex), cellIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(cellIndex), "Invalid cell index.");
        }

        // Check if the cell is blank (playable)
        return cells[cellIndex].Value == CellValue.Blank;
    }

    private GameResult CalculateWinner()
    {
        // Check if any row has a winner
        if (IsWinningCombination(CellIndex.Cell1, CellIndex.Cell2, CellIndex.Cell3))
        {

            return GetWinner(CellIndex.Cell1);
        }
        if (IsWinningCombination(CellIndex.Cell4, CellIndex.Cell5, CellIndex.Cell6))
        {

            return GetWinner(CellIndex.Cell4);
        }
        if (IsWinningCombination(CellIndex.Cell7, CellIndex.Cell8, CellIndex.Cell9))
        {

            return GetWinner(CellIndex.Cell7);
        }

        // Check if any column has a winner
        if (IsWinningCombination(CellIndex.Cell1, CellIndex.Cell4, CellIndex.Cell7))
        {

            return GetWinner(CellIndex.Cell1);
        }
        if (IsWinningCombination(CellIndex.Cell2, CellIndex.Cell5, CellIndex.Cell8))
        {

            return GetWinner(CellIndex.Cell2);
        }
        if (IsWinningCombination(CellIndex.Cell3, CellIndex.Cell6, CellIndex.Cell9))
        {

            return GetWinner(CellIndex.Cell3);
        }

        // Check if any diagonal has a winner
        if (IsWinningCombination(CellIndex.Cell1, CellIndex.Cell5, CellIndex.Cell9))
        {

            return GetWinner(CellIndex.Cell1);
        }
        if (IsWinningCombination(CellIndex.Cell3, CellIndex.Cell5, CellIndex.Cell7))
        {

            return GetWinner(CellIndex.Cell3);
        }

        // If all cells are filled and there is no winner, return a draw
        if (cells.Values.All(cell => cell.Value != CellValue.Blank))
        {

            return GameResult.Cat;
        }

        // Otherwise, the game is still in progress
        return GameResult.InProgress;
    }


    // Check if the three specified cells form a winning combination
    private bool IsWinningCombination(CellIndex a, CellIndex b, CellIndex c)
    {
        return cells[a].Value != CellValue.Blank &&
               cells[a].Value == cells[b].Value &&
               cells[a].Value == cells[c].Value;
    }

    // Get the winner based on the cell's value
    private GameResult GetWinner(CellIndex cell)
    {
        return cells[cell].Value == CellValue.X ? GameResult.XWins : GameResult.OWins;
    }

    // Get the information for a specific cell
    public CellInfo GetCell(CellIndex cellIndex)
    {
        return cells[cellIndex];
    }
}