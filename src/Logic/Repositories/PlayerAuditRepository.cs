using System;
using System.Net.Http.Json;
using System.Text.Json;

namespace Logic;

public class PlayerAuditRepository
{
    private readonly string baseUrl;
    private readonly HttpClient http = new();
    public PlayerAuditRepository(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }

        public async Task<PlayerAudit> GetPlayerAuditAsync(int playerId, int operationId)
    {
        var url = $"{baseUrl}/PlayerId/{playerId}/OperationId/{operationId}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.PlayerAudits.FirstOrDefault();
    }

    public async Task<PlayerAudit> CreatePlayerAuditAsync(PlayerAudit playerAudit)
    {
        ArgumentNullException.ThrowIfNull(playerAudit);
        var node = JsonSerializer.SerializeToNode(playerAudit);
        node?.AsObject().Remove(nameof(playerAudit.PlayerId));
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}";
        var response = await http.PostAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.PlayerAudits.Single();
    }
    
    public async Task<PlayerAudit> UpdatePlayerAuditAsync(PlayerAudit playerAudit)
    {
        ArgumentNullException.ThrowIfNull(playerAudit);
        var node = JsonSerializer.SerializeToNode(playerAudit);
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}/PlayerId/{playerAudit.PlayerId}/OperationId/{playerAudit.OperationId}";
        var response = await http.PutAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.PlayerAudits.Single();
    }

    public async Task DeletePlayerAuditAsync(int playerId, int operationId)
    {
        var url = $"{baseUrl}/PlayerId/{playerId}/OperationId/{operationId}";
        var response = await http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    private static async Task<PlayerAuditRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorRoot>(json);
        if (error is not null && error.Error is not null)
        {
            throw new Exception(error.Error.Message);
        }
        var root = JsonSerializer.Deserialize<PlayerAuditRoot>(json);
        if (root is null)
        {
            throw new Exception("Deserialization failed");
        }
        return root;
    }
}

