
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.CoreLib.Models.DTO.Common;

namespace Core.CoreLib.Models.DTO.Library
{
    public class XXXUpdatePresetDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ClientCode { get; set; }
        public string? SMRUCode { get; set; }
        public DateTime? ExpirationDt { get; set; }

        [NotMapped]
        public List<int> CategoryIds { get; set; } = new List<int>();

        [JsonIgnore]
        public string? Settings { get; set; }

        [NotMapped]
        public CommonAssetSettings PresetSettings
        {
            get =>
                !string.IsNullOrEmpty(Settings) ?
                    JsonSerializer.Deserialize<CommonAssetSettings>(Settings) ?? new CommonAssetSettings() :
                    new CommonAssetSettings();

            set => Settings = JsonSerializer.Serialize(value);
        }
    }
}