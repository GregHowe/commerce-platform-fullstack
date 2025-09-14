using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.Library
{
    public class UpdateCategoryDTO : AssetIdHandleDTOBase
    {
        public string Handle { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }  
    }
}
