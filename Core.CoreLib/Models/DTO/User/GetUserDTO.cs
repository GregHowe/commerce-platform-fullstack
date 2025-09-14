using Core.CoreLib.Models.DTO.Base;

namespace Core.CoreLib.Models.DTO.User
{
    public class GetUserDTO : BrandedDTOBase
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Settings { get; set; }

        // public List<GetUserRoleDTO>? Roles { get; set; } = new List<GetUserRoleDTO>();
        // public List<GetUserRoleDTO>? Seats { get; set; } = new List<GetUserRoleDTO>();
        // public List<GetSiteListDTO>? Sites { get; set; } = new List<GetSiteListDTO>();
    }
}
