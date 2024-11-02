using System.Text.Json.Serialization;

namespace Logic;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class GameBoardRoot
    {
        [JsonPropertyName("value")]
        public List<GameBoard> GameBoards { get; set; }
    }

