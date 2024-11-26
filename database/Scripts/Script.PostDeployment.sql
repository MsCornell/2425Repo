IF NOT (DB_NAME() = '2425cornell-db')
BEGIN
	DELETE FROM Game_Board 
	DELETE FROM Game 
	DELETE FROM Board 
	DELETE FROM Player 
END

DELETE FROM Game_Board WHERE GameId BETWEEN 10000 AND 10010
DELETE FROM Game WHERE Id BETWEEN 10000 AND 10010
DELETE FROM Board WHERE Id BETWEEN 10000 AND 10010
DELETE FROM Player WHERE Id BETWEEN 10000 AND 10010

SET IDENTITY_INSERT Player ON;

INSERT INTO Player (Id, Name, Created, _username, _password)
VALUES
    (10000, 'John Doe', '2021-01-01', 'JohnDoe', 'password'),
    (10001, 'Jane Doe', '2021-01-01', 'JaneDoe', 'password'),
    (10002, 'John Smith', '2021-01-01', 'JohnSmith', 'password'),
    (10003, 'Jane Smith', '2021-01-01', 'JaneSmith', 'password'),
    (10004, 'John Johnson', '2021-01-01', 'JohnJohnson', 'password'),
    (10005, 'Jane Johnson', '2021-01-01', 'JaneJohnson', 'password'),
    (10006, 'John Jackson', '2021-01-01', 'JohnJackson', 'password'),
    (10007, 'Jane Jackson', '2021-01-01', 'JaneJackson', 'password'),
    (10008, 'John Brown', '2021-01-01', 'JohnBrown', 'password'),
    (10009, 'Jane Brown', '2021-01-01', 'JaneBrown', 'password');

SET IDENTITY_INSERT Player OFF;

IF NOT EXISTS (SELECT TOP 1 1 FROM Character)
INSERT INTO Character
VALUES
    ('X'),
    ('O');

SET IDENTITY_INSERT Game ON;

INSERT INTO Game (Id, Started, Ended, AiCharacter, PlayerId, PlayerCharacter, GameWinner, GameMode, GameScore)
VALUES
    (10000, '2021-01-01', '2021-01-01', 0, 10000, 'X', 'X', 'Local', 30),
    (10001, '2021-01-01', '2021-01-01', 0, 10001, 'O', 'O', 'Local', 30),
    (10002, '2021-01-01', '2021-01-01', 1, 10002, 'X', 'X', 'Hard', 40),
    (10003, '2021-01-01', '2021-01-01', 1, 10003, 'X', 'X', 'Medium', 20),
    (10004, '2021-01-01', '2021-01-01', 1, 10004, 'X', 'X', 'Easy', 10),
    (10005, '2021-01-01', '2021-01-01', 0, 10005, 'X', '-', 'Local', 10),
    (10006, '2021-01-01', '2021-01-01', 0, 10006, 'O', '-', 'Local', 10),
    (10007, '2021-01-01', '2021-01-01', 1, 10007, 'O', '-', 'Easy', 5),
    (10008, '2021-01-01', '2021-01-01', 1, 10008, 'X', '-', 'Medium', 10),
    (10009, '2021-01-01', '2021-01-01', 1, 10009, 'O', '-', 'Hard', 20);

SET IDENTITY_INSERT Game OFF;

SET IDENTITY_INSERT Board ON;

INSERT INTO Board (Id, BoardWinner)
VALUES
    (10000, 'X'),
    (10001, 'O'),
    (10002, '-'),
    (10003, 'X'),
    (10004, 'O'),
    (10005, '-'),
    (10006, 'X'),
    (10007, '-'),
    (10008, '-');

SET IDENTITY_INSERT Board OFF;

INSERT INTO Game_Board (GameId, BoardId)
VALUES
    (10000, 10000),
    (10000, 10001),
    (10000, 10002),
    (10000, 10003),
    (10000, 10004),
    (10000, 10005),
    (10000, 10006),
    (10000, 10007),
    (10000, 10008);

