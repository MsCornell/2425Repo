using System;
using System.Text.Json;

namespace Logic;

public class GameBoardDetailRepository
{
    private readonly string baseUrl;
    private readonly HttpClient http = new();

    public GameBoardDetailRepository(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }
    public async Task<GameBoardDetail?> GetOneGameBoardAsync(int gameId,int boardId)
    {
        var url = $"{baseUrl}/GameId/{gameId}/BoardId/{boardId}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.GameBoardDetails.FirstOrDefault();
    }
    public async Task<IEnumerable<GameBoardDetail>?> GetAllGameBoardsAsync()
    {
        var url = $"{baseUrl}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.GameBoardDetails;
    }
    private static async Task<GameBoardDetailRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        var error = JsonSerializer.Deserialize<ErrorRoot>(json);
        if (error is not null && error.Error is not null)
        {
            throw new Exception(error.Error.Message);
        }

        var root = JsonSerializer.Deserialize<GameBoardDetailRoot>(json);
        if (root is null)
        {
            throw new Exception("Json is invalid.");
        }
        return root;
    }
}


