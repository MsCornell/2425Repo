using System.Text.Json.Serialization;

namespace Logic;

public class GameBoard
    {
        [JsonPropertyName("GameId")]
        public int GameId { get; set; }

        [JsonPropertyName("BoardId")]
        public int BoardId { get; set; }

        [JsonPropertyName("Position")]
        public int Position { get; set; }
    }

