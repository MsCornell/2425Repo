using System;

namespace Logic.Tests;

public class GameDetailTests
{
    private GameDetailRepository repository = new("http://localhost:5000/api/GameDetail");
    [Fact]
    //get all game details
    public async Task GetAllGameDetails_Valid_NoError()
    {
        var gameDetails = await repository.GetAllGameDetailsAsync();
        Assert.NotNull(gameDetails);
    }
    [Fact]
    //get one game detail
    public async Task GetOneGameDetail_Valid_NoError()
    {
        var gameDetail = await repository.GetOneGameDetailAsync(10000);
        Assert.NotNull(gameDetail);
    }
}
