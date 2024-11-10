using System.Text.Json.Serialization;

namespace Logic;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class CharacterRoot
    {
        [JsonPropertyName("value")]
        public List<Character> Characters { get; set; }
    }

