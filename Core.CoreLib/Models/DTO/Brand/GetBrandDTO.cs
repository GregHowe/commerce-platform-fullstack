using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.Brand
{
    public class GetBrandDTO : AssetIdHandleDTOBase
    {
        public string Host { get; set; }
        public string Settings { get; set; }
    }
}
