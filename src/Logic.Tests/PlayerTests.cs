using NuGet.Frameworks;

namespace Logic.Tests;

public class PlayerTests
{
    private PlayerRepository repository = new("https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/Player");

    [Fact]
    public async void Create_Valid_NoError()
    {
        // arrange
        
        var player = new Player
        {
            Name = "Jerry",
            Created = DateTime.Now,
            Email = "testabc@example.com",
            Password = "password"
        };

        // act
        var created = await repository.CreateAsync(player);
        await repository.DeleteAsync(created.Id);

        // assert
        Assert.NotNull(created);
        Assert.NotEqual(player.Id, created.Id);
    }

    [Fact]
    public async void Create_Invalid_ErrorThrown()
    {
        // arrange
        var player = new Player
        {
            Name = null,
            Created = DateTime.Now,
            Email = null,
            Password = "12345678"
        };

        // act & assert
        await Assert.ThrowsAsync<Exception>(async () => await repository.CreateAsync(player));
    }
    // update player
    [Fact]
    public async void Update_Valid_NoError()
    {
        // arrange
        var player = new Player
        {
            Name = "Jerry",
            Created = DateTime.Now,
            Email = "Jerry@example.com",
            Password = "password"
        };

        var created = await repository.CreateAsync(player);
        created.Name = "Tom";

        // act
        var updated = await repository.UpdateAsync(created);
        await repository.DeleteAsync(updated.Id);

        // assert
        Assert.NotNull(updated);
        Assert.Equal("Tom", updated.Name);
    }
}