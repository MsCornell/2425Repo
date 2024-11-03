using System;
using System.Net.Http.Json;
using System.Text.Json;

namespace Logic;

public class AuditOperationRepository
{
    private readonly string baseUrl;
    private readonly HttpClient http = new();
    public AuditOperationRepository(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }

    public async Task<AuditOperation> GetAuditOperationAsync(int id)
    {
        var url = $"{baseUrl}/Id/{id}";
        var response = await http.GetAsync(url);
        var root = await GetRootFromResponseAsync(response);
        return root.AuditOperations.FirstOrDefault();
    }
    public async Task<AuditOperation> CreateAuditOperationAsync(AuditOperation auditOperation)
    {
        ArgumentNullException.ThrowIfNull(auditOperation);
        var node = JsonSerializer.SerializeToNode(auditOperation);
        node?.AsObject().Remove(nameof(auditOperation.Id));
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}";
        var response = await http.PostAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.AuditOperations.Single();
    }

    public async Task<AuditOperation> UpdateAuditOperationAsync(AuditOperation auditOperation)
    {
        ArgumentNullException.ThrowIfNull(auditOperation);
        var node = JsonSerializer.SerializeToNode(auditOperation);
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}/Id/{auditOperation.Id}";
        var response = await http.PutAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.AuditOperations.Single();
    }
    public async Task<AuditOperation> UpdateAuditOperationAsync(string auditOperationName)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(auditOperationName);
        var auditOperation = new AuditOperation { Name = auditOperationName };
        var node = JsonSerializer.SerializeToNode(auditOperation);
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}?$filter=Name eq '{auditOperationName}'";
        var response = await http.PutAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.AuditOperations.Single();
    }

    public async Task DeleteAuditOperationAsync(int id)
    {
        var url = $"{baseUrl}/Id/{id}";
        var response = await http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
    public async Task DeleteAuditOperationAsync(string auditOperationName)
    {
        var url = $"{baseUrl}?$filter=Name eq '{auditOperationName}'";
        var response = await http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

        private static async Task<AuditOperationRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorRoot>(json);
        if (error is not null && error.Error is not null)
        {
            throw new Exception(error.Error.Message);
        }
        var root = JsonSerializer.Deserialize<AuditOperationRoot>(json);
        if (root is null)
        {
            throw new Exception("Json is invalid.");
        }
        return root;
    }
}

