using System.Text.Json.Serialization;

namespace Logic;

public class AuditOperation
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }
    }

