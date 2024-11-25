using System;

namespace Logic.Tests;
/*
CREATE TABLE Game (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Started DATETIME2 NOT NULL,
    Ended DATETIME2,
    AiCharacter BIT NOT NULL CHECK (AiCharacter IN (0, 1)),
    PlayerId INT NOT NULL,
    PlayerCharacter VARCHAR(1) NOT NULL CHECK (PlayerCharacter IN ('X', 'O')),
    GameWinner CHAR(1) CHECK (GameWinner IN ('X', 'O', '-')),
    GameMode VARCHAR(50) NOT NULL,
    GameScore INT NOT NULL,
    FOREIGN KEY (PlayerCharacter) REFERENCES Character(CharacterName),
    FOREIGN KEY (PlayerId) REFERENCES Player(Id) ON DELETE CASCADE ON UPDATE CASCADE
);
*/
public class GameTests
{
    private GameRepository repository = new("http://localhost:5000/api/Game");

    [Fact]
    // get game by id
    public async Task GetGame_NoError()
    {
        // arrange
        var gameId = 1;
        // act
        var game = await repository.GetGameAsync(gameId);
        // assert
        Assert.NotNull(game);
    }
    // create game
    [Fact]
    public async Task CreateGame_NoError()
    {
        // arrange
        var game = new Game { Started = DateTime.Now, AiCharacter = false, PlayerId = 1, PlayerCharacter = "X", GameMode = "Local", GameScore = 30};
        // act
        var created = await repository.CreateGameAsync(game);
        await repository.DeleteGameAsync(created.Id);
        // assert
        Assert.NotNull(created);
        Assert.Equal(game.AiCharacter, created.AiCharacter);
        Assert.Equal(game.PlayerId, created.PlayerId);
        Assert.Equal(game.PlayerCharacter, created.PlayerCharacter);
        Assert.Equal(game.GameMode, created.GameMode);
        Assert.Equal(game.GameScore, created.GameScore);
    }

    // update game
    [Fact]
    public async Task UpdateGame_NoError()
    {
        // arrange
        var game = new Game { Started = DateTime.Now, AiCharacter = false, PlayerId = 1, PlayerCharacter = "X", GameMode = "Local", GameScore = 30};
        var created = await repository.CreateGameAsync(game);
        created.GameScore = 40;
        // act
        var updated = await repository.UpdateGameAsync(created);
        await repository.DeleteGameAsync(updated.Id);
        // assert
        Assert.NotNull(updated);
        Assert.Equal(created.Started, updated.Started);
        Assert.Equal(created.AiCharacter, updated.AiCharacter);
        Assert.Equal(created.PlayerId, updated.PlayerId);
        Assert.Equal(created.PlayerCharacter, updated.PlayerCharacter);
        Assert.Equal(created.GameMode, updated.GameMode);
        Assert.Equal(created.GameScore, updated.GameScore);
    }
}
