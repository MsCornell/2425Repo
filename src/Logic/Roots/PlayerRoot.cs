using System.Text.Json.Serialization;

namespace Logic;

public class PlayerRoot
{
    [JsonPropertyName("value")]
    public List<Player> Players { get; set; }
}


