using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.User
{
    public class GetUserRoleDTO : AssetDescriptionDTOBase
    {
        public string Handle { get; set; }
        public List<string>? Permissions { get; set; } = new List<string>();
    }
}