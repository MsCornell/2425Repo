CREATE VIEW PlayerWinRate AS
-- Common Table Expression
WITH ModeStats AS (
    SELECT 
        P.Id AS PlayerId,
        P.Name AS PlayerName,
        G.GameMode,
        COUNT(G.Id) AS TotalGamesInMode,
        SUM(CASE WHEN G.GameWinner = G.PlayerCharacter THEN 1 ELSE 0 END) AS WinsInMode,
        ROUND(
            CAST(SUM(CASE WHEN G.GameWinner = G.PlayerCharacter THEN 1 ELSE 0 END) AS FLOAT) /
            NULLIF(COUNT(G.Id), 0) * 100, 2
        ) AS WinRateInMode
    FROM Player P
    LEFT JOIN Game G ON P.Id = G.PlayerId
    GROUP BY P.Id, P.Name, G.GameMode
),
OverallStats AS (
    SELECT 
        P.Id AS PlayerId,
        P.Name AS PlayerName,
        COUNT(G.Id) AS TotalGames,
        SUM(CASE WHEN G.GameWinner = G.PlayerCharacter THEN 1 ELSE 0 END) AS TotalWins,
        ROUND(
            CAST(SUM(CASE WHEN G.GameWinner = G.PlayerCharacter THEN 1 ELSE 0 END) AS FLOAT) /
            NULLIF(COUNT(G.Id), 0) * 100, 2
        ) AS OverallWinRate
    FROM Player P
    LEFT JOIN Game G ON P.Id = G.PlayerId
    GROUP BY P.Id, P.Name
)
SELECT 
    M.PlayerId,
    M.PlayerName,
    M.GameMode,
    M.TotalGamesInMode,
    M.WinsInMode,
    M.WinRateInMode,
    O.TotalGames,
    O.TotalWins,
    O.OverallWinRate
FROM ModeStats M
JOIN OverallStats O ON M.PlayerId = O.PlayerId;
