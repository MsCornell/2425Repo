using System.Text.Json.Serialization;

namespace Logic;

public class Player
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("OAuthId")]
    public string OAuthId { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Created")]
    public DateTime? Created { get; set; }

    [JsonPropertyName("_username")]
    public string Username { get; set; }

    [JsonPropertyName("_password")]
    public string Password { get; set; }
}

