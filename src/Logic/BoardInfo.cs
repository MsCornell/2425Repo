namespace Logic;

public class BoardInfo
{
    private readonly Dictionary<CellIndex, CellInfo> cells;

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

    // Winning mark image path
    public string WinningMark => winner != GameResult.InProgress ? UtilityExtensions.GetWinImage() : string.Empty;

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

    // Define the CanPlay method
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
    // Reset all cells to non-winning status before recalculating
    foreach (var cell in cells.Values)
    {
        cell.IsWinningCell = false;
    }

    // Check if any row has a winner
    if (IsWinningCombination(CellIndex.Cell1, CellIndex.Cell2, CellIndex.Cell3)) return MarkWinningCells(CellIndex.Cell1, CellIndex.Cell2, CellIndex.Cell3);
    if (IsWinningCombination(CellIndex.Cell4, CellIndex.Cell5, CellIndex.Cell6)) return MarkWinningCells(CellIndex.Cell4, CellIndex.Cell5, CellIndex.Cell6);
    if (IsWinningCombination(CellIndex.Cell7, CellIndex.Cell8, CellIndex.Cell9)) return MarkWinningCells(CellIndex.Cell7, CellIndex.Cell8, CellIndex.Cell9);

    // Check if any column has a winner
    if (IsWinningCombination(CellIndex.Cell1, CellIndex.Cell4, CellIndex.Cell7)) return MarkWinningCells(CellIndex.Cell1, CellIndex.Cell4, CellIndex.Cell7);
    if (IsWinningCombination(CellIndex.Cell2, CellIndex.Cell5, CellIndex.Cell8)) return MarkWinningCells(CellIndex.Cell2, CellIndex.Cell5, CellIndex.Cell8);
    if (IsWinningCombination(CellIndex.Cell3, CellIndex.Cell6, CellIndex.Cell9)) return MarkWinningCells(CellIndex.Cell3, CellIndex.Cell6, CellIndex.Cell9);

    // Check if any diagonal has a winner
    if (IsWinningCombination(CellIndex.Cell1, CellIndex.Cell5, CellIndex.Cell9)) return MarkWinningCells(CellIndex.Cell1, CellIndex.Cell5, CellIndex.Cell9);
    if (IsWinningCombination(CellIndex.Cell3, CellIndex.Cell5, CellIndex.Cell7)) return MarkWinningCells(CellIndex.Cell3, CellIndex.Cell5, CellIndex.Cell7);

    // If all cells are filled and there is no winner, return a draw
    if (cells.Values.All(cell => cell.Value != CellValue.Blank)) return GameResult.Cat;

    // Otherwise, the game is still in progress
    return GameResult.InProgress;
}

// Helper method to mark the winning cells
private GameResult MarkWinningCells(CellIndex a, CellIndex b, CellIndex c)
{
    cells[a].IsWinningCell = true;
    cells[b].IsWinningCell = true;
    cells[c].IsWinningCell = true;
    return GetWinner(a);
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
