using System.Text.Json.Serialization;

namespace Logic
{
    public class PlayerWinRate
    {
        [JsonPropertyName("PlayerId")]
        public int PlayerId { get; set; }

        [JsonPropertyName("PlayerName")]
        public string PlayerName { get; set; } = string.Empty;

        // Add Name property, return PlayerName
        [JsonIgnore]
        public string Name => PlayerName;

        [JsonPropertyName("GameMode")]
        public string GameMode { get; set; } = string.Empty;

        [JsonPropertyName("TotalGamesInMode")]
        public int TotalGamesInMode { get; set; }

        [JsonPropertyName("WinsInMode")]
        public int WinsInMode { get; set; }

        [JsonPropertyName("WinRateInMode")]
        public double WinRateInMode { get; set; }

        [JsonPropertyName("TotalScoreInMode")]
        public int TotalScoreInMode { get; set; }

        // Add Score property, return TotalScoreInMode
        [JsonIgnore]
        public int Score => TotalScoreInMode;

        // Add Rank property, return Rank
        [JsonIgnore]
        public int Rank { get; set; }

        [JsonPropertyName("TotalGames")]
        public int TotalGames { get; set; }

        [JsonPropertyName("TotalWins")]
        public int TotalWins { get; set; }

        [JsonPropertyName("OverallWinRate")]
        public double OverallWinRate { get; set; }

        [JsonPropertyName("TotalScore")]
        public int TotalScore { get; set; }

        [JsonPropertyName("AvatarUrl")]
        public string AvatarUrl { get; set; }
    }
}
