
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Core.CoreLib.Models.DTO.Common;

namespace Core.CoreLib.Models.DTO.Library
{
    public class PresetDTO
    {
        [Key]
        public int Id { get; set; }
        public string? Handle { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ClientCode { get; set; }
        public string? SMRUCode { get; set; }
        public DateTime? ExpirationDt { get; set; }
        public string? Settings { get; set; }
        public string Type { get; set; }
        public bool AllowAgent { get; set; }
        public bool AllowDBA { get; set; }
        public bool AllowEagle { get; set; }
        public bool AllowNautilus { get; set; }
        public bool AllowRegisteredRep { get; set; }
        public bool AllowGO { get; set; }
        public bool AllowHO { get; set; }
        public bool AllowLongTermCare { get; set; }
        public bool AllowBusinessSolutions { get; set; }
        public bool AllowAARP { get; set; }
        public int? SiteId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPrivate { get; set; }
        public int? BrandId { get; set; }
        public DateTime CreatedDt { get; set; }
        public int? ParentPresetId { get; set; }

        [NotMapped]
        public CommonAndHostedAssetSettings PresetSettings
        {
            get =>
                !string.IsNullOrEmpty(Settings) ?
                    JsonSerializer.Deserialize<CommonAndHostedAssetSettings>(Settings) ?? new CommonAndHostedAssetSettings() :
                    new CommonAndHostedAssetSettings();

            set => Settings = JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public List<int> CategoryIds { get; set; } = new List<int>();

        // 1:Many, can be empty, should not be null
        [NotMapped]
        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
    }
}