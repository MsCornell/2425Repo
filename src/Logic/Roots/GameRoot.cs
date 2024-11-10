using System.Text.Json.Serialization;
namespace Logic;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class GameRoot
    {
        [JsonPropertyName("value")]
        public List<Game> Games { get; set; }
    }

