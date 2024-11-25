using System.Text.Json.Serialization;

namespace Logic;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class MLRoot
{
    [JsonPropertyName("value")]
    public List<string> NextMove { get; set; }
}