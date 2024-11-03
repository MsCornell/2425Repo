using System.Text.Json.Serialization;
namespace Logic;

public class Game
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Started")]
        public DateTime Started { get; set; }

        [JsonPropertyName("Ended")]
        public DateTime Ended { get; set; }

        [JsonPropertyName("NextBoard")]
        public int NextBoard { get; set; }

        [JsonPropertyName("NextPlayer")]
        public int NextPlayer { get; set; }

        [JsonPropertyName("AiCharacter")]
        public string AiCharacter { get; set; }

        [JsonPropertyName("PlayerId")]
        public int PlayerId { get; set; }

        [JsonPropertyName("PlayerCharacter")]
        public string PlayerCharacter { get; set; }

        [JsonPropertyName("GameWinner")]
        public string GameWinner { get; set; }
    }

