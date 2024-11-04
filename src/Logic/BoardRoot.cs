using System.Text.Json.Serialization;

namespace Logic;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class BoardRoot
    {
        [JsonPropertyName("value")]
        public List<Board> Boards { get; set; }
    }

