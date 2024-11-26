using System.Net.Http.Json;
using System.Text.Json;

namespace Logic;

public class GameRepository
{
    private readonly string baseUrl;
    private readonly static HttpClient http = new HttpClient();

     public GameRepository(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }

    public GameRepository()
    {
    }

    public async Task<Game?> GetGameAsync(int id)
    {
        var url = $"{baseUrl}/Id/{id}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.Games.FirstOrDefault();
    }

    public async Task<Game> CreateGameAsync(Game game)
    {
        ArgumentNullException.ThrowIfNull(game);

        var node = JsonSerializer.SerializeToNode(game);
        node?.AsObject().Remove(nameof(game.Id));
        var content = JsonContent.Create(node);

        var url = $"{baseUrl}";
        var response = await http.PostAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.Games.Single();
    }

    public async Task<Game> UpdateGameAsync(Game game)
    {
        ArgumentNullException.ThrowIfNull(game);
        var node = JsonSerializer.SerializeToNode(game);
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}/Id/{game.Id}";
        var response = await http.PutAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.Games.Single();
    }
    public async Task DeleteGameAsync(int id)
    {
        var url = $"{baseUrl}/Id/{id}";
        var response = await http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    private static async Task<GameRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        var error = JsonSerializer.Deserialize<ErrorRoot>(json);
        if (error is not null && error.Error is not null)
        {
            throw new Exception(error.Error.Message);
        }

        var root = JsonSerializer.Deserialize<GameRoot>(json);
        if (root is null)
        {
            throw new Exception("Json is invalid.");
        }

        return root;
    }
}
