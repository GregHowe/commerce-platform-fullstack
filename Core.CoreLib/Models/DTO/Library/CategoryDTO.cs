
namespace Core.CoreLib.Models.DTO.Library
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Keywords { get; set; }
        public int BrandId { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ExpirationDt { get; set; }
    }
}