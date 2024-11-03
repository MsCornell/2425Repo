using NuGet.Frameworks;

namespace Logic.Tests;

public class PlayerTests
{
    private PlayerRepository repository = new("http://localhost:5000/api/Player");

    [Fact]
    public async void Create_Valid_NoError()
    {
        // arrange
        var player = new Player
        {
            OAuthId = Guid.NewGuid().ToString(),
            Name = "Jerry",
            Created = DateTime.Now,
            Username = Guid.NewGuid().ToString(),
            Password = "password"
        };

        // act
        var created = await repository.CreateAsync(player);
        await repository.DeleteAsync(created.Id);

        // assert
        Assert.NotNull(created);
        Assert.Equal(player.OAuthId, created.OAuthId);
        Assert.NotEqual(player.Id, created.Id);
    }

    [Fact]
    public async void Create_Invalid_ErrorThrown()
    {
        // arrange
        var player = new Player
        {
            OAuthId = null,
            Name = null,
            Created = DateTime.Now,
            Username = null,
            Password = null
        };

        // act & assert
        await Assert.ThrowsAsync<Exception>(async () => await repository.CreateAsync(player));
    }
}