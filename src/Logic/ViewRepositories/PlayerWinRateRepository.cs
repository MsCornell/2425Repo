using System;
using System.Text.Json;

namespace Logic;

public class PlayerWinRateRepository
{
    private readonly string baseUrl;
    private readonly HttpClient http = new();

    public PlayerWinRateRepository(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }
    public async Task<PlayerWinRate?> GetOnePlayerAsync(int playerId)
    {
        var url = $"{baseUrl}/PlayerId/{playerId}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.PlayerWinRates.FirstOrDefault();
    }
    public async Task<IEnumerable<PlayerWinRate>?> GetAllPlayerAsync()
    {
        var url = $"{baseUrl}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.PlayerWinRates;
    }
    private static async Task<PlayerWinRateRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        var error = JsonSerializer.Deserialize<ErrorRoot>(json);
        if (error is not null && error.Error is not null)
        {
            throw new Exception(error.Error.Message);
        }

        var root = JsonSerializer.Deserialize<PlayerWinRateRoot>(json);
        if (root is null)
        {
            throw new Exception("Json is invalid.");
        }

        return root;
    }
}

