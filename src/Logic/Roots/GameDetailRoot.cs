using System.Text.Json.Serialization;

namespace Logic;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class GameDetailRoot
    {
        [JsonPropertyName("value")]
        public List<GameDetail> GameDetails { get; set; }
    }

