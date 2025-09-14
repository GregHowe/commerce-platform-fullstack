namespace Core.CoreLib.Models.DTO.Authentication
{
    public class GetAuthTokenDTO : AuthRefreshDTO
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
