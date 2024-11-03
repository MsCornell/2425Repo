using System.Text.Json.Serialization;

namespace Logic;

public class Board
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Started")]
        public DateTime Started { get; set; }

        [JsonPropertyName("Ended")]
        public DateTime Ended { get; set; }

        [JsonPropertyName("BoardWinner")]
        public string BoardWinner { get; set; }

        [JsonPropertyName("Cell1")]
        public string Cell1 { get; set; }

        [JsonPropertyName("Cell2")]
        public string Cell2 { get; set; }

        [JsonPropertyName("Cell3")]
        public string Cell3 { get; set; }

        [JsonPropertyName("Cell4")]
        public string Cell4 { get; set; }

        [JsonPropertyName("Cell5")]
        public string Cell5 { get; set; }

        [JsonPropertyName("Cell6")]
        public string Cell6 { get; set; }

        [JsonPropertyName("Cell7")]
        public string Cell7 { get; set; }

        [JsonPropertyName("Cell8")]
        public string Cell8 { get; set; }

        [JsonPropertyName("Cell9")]
        public string Cell9 { get; set; }
    }

