using Newtonsoft.Json;

namespace Core.CoreLib.Models.Database.Core.Settings
{
    public class BrandSettings
    {
        [JsonProperty("sso")]
        public SSOSettings SSO { get; set; }
    }

    public class SSOSettings
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("handle")]
        public string Handle { get; set; }

        [JsonProperty("stagingUrl")]
        public string StagingUrl { get; set; }

        [JsonProperty("prodUrl")]
        public string ProdUrl { get; set; }

        [JsonProperty("urlFailure")]
        public string UrlFailure { get; set; }
    }
}