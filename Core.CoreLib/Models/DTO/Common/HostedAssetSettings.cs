
using System.Text.Json.Serialization;

namespace Core.CoreLib.Models.DTO.Common
{
    public class HostedAssetSettings
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Url { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FileName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FileType { get; set; }
    }
}