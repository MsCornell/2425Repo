CREATE VIEW GameBoardDetail AS
SELECT 
    G.Id AS GameId,
    B.Id AS BoardId,
    B.BoardWinner
FROM Game_Board GB
JOIN Game G ON GB.GameId = G.Id
JOIN Board B ON GB.BoardId = B.Id;
