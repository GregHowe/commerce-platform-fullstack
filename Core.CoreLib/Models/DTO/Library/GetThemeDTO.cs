
namespace Core.CoreLib.Models.DTO.Library
{
    public class GetThemeDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Style { get; set; }
        public int BrandId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsPrivate { get; set; } = false;
        public string? Settings { get; set; }
    }
}
