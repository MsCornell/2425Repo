
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

    public async Task<Character> UpdateCharacterAsync(Character character, string newCharacterName)
    {
        ArgumentNullException.ThrowIfNull(character);
        ArgumentNullException.ThrowIfNull(newCharacterName);
        var updatedCharacter = new Character { CharacterName = newCharacterName };
        //var node = JsonSerializer.SerializeToNode(updatedCharacter);
        var content = JsonContent.Create(updatedCharacter);
        var url = $"{baseUrl}/Character/{character.CharacterName}";
        var response = await http.PutAsync(url, content);
        var root = await GetRootFromResponseAsync(response);
        return root.Characters.Single();
     }
    //     public async Task<Character> UpdateCharacterAsync(Character character, string newCharacterName)
    // {
    //     ArgumentNullException.ThrowIfNull(character);
    //     ArgumentNullException.ThrowIfNull(newCharacterName);
    //     var oldCharacterName = character.CharacterName;
    //     character.CharacterName = newCharacterName;
    //     var node = JsonSerializer.SerializeToNode(character);
    //     var content = JsonContent.Create(node);
    //     var url = $"{baseUrl}/Character/{oldCharacterName}";
    //     var response = await http.PatchAsync(url, content);
    //     var root = await GetRootFromResponseAsync(response);
    //     return root.Characters.Single();
    // }
    
    public async Task DeleteCharacterAsync(string characterName)
    {
        var url = $"{baseUrl}/Character/{characterName}";
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
