
using Core.CoreLib.Models.DTO.Library;
using Core.CoreLib.Models.DTO.User;

namespace Core.CoreLib.Models.DTO.Site
{
    public class SiteDTO
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public bool SeoIsPrivate { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public int? BrandId { get; set; }
        public int EnvironmentId { get; set; }
        public string? UserId { get; set; }
        public string? UserObjectId { get; set; }
        public int? HomepageId { get; set; }
        public string? Settings { get; set; }
        public string? Footer { get; set; }
        public string? Navigation { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Retired { get; set; }
        public string? Style { get; set; }
        
        public List<PageDTO>? Pages { get; set; } = new List<PageDTO>();
        public List<PresetDTO>? Presets { get; set; } = new List<PresetDTO>();
        public List<GetUserRoleDTO>? Seats { get; set; } = new List<GetUserRoleDTO>();
        public SiteThemesDTO Themes { get; set; }
    }
}