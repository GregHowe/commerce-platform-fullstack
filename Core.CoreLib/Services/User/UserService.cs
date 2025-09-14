using Azure.Core;
using Azure.Identity;
using Core.CoreLib.Models.Azure.ActiveDirectory;
using Core.CoreLib.Services.Azure.ActiveDirectory;
using Microsoft.Extensions.Configuration;

namespace Core.CoreLib.Services.User
{
    public partial class UserService : IUserService
    {
        protected IActiveDirectoryService _adService;
        protected IConfiguration _configuration;

        private string _tenantId;
        private string _clientId;

        public UserService(
            IActiveDirectoryService adService,
            IConfiguration configuration)
        {
            _adService = adService;
            _configuration = configuration;

            _tenantId = _configuration["AzureAd:TenantId"] ?? string.Empty;
            _clientId = _configuration["AzureAd:ClientId"] ?? string.Empty;
        }

        public async Task<ADUser> GetADUserByIdAsync(string userId) =>
            await
            _adService
            .GetADUserByIdAsync(
                userId);

        public async Task<ADUser> GetADUserAsync(string userEmail) =>
            await
            _adService
            .GetADUserAsync(
                userEmail);

        public async Task<List<ADUser>> GetADUsersAsync(string firstName, string lastName) =>
            await
            _adService
            .GetADUsersAsync(
                firstName,
                lastName);

        public async Task<ADUser> GetSSOADUserAsync(string ssoUserId) =>
            await
            _adService
            .GetSSOADUserAsync(
                ssoUserId);

        public async Task<ADUser> AddADUserAsync(ADUser user) =>
            await
            _adService
            .AddADUserAsync(
                user.GraphUserData.Mail,
                user.GraphUserData.GivenName,
                user.GraphUserData.Surname,
                user.GraphUserData.DisplayName,
                user.GraphUserData.StreetAddress,
                user.GraphUserData.City,
                user.GraphUserData.State,
                user.GraphUserData.PostalCode,
                user.GraphUserData.Country,
                user.GraphUserData.MobilePhone,
                user.GraphUserData.EmployeeType,
                user.BrandId,
                user.ADGroupIds,
                user.SSOId,
                user.GraphUserData.AccountEnabled.HasValue ? user.GraphUserData.AccountEnabled.Value : false,
                user.HasPersonalizedWebsiteAgent,
                user.HasPersonalizedWebsiteRecruiter,
                user.EligibleForPersonalizedWebsite,
                user.EagleAdvisor,
                user.Nautilus,
                user.RegisteredRep,
                user.ApprovedDBA,
                user.LongTermCare,
                user.AARP);

        public async Task UpdateADUserAsync(
            ADUser user)
        {
            await 
            _adService
            .UpdateADUserAsync(
                user);
        }

        public async Task<bool> UserExists(
           ADUser user)
        {
            try
            {
                var exists =
                    await
                    _adService
                    .GetADUserAsync(
                        user.GraphUserData.Mail);

                return
                    exists != null;
            }
            catch (Exception)
            {
                // OK to burry this expected excpetion
                return false;
            }
        }

        public async Task DeleteUserAsync(string userEmail) =>
            await
            _adService
            .DeleteADUser(
                userEmail);

        public async Task SetUserActiveStatusAsync(string userEmail, bool activate = true) =>
            await
            _adService
            .SetUserAccountStatusAsync(
                userEmail,
                activate);

        public async Task<bool> ValidateUserPasswordAsync(
            ADUser user,
            string password)
        {
            // Setup in App registrations > AzureADAuth(id by clientId) > API Permissions
            var scopes = 
                new[] { "APIConnectors.ReadWrite.All" };

            var userNamePasswordCredential =
                new UsernamePasswordCredential(
                user.GraphUserData.UserPrincipalName,
                password,
                _tenantId,
                _clientId,
                new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                });

            var accessToken = 
                await userNamePasswordCredential.GetTokenAsync(new TokenRequestContext(scopes) { });

            return true;
        }
    }
}