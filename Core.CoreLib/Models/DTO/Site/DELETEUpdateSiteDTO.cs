using Core.CoreLib.Models.DTO.Base;
using Core.CoreLib.Models.DTO.Library;

namespace Core.CoreLib.Models.DTO.Site
{
    public class XXXUpdateSiteDTO : AssetDescriptionDTOBase
    {
        public string Handle { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Settings { get; set; }
        public string? Style { get; set; }
        public string? Navigation { get; set; }
        public string? Footer { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public bool SeoIsPrivate { get; set; } = false;
        public bool IsPrivate { get; set; } = false;
        public int? HomepageId { get; set; }
        public List<PageDTO> Pages { get; set; } = new List<PageDTO>();
        public SiteThemesDTO Themes { get; set; }
        //public List<SiteThemesDTO> Themes { get; set; } = new List<SiteThemesDTO>();
    }
}