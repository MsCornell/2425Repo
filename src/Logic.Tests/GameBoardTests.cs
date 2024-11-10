using System;

namespace Logic.Tests;

public class GameBoardTests
{
    private GameBoardRepository repository = new("http://localhost:5000/api/Game_Board");

   [Fact]
   // get gameboard by id
    public async Task GetGameBoard_NoError()
    {
         // arrange
         var gameId = 1;
         var boardId = 1;
         // act
         var gameBoard = await repository.GetGameBoardAsync(gameId, boardId);
         // assert
         Assert.NotNull(gameBoard);
    }
    // create gameboard
    [Fact]
    public async Task CreateGameBoard_NoError()
    {
        // arrange
        var gameBoard = new GameBoard { GameId = 17, BoardId = 17, Position = 1};
        // act
        var created = await repository.CreateGameBoardAsync(gameBoard);
        await repository.DeleteGameBoardAsync(created.GameId, created.BoardId);
        // assert
        Assert.NotNull(created);
        Assert.Equal(gameBoard.GameId, created.GameId);
        Assert.Equal(gameBoard.BoardId, created.BoardId);
    }
    // update gameboard

    [Fact]
    public async Task UpdateGameBoard_NoError()
    {
        // arrange
        var gameBoard = new GameBoard { GameId = 17, BoardId = 17, Position = 0};
        var newGameBoard = new GameBoard { GameId = 17, BoardId = 17, Position = 1};
        var created = await repository.CreateGameBoardAsync(gameBoard);
        // act
        var updated = await repository.UpdateGameBoardAsync(newGameBoard);

        await repository.DeleteGameBoardAsync(updated.GameId, updated.BoardId);
        // assert
        Assert.NotNull(updated);
        Assert.Equal(created.GameId, updated.GameId);
        Assert.Equal(created.BoardId, updated.BoardId);
        Assert.Equal(1, updated.Position);
    }

}
