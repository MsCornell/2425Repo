using System.Text.Json.Serialization;

namespace Logic;

public class Error
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }
}

