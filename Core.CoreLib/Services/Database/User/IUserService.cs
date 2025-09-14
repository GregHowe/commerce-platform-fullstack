using Core.CoreLib.Models.Database.Core;
using Core.CoreLib.Models.Database.NYL;
using Core.CoreLib.Models.DTO.User;

namespace Core.CoreLib.Services.Database.User
{
    public interface IUserService
    {
        Task<UserAccessInfo> GetRandomUser(bool prod = false);

        Task<UserAccessInfo> GetUserById(
            string nylId,
            bool prod = false);

        Task<UserAccessInfoDetailDDC?> GetUserAccessInfoDetailDDCData(
          string nylId,
          bool prod = false);

        Task<List<UserLicenseDetailDDC>?> GetUserLicenseInfoDetailDDCData(
           string nylId,
           bool prod = false);

        Task<UserAcceptance?> GetUserAcceptanceData(
           string userObjectId,
           bool prod = false);

        Task<int> SetUserAcceptanceData(
         string userId,
         string userObjectId,
         bool welcomePagePresented,
         bool acceptedTerms,
         bool prod = false);

        Task<UserMigrationInfoDTO> GetUserMigrationInfo(string markerterNo);

        Task<int> SetUserMigrationInfo(UserMigrationInfoDTO userMigrationInfo);

        Task<int> RetireUserMigrationInfo(string markerterNo, bool hardDelete = false);
    }
}