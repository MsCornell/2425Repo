CREATE TABLE Game_Board (
    GameId INT,
    BoardId INT,
    Position INT,
    PRIMARY KEY (GameId, BoardId),
    FOREIGN KEY (GameId) REFERENCES Game(Id),
    FOREIGN KEY (BoardId) REFERENCES Board(Id)
);

