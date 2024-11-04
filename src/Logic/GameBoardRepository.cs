using System.Net.Http.Json;
using System.Text.Json;

namespace Logic;

public class GameBoardRepository
{
    private readonly string baseUrl;
    private readonly HttpClient http = new();

    public GameBoardRepository(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }

    public async Task<GameBoard?> GetGameBoardAsync(int gameId, int boardId)
    {
        var url = $"{baseUrl}/GameId/{gameId}/BoardId/{boardId}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.GameBoards.FirstOrDefault();
    }
    public async Task<GameBoard> CreateGameBoardAsync(GameBoard gameBoard)
    {
        ArgumentNullException.ThrowIfNull(gameBoard);
        var node = JsonSerializer.SerializeToNode(gameBoard);
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}";
        var response = await http.PostAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.GameBoards.Single();
    }
    public async Task<GameBoard> UpdateGameBoardAsync(GameBoard gameBoard)
    {
        ArgumentNullException.ThrowIfNull(gameBoard);
        var node = JsonSerializer.SerializeToNode(gameBoard);
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}/GameId/{gameBoard.GameId}/BoardId/{gameBoard.BoardId}";
        var response = await http.PutAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.GameBoards.Single();
    }
    // delete gameboard by id
    public async Task DeleteGameBoardAsync(int gameId, int boardId)
    {
        var url = $"{baseUrl}/GameId/{gameId}/BoardId/{boardId}";
        var response = await http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
    // delete gameboard by object
    public async Task DeleteGameBoardAsync(GameBoard gameBoard)
    {

        var url = $"{baseUrl}/GameId/{gameBoard.GameId}/BoardId/{gameBoard.BoardId}";
        var response = await http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    private static async Task<GameBoardRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorRoot>(json);
        if (error is not null && error.Error is not null)
        {
            throw new Exception(error.Error.Message);
        }
        var root = JsonSerializer.Deserialize<GameBoardRoot>(json);
        if (root is null)
        {
            throw new Exception("Json is invalid.");
        }
        return root;
    }
}

