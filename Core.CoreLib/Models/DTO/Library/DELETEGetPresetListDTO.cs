using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.Library
{
    public class XXXGetPresetListDTO : AssetIdHandleDTOBase
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ClientCode { get; set; }
        public bool IsPrivate { get; set; } = false;
    }
}