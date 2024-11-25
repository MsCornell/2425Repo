using System.Text.Json.Serialization;

namespace Logic;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class GameBoardDetailRoot
    {
        [JsonPropertyName("value")]
        public List<GameBoardDetail> GameBoardDetails { get; set; }
    }

