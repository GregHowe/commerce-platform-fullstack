using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.Brand
{
    public class UpdateBrandDTO : AssetDescriptionDTOBase
    {
        public string Handle { get; set; }
        public string Host { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Settings { get; set; }
    }
}