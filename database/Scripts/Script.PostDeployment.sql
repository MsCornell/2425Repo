-- seed player table
IF NOT EXISTS(SELECT TOP 1
    Id
FROM Player)
INSERT INTO Player
    (OAuthId, Name, Created, _username, _password)
VALUES
    ('123456', 'John Doe', '2021-01-01', 'JohnDoe', 'password'),
    ('123457', 'Jane Doe', '2021-01-01', 'JaneDoe', 'password'),
    ('123458', 'John Smith', '2021-01-01', 'JohnSmith', 'password'),
    ('123459', 'Jane Smith', '2021-01-01', 'JaneSmith', 'password'),
    ('123460', 'John Johnson', '2021-01-01', 'JohnJohnson', 'password'),
    ('123461', 'Jane Johnson', '2021-01-01', 'JaneJohnson', 'password'),
    ('123462', 'John Jackson', '2021-01-01', 'JohnJackson', 'password'),
    ('123463', 'Jane Jackson', '2021-01-01', 'JaneJackson', 'password'),
    ('123464', 'John Brown', '2021-01-01', 'JohnBrown', 'password'),
    ('123465', 'Jane Brown', '2021-01-01', 'JaneBrown', 'password');

-- SEED CHARACTER TABLE

--DELETE FROM Character;
IF NOT EXISTS(SELECT TOP 1
    [Character]
FROM Character)
INSERT INTO Character
VALUES
    ('JohnDoe'),
    ('JaneDoe'),
    ('JohnSmith'),
    ('JaneSmith'),
    ('JohnJohnson'),
    ('JaneJohnson'),
    ('JohnJackson'),
    ('JaneJackson'),
    ('JohnBrown'),
    ('JaneBrown');

-- DELETE FROM Game;
-- DELETE From Game_Board;
-- DELETE FROM Board;
-- Seed the Game table with 10 records
DBCC CHECKIDENT ('Game', RESEED, 0);
IF NOT EXISTS(SELECT TOP 1
    Id
FROM Game)
-- reset the identity seed
INSERT INTO Game
    (Started, Ended, NextBoard, NextPlayer, AiCharacter, PlayerId, PlayerCharacter, GameWinner)
VALUES
    ('2021-01-01', '2021-01-01', 1, 1, 'JohnDoe', 2, 'JaneDoe', 'JohnDoe'),
    ('2021-01-01', '2021-01-01', 1, 1, 'JohnDoe', 2, 'JaneDoe', 'JohnDoe'),
    ('2021-01-01', '2021-01-01', 1, 1, 'JohnDoe', 2, 'JaneDoe', 'JohnDoe'),
    ('2021-01-01', '2021-01-01', 1, 1, 'JohnDoe', 2, 'JaneDoe', 'JohnDoe'),
    ('2021-01-01', '2021-01-01', 1, 1, 'JohnDoe', 2, 'JaneDoe', 'JohnDoe'),
    ('2021-01-01', '2021-01-01', 1, 1, 'JohnDoe', 2, 'JaneDoe', 'JohnDoe'),
    ('2021-01-01', '2021-01-01', 1, 1, 'JohnDoe', 2, 'JaneDoe', 'JohnDoe'),
    ('2021-01-01', '2021-01-01', 1, 1, 'JohnDoe', 2, 'JaneDoe', 'JohnDoe'),
    ('2021-01-01', '2021-01-01', 1, 1, 'JohnDoe', 2, 'JaneDoe', 'JohnDoe'),
    ('2021-01-01', '2021-01-01', 1, 1, 'JohnDoe', 2, 'JaneDoe', 'JohnDoe');

-- seed Board table
DBCC CHECKIDENT ('Board', RESEED, 0);
IF NOT EXISTS(SELECT TOP 1
    Id
FROM Board)
INSERT INTO Board
    (Started, Ended, BoardWinner, Cell1, Cell2, Cell3, Cell4, Cell5, Cell6, Cell7, Cell8, Cell9)
VALUES
    -- seed 10 records
    ('2021-01-01', '2021-01-01', 'JohnDoe', 'X', 'O', 'X', 'O', 'X', 'O', 'X', 'O', 'X'),
    ('2021-01-01', '2021-01-01', 'JohnDoe', 'O', 'X', 'O', 'X', 'O', 'X', 'O', 'X', 'O'),
    ('2021-01-01', '2021-01-01', 'JohnDoe', 'X', 'O', 'X', 'O', 'X', 'O', 'X', 'O', 'X'),
    ('2021-01-01', '2021-01-01', 'JohnDoe', 'O', 'X', 'O', 'X', 'O', 'X', 'O', 'X', 'O'),
    ('2021-01-01', '2021-01-01', 'JohnDoe', 'X', 'O', 'X', 'O', 'X', 'O', 'X', 'O', 'X'),
    ('2021-01-01', '2021-01-01', 'JohnDoe', 'O', 'X', 'O', 'X', 'O', 'X', 'O', 'X', 'O'),
    ('2021-01-01', '2021-01-01', 'JohnDoe', 'X', 'O', 'X', 'O', 'X', 'O', 'X', 'O', 'X'),
    ('2021-01-01', '2021-01-01', 'JohnDoe', 'O', 'X', 'O', 'X', 'O', 'X', 'O', 'X', 'O'),
    ('2021-01-01', '2021-01-01', 'JohnDoe', 'X', 'O', 'X', 'O', 'X', 'O', 'X', 'O', 'X'),
    ('2021-01-01', '2021-01-01', 'JohnDoe', 'O', 'X', 'O', 'X', 'O', 'X', 'O', 'X', 'O');

-- seed the Gameboard table with 10 records
/*
    CREATE TABLE Game_Board (
    GameId INT,
    BoardId INT,
    Position INT,
    PRIMARY KEY (GameId, BoardId),
    FOREIGN KEY (GameId) REFERENCES Game(Id),
    FOREIGN KEY (BoardId) REFERENCES Board(Id)
);
    */
IF NOT EXISTS(SELECT TOP 1
    GameId
FROM Game_Board)
INSERT INTO Game_Board
    (GameId, BoardId, Position)
VALUES
    (1, 1, 1),
    (2, 2, 2),
    (3, 3, 3),
    (4, 4, 4),
    (5, 5, 5),
    (6, 6, 6),
    (7, 7, 7),
    (8, 8, 8),
    (9, 9, 9);

Delete from Audit_Operation;
-- seed the Audit_Operation table with 10 records
DBCC CHECKIDENT ('Audit_Operation', RESEED, 0);
IF NOT EXISTS(SELECT TOP 1
    Id
FROM Audit_Operation)
INSERT INTO Audit_Operation
    (Name)
VALUES
    ('Create'),
    ('Read'),
    ('Update'),
    ('Delete'),
    ('Login'),
    ('Logout'),
    ('Register'),
    ('Game'),
    ('Board'),
    ('Character');

/*
We currently don't have this table in our database
*/
-- seed the Player_Audit table with 10 records
-- IF NOT EXISTS(SELECT TOP 1 PlayerId FROM Player_Audit)
-- INSERT INTO Player_Audit (Date, PlayerId, OperationId)
-- VALUES
--     ('2021-01-01', 1, 1),
--     ('2021-01-01', 2, 2),
--     ('2021-01-01', 3, 3),
--     ('2021-01-01', 4, 4),
--     ('2021-01-01', 5, 5),
--     ('2021-01-01', 6, 6),
--     ('2021-01-01', 7, 7),
--     ('2021-01-01', 8, 8),
--     ('2021-01-01', 9, 9),
--     ('2021-01-01', 10, 10);
