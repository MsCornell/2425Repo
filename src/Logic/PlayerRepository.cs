using System.Net.Http.Json;
using System.Text.Json;

namespace Logic;

public class PlayerRepository
{
    private readonly string baseUrl;
    private readonly HttpClient http = new();

    public PlayerRepository(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }

    public async Task<Player?> GetAsync(int Id)
    {
        var url = $"{baseUrl}/Id/{Id}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.Players.FirstOrDefault();
    }

    public async Task<Player?> GetAsync(string username, string password)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(username);
        ArgumentNullException.ThrowIfNullOrEmpty(password);

        var url = $"{baseUrl}?$filter=_username eq '{username}' and _password eq '{password}'";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.Players.FirstOrDefault();
    }

    public async Task<Player> CreateAsync(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);

        var node = JsonSerializer.SerializeToNode(player);
        node?.AsObject().Remove(nameof(player.Id));
        var content = JsonContent.Create(node);

        var url = $"{baseUrl}";
        var response = await http.PostAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.Players.Single();
    }

    public async Task<Player> UpdateAsync(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);

        var node = JsonSerializer.SerializeToNode(player);
        node?.AsObject().Remove(nameof(player.Id));
        var content = JsonContent.Create(node);

        var url = $"{baseUrl}/Id/{player.Id}";
        var response = await http.PutAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.Players.Single();
    }

    public async Task DeleteAsync(int id)
    {
        var url = $"{baseUrl}/Id/{id}";
        var response = await http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    private static async Task<PlayerRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        var error = JsonSerializer.Deserialize<ErrorRoot>(json);
        if (error is not null && error.Error is not null)
        {
            throw new Exception(error.Error.Message);
        }

        var root = JsonSerializer.Deserialize<PlayerRoot>(json);
        if (root is null)
        {
            throw new Exception("Json is invalid.");
        }

        return root;
    }
}