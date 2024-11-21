using System.Text.Json.Serialization;

namespace Logic;

public class GameBoardDetail
    {
        [JsonPropertyName("GameId")]
        public int GameId { get; set; }

        [JsonPropertyName("BoardId")]
        public int BoardId { get; set; }

        [JsonPropertyName("BoardWinner")]
        public string BoardWinner { get; set; }
    }

