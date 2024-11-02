using System.Text.Json.Serialization;

namespace Logic;

public class Character
    {
        [JsonPropertyName("Character")]
        public string CharacterName { get; set; }
    }

