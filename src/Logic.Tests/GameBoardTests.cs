using System;

namespace Logic.Tests;
/*
CREATE TABLE Game_Board (
    GameId INT NOT NULL,
    BoardId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (GameId, BoardId),
    CONSTRAINT FK_Game FOREIGN KEY (GameId) REFERENCES Game(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_Board FOREIGN KEY (BoardId) REFERENCES Board(Id) ON DELETE CASCADE ON UPDATE CASCADE
);
*/
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
        //await repository.DeleteGameBoardAsync(2, 1);
        var gameBoard = new GameBoard { GameId = 2, BoardId = 1, CreatedAt = DateTime.Now};
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
        var gameBoard = new GameBoard { GameId = 2, BoardId = 1, CreatedAt = DateTime.Now};
        var created = await repository.CreateGameBoardAsync(gameBoard);
        created.GameId = 3;
        // act
        var updated = await repository.UpdateGameBoardAsync(created);
        await repository.DeleteGameBoardAsync(updated.GameId, updated.BoardId);
        await repository.DeleteGameBoardAsync(gameBoard.GameId, gameBoard.BoardId);
        // assert
        Assert.NotNull(updated);
        Assert.Equal(created.GameId, updated.GameId);
        Assert.Equal(created.BoardId, updated.BoardId);
    }

}
