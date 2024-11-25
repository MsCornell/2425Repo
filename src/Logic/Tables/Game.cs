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

        [JsonPropertyName("AiCharacter")]
        public bool AiCharacter { get; set; }

        [JsonPropertyName("PlayerId")]
        public int PlayerId { get; set; }

        [JsonPropertyName("PlayerCharacter")]
        public string PlayerCharacter { get; set; }

        [JsonPropertyName("GameWinner")]
        public string GameWinner { get; set; }

        [JsonPropertyName("GameMode")]
        public string GameMode { get; set; }

        [JsonPropertyName("GameScore")]
        public int GameScore { get; set; }
    }
