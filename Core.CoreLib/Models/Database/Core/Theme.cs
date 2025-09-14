namespace Core.CoreLib.Models.Database.Core
{
    public class Theme
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Settings { get; set; }
        public string? Style { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public int BrandId { get; set; }
    }
}
