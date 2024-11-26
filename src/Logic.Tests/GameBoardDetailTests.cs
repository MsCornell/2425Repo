using System;

namespace Logic.Tests;

public class GameBoardDetailTests
{
    private GameBoardDetailRepository repository = new("http://localhost:5000/api/GameBoardDetail");
    [Fact]
    //get all game board details
    public async Task GetAllGameDetails_Valid_NoError()
    {
        var gameBoardDetails = await repository.GetAllGameBoardsAsync();
        Assert.NotNull(gameBoardDetails);
    }
    [Fact]
    //get one game board detail
    public async Task GetOneGameDetail_Valid_NoError()
    {
        var gameBoardDetail = await repository.GetOneGameBoardAsync(10000,10000);
        Assert.NotNull(gameBoardDetail);
    }
}

