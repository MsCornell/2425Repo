if (db_name() = '2425cornell-db')
 RETURN;
-- seed player table
-- INITIALIZE THE DATABASE
DELETE FROM Game_Board;
DELETE FROM Game;
DELETE FROM Board;
DELETE FROM Character;
DELETE FROM Player;

INSERT INTO Player (Name, Created, _username, _password)
VALUES
    ('John Doe', '2021-01-01', 'JohnDoe', 'password'),
    ('Jane Doe', '2021-01-01', 'JaneDoe', 'password'),
    ('John Smith', '2021-01-01', 'JohnSmith', 'password'),
    ('Jane Smith', '2021-01-01', 'JaneSmith', 'password'),
    ('John Johnson', '2021-01-01', 'JohnJohnson', 'password'),
    ('Jane Johnson', '2021-01-01', 'JaneJohnson', 'password'),
    ('John Jackson', '2021-01-01', 'JohnJackson', 'password'),
    ('Jane Jackson', '2021-01-01', 'JaneJackson', 'password'),
    ('John Brown', '2021-01-01', 'JohnBrown', 'password'),
    ('Jane Brown', '2021-01-01', 'JaneBrown', 'password');
 
-- SEED CHARACTER TABLE
 
--DELETE FROM Character;
INSERT INTO Character
VALUES
    ('X'),
    ('O');

-- reset the identity seed
DBCC CHECKIDENT ('Game', RESEED, 0);
INSERT INTO Game (Started, Ended, AiCharacter, PlayerId, PlayerCharacter, GameWinner, GameMode, GameScore)
VALUES
    ('2021-01-01', '2021-01-01', 0, 1, 'X', 'X', 'Local', 30),
    ('2021-01-01', '2021-01-01', 0, 2, 'O', 'O', 'Local', 30),
    ('2021-01-01', '2021-01-01', 1, 3, 'X', 'X', 'Hard', 40),
    ('2021-01-01', '2021-01-01', 1, 4, 'X', 'X', 'Medium', 20),
    ('2021-01-01', '2021-01-01', 1, 5, 'X', 'X', 'Easy', 10),
    ('2021-01-01', '2021-01-01', 0, 6, 'X', '-', 'Local', 10),
    ('2021-01-01', '2021-01-01', 0, 7, 'O', '-', 'Local', 10),
    ('2021-01-01', '2021-01-01', 1, 8, 'O', '-', 'Easy', 5),
    ('2021-01-01', '2021-01-01', 1, 9, 'X', '-', 'Medium', 10),
    ('2021-01-01', '2021-01-01', 1, 10, 'O', '-', 'Hard', 20);
 
DBCC CHECKIDENT ('Board', RESEED, 0);
INSERT INTO Board (BoardWinner)
VALUES
    ('X'),
    ('O'),
    ('-'),
    ('X'),
    ('O'),
    ('-'),
    ('X'),
    ('-'),
    ('-');
 
-- seed Game_Board table
INSERT INTO Game_Board (GameId, BoardId)
VALUES
    (1, 1),
    (1, 2),
    (1, 3),
    (1, 4),
    (1, 5),
    (1, 6),
    (1, 7),
    (1, 8),
    (1, 9);
 