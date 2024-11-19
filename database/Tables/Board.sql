CREATE TABLE Board (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BoardWinner CHAR(1) NOT NULL CHECK (BoardWinner IN ('X', 'O', '-'))
);

