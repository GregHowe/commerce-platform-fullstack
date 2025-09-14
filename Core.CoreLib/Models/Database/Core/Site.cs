namespace Core.CoreLib.Models.Database.Core
{
    public class Site
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Settings { get; set; }
        public string? Style { get; set; }
        public string? Navigation { get; set; }
        public string? Footer { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public bool SeoIsPrivate { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public int BrandId { get; set; }
        public string UserId { get; set; }
        public int? HomepageId { get; set; }
    }
}
