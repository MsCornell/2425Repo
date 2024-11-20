using System.Text.Json.Serialization;

namespace Logic;

public class Board
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("BoardWinner")]
        public string BoardWinner { get; set; }
    }
