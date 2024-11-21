using System.Text.Json.Serialization;

namespace Logic;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class PlayerWinRateRoot
    {
        [JsonPropertyName("value")]
        public List<PlayerWinRate> PlayerWinRates { get; set; }
    }

