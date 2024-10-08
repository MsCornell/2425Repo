namespace Logic;

public class GameInfo
{
    private readonly Dictionary<BoardIndex, BoardInfo> boards;

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
            board.WinnerChanged += (s, e) => Winner = CalculateWinner();
        }

        // Randomize starting player & initialize
        NextPlayer = new Random().Next(0, 2) == 0 ? Players.X : Players.O;
        NextBoards = Boards(GameResult.InProgress);
        winner = CalculateWinner();
    }

    private GameResult CalculateWinner()
    {
        // Check rows
        foreach (var combination in new[]
        {
            (BoardIndex.Board1, BoardIndex.Board2, BoardIndex.Board3),
            (BoardIndex.Board4, BoardIndex.Board5, BoardIndex.Board6),
            (BoardIndex.Board7, BoardIndex.Board8, BoardIndex.Board9),
            (BoardIndex.Board1, BoardIndex.Board4, BoardIndex.Board7),
            (BoardIndex.Board2, BoardIndex.Board5, BoardIndex.Board8),
            (BoardIndex.Board3, BoardIndex.Board6, BoardIndex.Board9),
            (BoardIndex.Board1, BoardIndex.Board5, BoardIndex.Board9),
            (BoardIndex.Board3, BoardIndex.Board5, BoardIndex.Board7)
        })
        {
            if (boards[combination.Item1].Winner == boards[combination.Item2].Winner &&
                boards[combination.Item2].Winner == boards[combination.Item3].Winner &&
                boards[combination.Item1].Winner != GameResult.InProgress)
            {
                Winner = boards[combination.Item1].Winner;
                return Winner;
            }
        }

        // Check for a tie
        foreach (var board in boards.Values)
        {
            if (board.Winner == GameResult.InProgress) { return GameResult.InProgress; }
        }
        return GameResult.Cat;
    }

    public void Play(BoardIndex boardIndex, CellIndex cellIndex)
    {
        if (!CanPlay(boardIndex, cellIndex))
        {
            throw new Exception("Selected play is invalid.");
        }

        var board = boards[boardIndex];
        board.Play(cellIndex, NextPlayer);

        // convert cellIndex to boardIndex 
        var nextBoard = (BoardIndex)(int)cellIndex;
        if (boards[nextBoard].Winner == GameResult.InProgress)
        {
            NextBoards = [nextBoard];
        }
        else
        {
            NextBoards = Boards(GameResult.InProgress);
        }

        // Switch to the next player
        NextPlayer = NextPlayer == Players.X ? Players.O : Players.X;
    }

    public bool CanPlay(BoardIndex boardIndex, CellIndex cellIndex)
    {
        if (!Enum.IsDefined(typeof(BoardIndex), boardIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(boardIndex), "Invalid board index.");
        }

        if (!Enum.IsDefined(typeof(CellIndex), cellIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(cellIndex), "Invalid cell index.");
        }

        if (Winner != GameResult.InProgress)
        {
            return false;
        }

        if (!boards.TryGetValue(boardIndex, out BoardInfo? value))
        {
            return false;
        }

        if (value.Winner != GameResult.InProgress)
        {
            return false;
        }

        return value.CanPlay(cellIndex);
    }

    public BoardInfo GetBoard(BoardIndex boardIndex)
    {
        return boards[boardIndex];
    }

    private BoardIndex[] Boards(GameResult? gameResult = null)
    {
        return boards
            .Where(x => gameResult is null || x.Value.Winner == gameResult)
            .Select(x => x.Key)
            .ToArray();
    }
}
