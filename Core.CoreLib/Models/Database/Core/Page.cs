namespace Core.CoreLib.Models.Database.Core
{
    public class Page
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
        public bool SeoIsPrivate { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public int SiteId { get; set; }
        public int? ParentPageId { get; set; }
    }
}
