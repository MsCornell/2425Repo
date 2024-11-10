using System;

namespace Logic.Tests;

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
        var game = new Game { Started = DateTime.Now, 
        Ended = DateTime.Now, NextBoard = 1, NextPlayer = 1, 
        AiCharacter = "Jenny", 
        PlayerId = 1, 
        PlayerCharacter = "JaneDoe", 
        GameWinner = "Jenny" };
        // act
        var created = await repository.CreateGameAsync(game);
        await repository.DeleteGameAsync(created.Id);
        // assert
        Assert.NotNull(created);
        Assert.Equal(game.NextBoard, created.NextBoard);
        Assert.Equal(game.NextPlayer, created.NextPlayer);
        Assert.Equal(game.AiCharacter, created.AiCharacter);
        Assert.Equal(game.PlayerId, created.PlayerId);
        Assert.Equal(game.PlayerCharacter, created.PlayerCharacter);
        Assert.Equal(game.GameWinner, created.GameWinner);
    }

    // update game
    [Fact]
    public async Task UpdateGame_NoError()
    {
        // arrange
        var game = new Game { Started = DateTime.Now, 
        Ended = DateTime.Now, NextBoard = 1, NextPlayer = 1, 
        AiCharacter = "Jenny", 
        PlayerId = 1, 
        PlayerCharacter = "JaneDoe", 
        GameWinner = "Jenny" };
        var created = await repository.CreateGameAsync(game);
        created.GameWinner = "JaneDoe";
        // act
        var updated = await repository.UpdateGameAsync(created);
        await repository.DeleteGameAsync(updated.Id);
        // assert
        Assert.NotNull(updated);
        Assert.Equal(created.NextBoard, updated.NextBoard);
        Assert.Equal(created.NextPlayer, updated.NextPlayer);
        Assert.Equal(created.AiCharacter, updated.AiCharacter);
        Assert.Equal(created.PlayerId, updated.PlayerId);
        Assert.Equal(created.PlayerCharacter, updated.PlayerCharacter);
        Assert.Equal(created.GameWinner, updated.GameWinner);
    }
}
