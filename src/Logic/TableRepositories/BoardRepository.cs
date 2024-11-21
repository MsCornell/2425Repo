using System;
using System.Net.Http.Json;
using System.Text.Json;

namespace Logic;

public class BoardRepository
{
    private readonly string baseUrl;
    private readonly HttpClient http = new();
    public BoardRepository(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }
    public async Task<Board?> GetBoardAsync(int id)
    {
        var url = $"{baseUrl}/Id/{id}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.Boards.FirstOrDefault();
    }
    public async Task<Board> CreateBoardAsync(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);
        var node = JsonSerializer.SerializeToNode(board);
        node?.AsObject().Remove(nameof(board.Id));
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}";
        var response = await http.PostAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.Boards.Single();
    }
    
    public async Task<Board> UpdateBoardAsync(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);
        var node = JsonSerializer.SerializeToNode(board);
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}/Id/{board.Id}";
        var response = await http.PutAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.Boards.Single();
    }
    public async Task DeleteBoardAsync(int id)
    {
        var url = $"{baseUrl}/Id/{id}";
        var response = await http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
    private static async Task<BoardRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorRoot>(json);
        if (error is not null && error.Error is not null)
        {
            throw new Exception(error.Error.Message);
        }
        var root = JsonSerializer.Deserialize<BoardRoot>(json);
        if (root is null)
        {
            throw new Exception("Json is invalid.");
        }
        return root;
    }
}

