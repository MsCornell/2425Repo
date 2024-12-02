using System;
using System.Text.Json;

namespace Logic;

public class PlayerWinRateRepository
{
    private readonly string baseUrl;
    private readonly HttpClient http;

    public PlayerWinRateRepository(string baseUrl, HttpClient http)
    {
        this.baseUrl = baseUrl;
        this.http = http;
    }
    // Get win rates based on game mode and start date
        public async Task<IEnumerable<PlayerWinRate>> GetPlayerWinRatesAsync(string gameMode, DateTime? startDate = null)
    {
        var url = $"{baseUrl}?$filter=GameMode eq '{gameMode}'";
        if (startDate.HasValue)
        {
            url += $" and Started ge '{startDate.Value:yyyy-MM-dd}'";
        }
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.PlayerWinRates;
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

