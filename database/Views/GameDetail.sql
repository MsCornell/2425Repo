CREATE VIEW GameDetail AS 
SELECT G.Id AS GameId, 
P.Name AS PlayerName, 
G.PlayerCharacter, 
G.AiCharacter, 
G.GameMode, 
G.Started, 
G.Ended, 
G.GameWinner, 
G.GameScore 
FROM Game G JOIN Player P 
ON G.PlayerId = P.Id;
