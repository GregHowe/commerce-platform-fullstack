
using Core.CoreLib.Models.DTO.Common;

namespace Core.Backend.Models.Web.Preset
{
    public class WebUpdatePresetDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ClientCode { get; set; }
        public string? SMRUCode { get; set; }
        public DateTime? ExpirationDt { get; set; }
        public int? ParentPresetId { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
        public CommonAssetSettings PresetSettings { get; set; }
    }
}