using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.Authentication
{
    public class GetUserAuthDTO : BrandedDTOBase
    {
        public string Password { get; set; }
    }
}