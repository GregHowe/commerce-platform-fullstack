
using Core.CoreLib.Models.DTO.Site;

namespace Core.Backend.Models.Web
{
    public class SiteDTO : CoreLib.Models.DTO.Site.SiteDTO
    {
        public NYLUser User { get; set; }
    }
}