
using System.Text.Json.Serialization;

namespace Core.CoreLib.Models.Azure.ServiceBus
{
    public class PublishSiteMessage : ServiceBusMessageBase
    {
        [JsonPropertyName("brandId")]
        public int BrandId { get; set; }

        [JsonPropertyName("siteId")]
        public int SiteId { get; set; }

        [JsonPropertyName("action")]
        public string Action  { get; set; }
    }
}
