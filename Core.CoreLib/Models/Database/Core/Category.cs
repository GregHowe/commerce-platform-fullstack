namespace Core.CoreLib.Models.Database.Core
{
    public class Category
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public int BrandId { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
