using Core.CoreLib.Models.Azure.ActiveDirectory;

namespace Core.CoreLib.Services.User
{
    public interface IUserService
    {
        Task<ADUser> GetADUserByIdAsync(string userId);
        Task<ADUser> GetADUserAsync(string userEmail);
        Task<List<ADUser>> GetADUsersAsync(string firstName, string lastName);
        Task<ADUser> GetSSOADUserAsync(string ssoUserId);
        Task<ADUser> AddADUserAsync(ADUser user);
        Task UpdateADUserAsync(ADUser user);
        Task<bool> UserExists(ADUser user);
        Task DeleteUserAsync(string userEmail);
        Task SetUserActiveStatusAsync(string userEmail, bool activate = true);
        Task<bool> ValidateUserPasswordAsync(ADUser user, string password);
    }
}
