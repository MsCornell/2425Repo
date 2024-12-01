using System;

namespace Logic.Tests;

public class PlayerWinRateTests
{
    private PlayerWinRateRepository repository = new("http://localhost:5000/api/PlayerWinRate");
    [Fact]
    //get all player win rates
    public async Task GetAllPlayerWinRates_Valid_NoError()
    {
        var playerWinRates = await repository.GetAllPlayerAsync();
        Assert.NotNull(playerWinRates);
    }
    [Fact]
    //get one player win rate
    public async Task GetOnePlayerWinRate_Valid_NoError()
    {
        var playerWinRate = await repository.GetOnePlayerAsync(10000);
        Assert.NotNull(playerWinRate);
    }
}

