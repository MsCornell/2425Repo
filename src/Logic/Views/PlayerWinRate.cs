using System.Text.Json.Serialization;

namespace Logic;

public class PlayerWinRate
    {
        [JsonPropertyName("PlayerId")]
        public int PlayerId { get; set; }

        [JsonPropertyName("PlayerName")]
        public string PlayerName { get; set; }

        [JsonPropertyName("GameMode")]
        public string GameMode { get; set; }

        [JsonPropertyName("TotalGamesInMode")]
        public int TotalGamesInMode { get; set; }

        [JsonPropertyName("WinsInMode")]
        public int WinsInMode { get; set; }

        [JsonPropertyName("WinRateInMode")]
        public double WinRateInMode { get; set; }

        [JsonPropertyName("TotalGames")]
        public int TotalGames { get; set; }

        [JsonPropertyName("TotalWins")]
        public int TotalWins { get; set; }

        [JsonPropertyName("OverallWinRate")]
        public double OverallWinRate { get; set; }
    }

