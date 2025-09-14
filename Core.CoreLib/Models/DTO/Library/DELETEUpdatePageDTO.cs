using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.Library
{
    public class XXXUpdatePageDTO : AssetIdHandleDTOBase
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Blocks { get; set; }
        public string? Settings { get; set; }
        public string? RedirectUrl { get; set; }
        public string? NavigationTitle { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public bool SeoIsPrivate { get; set; } = false;
        public bool IsPrivate { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int? ParentPageId { get; set; }
    }
}