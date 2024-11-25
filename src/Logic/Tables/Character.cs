using System.Text.Json.Serialization;

namespace Logic;

public class Character
    {
        [JsonPropertyName("CharacterName")]
        public string CharacterName { get; set; }
    }

