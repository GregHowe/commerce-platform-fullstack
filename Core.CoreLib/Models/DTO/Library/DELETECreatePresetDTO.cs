using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.Library
{
    public class XXXCreatePresetDTO : AssetDescriptionDTOBase
    {
        public string Type { get; set; }
        public string? Settings { get; set; }
        public string? ClientCode { get; set; }
        public bool IsPrivate { get; set; } = false;
        public int? BrandId { get; set; }
        public int? SiteId { get; set; }
	}
}