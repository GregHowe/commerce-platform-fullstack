
using System.Text.Json.Serialization;

namespace Core.CoreLib.Models.Azure.ServiceBus
{
    public class ServiceBusMessageBase
    {
        // CORE.2.0
        [JsonPropertyName("issuedBy")]
        public string IssuedBy { get; set; }

        [JsonPropertyName("issuedDate")]
        public DateTimeOffset IssuedDate { get; set; }

        // Additional data we may want to stuff on the message
        [JsonPropertyName("arguments")]
        public IDictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();
    }
}
