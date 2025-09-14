
using Core.CoreLib.Models.Azure.ActiveDirectory;

namespace Core.CoreLib.Services.Azure.ActiveDirectory
{
    public interface IActiveDirectoryService
    {
        Task<ADUser> GetADUserByIdAsync(
           string userId);

        Task<ADUser> GetADUserAsync(
            string userEmail);

        Task<List<ADUser>> GetADUsersAsync(
          string firstName,
          string lastName);

        Task<ADUser> GetSSOADUserAsync(
            string ssoUserId);

        Task<ADUser> AddADUserAsync(
            string userEmail,
            string firstName,
            string lastName,
            string displayName,
            string address,
            string city,
            string state,
            string postalCode,
            string country,
            string primaryPhone,
            string employeeType,
            string brandId,
            List<string> adGroupIds,
            string ssoUserId,
            bool active,
            bool hasPersonalizedWebsiteAgent,
            bool hasPersonalizedWebsiteRecruiter,
            bool eligibleForPersonalizedWebsite,
            bool eagleAdvisor,
            bool nautilus,
            bool registeredRep,
            bool approvedDBA,
            bool longTermCare,
            bool aarp);

        Task DeleteADUser(
            string userEmail);

        Task SetUserAccountStatusAsync(
            string userEmail,
            bool active = true);

        Task UpdateADUserAsync(
            ADUser user);

        string UpdateUserPassword(
            ADUser user);
    }
}