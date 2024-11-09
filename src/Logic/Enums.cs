namespace Logic;

public enum GameResult
{
    X,
    O,
    Cat, // For a tie
    InProgress,
    Unknown,
    XWins,
    OWins
}

public enum CellValue
{
    Blank,
    X,
    O
}

public enum Players
{
    X,
    O,
    cat
}

public enum CellIndex : int { Cell1 = 1, Cell2 = 2, Cell3 = 3, Cell4 = 4, Cell5 = 5, Cell6 = 6, Cell7 = 7, Cell8 = 8, Cell9 = 9 }

public enum BoardIndex : int { Board1 = 1, Board2 = 2, Board3 = 3, Board4 = 4, Board5 = 5, Board6 = 6, Board7 = 7, Board8 = 8, Board9 = 9 }