using System.Text.Json.Serialization;

namespace Logic;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class PlayerAuditRoot
    {
        [JsonPropertyName("value")]
        public List<PlayerAudit> PlayerAudits { get; set; }
    }

