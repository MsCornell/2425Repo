using System;

namespace Logic.Tests;

public class BoardTests
{
    private BoardRepository repository = new("http://localhost:5000/api/Board");
    [Fact]
    public async void Create_Valid_NoError()
    {
        // arrange
        var board = new Board
        {
            Started = DateTime.Now,
            Ended = DateTime.Now,
            BoardWinner = "Jenny",
            Cell1 = "X",
            Cell2 = "O",
            Cell3 = "X",
            Cell4 = "O",
            Cell5 = "X",
            Cell6 = "O",
            Cell7 = "X",
            Cell8 = "O",
            Cell9 = "X"
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
            Started = DateTime.Now,
            Ended = DateTime.Now,
            BoardWinner = "Jerry",
            Cell1 = "X",
            Cell2 = "X",
            Cell3 = "X",
            Cell4 = "O",
            Cell5 = "O",
            Cell6 = "X",
            Cell7 = "O",
            Cell8 = "O",
            Cell9 = "X"
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
            Started = DateTime.Now,
            Ended = DateTime.Now,
            BoardWinner = "Hinton",
            Cell1 = "O",
            Cell2 = "O",
            Cell3 = "O",
            Cell4 = "O",
            Cell5 = "X",
            Cell6 = "X",
            Cell7 = "X",
            Cell8 = "X",
            Cell9 = "X"
        };

        var created = await repository.CreateBoardAsync(board);
        created.BoardWinner = "Tom";

        // act
        var updated = await repository.UpdateBoardAsync(created);
        await repository.DeleteBoardAsync(updated.Id);

        // assert
        Assert.NotNull(updated);
        Assert.Equal("Tom", updated.BoardWinner);
        Assert.Equal(created.Id, updated.Id);
    }
}
