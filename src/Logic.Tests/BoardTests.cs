using System;

namespace Logic.Tests;

    /*
    CREATE TABLE Board (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BoardWinner CHAR(1) NOT NULL CHECK (BoardWinner IN ('X', 'O', '-'))
    );
    */
public class BoardTests
{
    private BoardRepository repository = new("http://localhost:5000/api/Board");
    [Fact]
    public async void Create_Valid_NoError()
    {
        // arrange
        var board = new Board{
            BoardWinner = "X"
        };
        
        // act
        var created = await repository.CreateBoardAsync(board);
        await repository.DeleteBoardAsync(created.Id);

        // assert
        Assert.NotNull(created);
        Assert.Equal(board.BoardWinner, created.BoardWinner);
        Assert.NotEqual(board.Id, created.Id);
    }

    [Fact]
    //create test for get board
    public async void Get_Valid_NoError()
    {
        // arrange
        var board = new Board
        {
            BoardWinner = "O"
           
        };

        var created = await repository.CreateBoardAsync(board);

        // act
        var get = await repository.GetBoardAsync(created.Id);
        await repository.DeleteBoardAsync(created.Id);

        // assert
        Assert.NotNull(get);
        Assert.Equal(board.BoardWinner, get.BoardWinner);
        Assert.Equal(created.Id, get.Id);
    }

    //create for update
    [Fact]
    public async void Update_Valid_NoError()
    {
        // arrange
        var board = new Board
        {
            BoardWinner = "X"
            
        };

        var created = await repository.CreateBoardAsync(board);
        created.BoardWinner = "O";

        // act
        var updated = await repository.UpdateBoardAsync(created);
        await repository.DeleteBoardAsync(updated.Id);

        // assert
        Assert.NotNull(updated);
        Assert.Equal("O", updated.BoardWinner);
        Assert.Equal(created.Id, updated.Id);
    }
}
