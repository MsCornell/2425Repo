using System.Text.Json.Serialization;

namespace Logic;

public class ErrorRoot
{
    [JsonPropertyName("error")]
    public Error Error { get; set; }
}

