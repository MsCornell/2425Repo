
using System.Net.Http.Json;
using System.Text.Json;

namespace Logic;

public class CharacterRepository
{
    private readonly string baseUrl;
    private readonly HttpClient http = new();

    public CharacterRepository(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }
 
    // no need to add get func for character
    public async Task<Character> CreateCharacterAsync(Character character)
    {
        ArgumentNullException.ThrowIfNull(character);
        var node = JsonSerializer.SerializeToNode(character);
        var content = JsonContent.Create(node);
        var url = $"{baseUrl}";
        var response = await http.PostAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.Characters.Single();
    }
   
    public async Task DeleteCharacterAsync(string characterName)
    {
        var url = $"{baseUrl}/CharacterName/{characterName}";
        var response = await http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    private static async Task<CharacterRoot> GetRootFromResponseAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorRoot>(json);
        if (error is not null && error.Error is not null)
        {
            throw new Exception(error.Error.Message);
        }
        var root = JsonSerializer.Deserialize<CharacterRoot>(json);
        if (root is null)
        {
            throw new Exception("Json is invalid.");
        }
        return root;
    }
}
