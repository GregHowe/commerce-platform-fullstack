using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.User
{
    public class GetUserListDTO : AssetIdHandleDTOBase
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
