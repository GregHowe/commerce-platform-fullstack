using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.Site
{
    public class GetSiteListDTO : AssetIdHandleDTOBase
    {
        public string UserId { get; set; }
        public bool IsPrivate { get; set; } = false;
    }
}