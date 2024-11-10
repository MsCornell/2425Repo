using System.Text.Json.Serialization;

namespace Logic;

public class PlayerAudit
    {
        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("PlayerId")]
        public int PlayerId { get; set; }

        [JsonPropertyName("OperationId")]
        public int OperationId { get; set; }
    }

