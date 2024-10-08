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
            cell.ValueChanged += (s, e) => Winner = CalculateWinner();
        }

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

        var playerValue = player == Players.X ? CellValue.X : CellValue.O;

        cells[cellIndex].Value = playerValue;
    }

    public bool CanPlay(CellIndex cellIndex)
    {
        if (!Enum.IsDefined(typeof(CellIndex), cellIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(cellIndex), "Invalid cell index.");
        }

        return cells[cellIndex].Value == CellValue.Blank;
    }

    private GameResult CalculateWinner()
    {
        // Check rows
        foreach (var combination in new[]
        {
            // rows
            (CellIndex.Cell1, CellIndex.Cell2, CellIndex.Cell3),
            (CellIndex.Cell4, CellIndex.Cell5, CellIndex.Cell6),
            (CellIndex.Cell7, CellIndex.Cell8, CellIndex.Cell9),
            // columns
            (CellIndex.Cell1, CellIndex.Cell4, CellIndex.Cell7),
            (CellIndex.Cell2, CellIndex.Cell5, CellIndex.Cell8),
            (CellIndex.Cell3, CellIndex.Cell6, CellIndex.Cell9),
            // diagonals
            (CellIndex.Cell1, CellIndex.Cell5, CellIndex.Cell9),
            (CellIndex.Cell3, CellIndex.Cell5, CellIndex.Cell7)
        })
        {
            if (cells[combination.Item1].Value == cells[combination.Item2].Value &&
                cells[combination.Item2].Value == cells[combination.Item3].Value &&
                cells[combination.Item1].Value != CellValue.Blank)
            {
                return cells[combination.Item1].Value == CellValue.X ? GameResult.X : GameResult.O;
            }
        }

        // Check for a tie
        foreach (var cell in cells.Values)
        {
            if (cell.Value == CellValue.Blank) { return GameResult.InProgress; }
        }
        return GameResult.Cat;
    }

    public CellInfo GetCell(CellIndex cellIndex)
    {
        return cells[cellIndex];
    }
}