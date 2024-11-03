using System.Text.Json.Serialization;

namespace Logic;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class AuditOperationRoot
    {
        [JsonPropertyName("value")]
        public List<AuditOperation> AuditOperations { get; set; }
    }

