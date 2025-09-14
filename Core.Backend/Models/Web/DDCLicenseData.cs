
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Backend.Models.Web
{
    public class DDCLicenseData
    {
        [JsonPropertyName("busLicenseTpCode")]
        public string BusLicenseTpCode { get; set; }

        [JsonPropertyName("busEntityCode")]
        public string BusEntityCode { get; set; }

        [JsonPropertyName("licenseExpiryDt")]
        public string LicenseExpiryDt { get; set; }

        [JsonPropertyName("licenseIdNumber")]
        public string LicenseIdNumber { get; set; }

        [JsonPropertyName("licenseIssueDt")]
        public string LicenseIssueDt { get; set; }

        [JsonPropertyName("licenseLobCode")]
        public string LicenseLobCode { get; set; }

        [JsonPropertyName("licenseTpCode")]
        public string LicenseTpCode { get; set; }

        [JsonPropertyName("licenseTpDescription")]
        public string LicenseTpDescription { get; set; }

        [JsonPropertyName("marketerStatusTpCode")]
        public string MarketerStatusTpCode { get; set; }

        [JsonPropertyName("marketerStatusTpDesc")]
        public string MarketerStatusTpDesc { get; set; }

        [JsonPropertyName("stateCountyCode")]
        public string StateCountyCode { get; set; }

        [JsonPropertyName("agentData")]
        public string AgentData { get; set; }

        [JsonPropertyName("eagleData")]
        public string EagleData { get; set; }
    }
}