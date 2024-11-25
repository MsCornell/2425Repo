using System;
using System.Text.Json;

namespace Logic;

public class GameDetailRepository
{
    private readonly string baseUrl;
    private readonly HttpClient http = new();

    public GameDetailRepository(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }
    public async Task<GameDetail?> GetOneGameDetailAsync(int gameId)
    {
        var url = $"{baseUrl}/GameId/{gameId}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.GameDetails.FirstOrDefault();
    }
    public async Task<IEnumerable<GameDetail>?> GetAllGameDetailsAsync()
    {
        var url = $"{baseUrl}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.GameDetails;
    }
    private static async Task<GameDetailRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        var error = JsonSerializer.Deserialize<ErrorRoot>(json);
        if (error is not null && error.Error is not null)
        {
            throw new Exception(error.Error.Message);
        }

        var root = JsonSerializer.Deserialize<GameDetailRoot>(json);
        if (root is null)
        {
            throw new Exception("Json is invalid.");
        }
        return root;
    }
}



