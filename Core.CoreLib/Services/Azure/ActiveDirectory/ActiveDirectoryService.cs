
using Core.CoreLib.Models.Azure.ActiveDirectory;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Models.Exceptions;
using Core.CoreLib.Services.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Core.CoreLib.Services.Azure.ActiveDirectory
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        protected IPasswordService _passwordService;
        protected IConfiguration _configuration;

        private GraphServiceClient? _graphClient;
        private string _tenantId;
        private string _clientId;
        private string _clientSecret;

        public ActiveDirectoryService(
            IPasswordService passwordService,
            IConfiguration configuration)
        {
            _passwordService = passwordService;
            _configuration = configuration;

            _tenantId = _configuration["AzureAd:TenantId"] ?? string.Empty;
            _clientId = _configuration["AzureAd:ClientId"] ?? string.Empty;
            _clientSecret = _configuration["AzureAd:ClientSecret"] ?? string.Empty;
        }

        private async Task<GraphServiceClient> GetGraphServiceClientAsync()
        {
            if (_graphClient != null)
                return _graphClient;

            // Configure app builder
            var authority = $"https://login.microsoftonline.com/{_tenantId}";
            var app = 
                ConfidentialClientApplicationBuilder
                .Create(_clientId)
                .WithClientSecret(_clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();

            // Acquire tokens for Graph API
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            //var scopes =
             //   new[] { "APIConnectors.ReadWrite.All" };
            //Directory.AccessAsUser.All

            var authenticationResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();

            // Create GraphClient and attach auth header to all request (acquired on previous step)
            _graphClient = 
                new GraphServiceClient(
                    new DelegateAuthenticationProvider(requestMessage =>
                    {
                        requestMessage.Headers.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", authenticationResult.AccessToken);

                        return Task.FromResult(0);
                    }));

            return _graphClient;
        }

        private async Task<ADUser> GetADUserByFilterAsync(
            string filter)
        {
            var graphClient =
                await GetGraphServiceClientAsync();

            var user =
                (await 
                graphClient
                .Users
                .Request()
                .Filter(filter)
                .Select(MasterADUserFieldList)
                .GetAsync())
                .FirstOrDefault();

            if (user == null)
                throw new DataNotFoundException("User not found");

            // Our users must have these extentions set, errors here should be investigated
            user.Extensions =
                await
                graphClient
                .Users[user.Id]
                .Extensions
                .Request()
                .GetAsync();

            return
                new ADUser()
                {
                    GraphUserData = user,
                };
        }

        public async Task<ADUser> GetADUserByIdAsync(
           string userId) =>
               await
               GetADUserByFilterAsync(
                   $"id eq '{userId}'");

        private async Task<List<ADUser>> GetADUsersByFilterAsync(
            string filter)
        {
            var graphClient =
                await GetGraphServiceClientAsync();

            var users =
                (await
                graphClient
                .Users
                .Request()
                .Filter(filter)
                .Select(MasterADUserFieldList)
                .GetAsync())
                .ToList();

            if (users == null ||
                users.Count == 0)
                return new List<ADUser>();

            var result = new List<ADUser>();

            foreach (var user in users)
            {
                // Our users must have these extentions set, errors here should be investigated
                user.Extensions =
                    await
                    graphClient
                    .Users[user.Id]
                    .Extensions
                    .Request()
                    .GetAsync();

                result.Add(
                    new ADUser()
                    {
                        GraphUserData = user,
                    });
            }

            return result;
        }

        public async Task<ADUser> GetADUserAsync(
           string userEmail) =>
               await
               GetADUserByFilterAsync(
                   $"{AzureADFields.Mail} eq '{userEmail}'");

        public async Task<List<ADUser>> GetADUsersAsync(
           string firstName,
           string lastName) =>
               await
               GetADUsersByFilterAsync(
                   $"{AzureADFields.GivenName} eq '{firstName}' and {AzureADFields.Surname} eq '{lastName}'");


        public async Task<ADUser> GetSSOADUserAsync(
            string ssoUserId) =>
                await
                GetADUserByFilterAsync(
                    $"{AzureADFields.EmployeeID} eq '{ssoUserId}'");

        public async Task DeleteADUser(
            string userEmail)
        {
            var user =
                await
                GetADUserByFilterAsync(
                    $"{AzureADFields.Mail} eq '{userEmail}'");

            await 
                (await GetGraphServiceClientAsync())
                .Users[user.GraphUserData.Id]
                .Request()
                .DeleteAsync();
        }

        private const string MasterADUserFieldList =
            $"{AzureADFields.Id},{AzureADFields.GivenName},{AzureADFields.Surname},{AzureADFields.StreetAddress},{AzureADFields.City},{AzureADFields.State},{AzureADFields.PostalCode},{AzureADFields.Country},{AzureADFields.AccountEnabled}," +
            $"{AzureADFields.Mail},{AzureADFields.MobilePhone},{AzureADFields.CreatedDateTime},{AzureADFields.CompanyName},{AzureADFields.JobTitle},{AzureADFields.DisplayName},{AzureADFields.MailNickName}," +
            $"{AzureADFields.UserPrincipalName},{AzureADFields.PasswordProfile},{AzureADFields.EmployeeType},{AzureADFields.EmployeeID},{AzureADFields.AdditionalData}";

        public async Task<ADUser> AddADUserAsync(
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
            bool aarp)
        {
            if (string.IsNullOrWhiteSpace(userEmail) ||
                !userEmail.Contains("@"))
                throw new BadRequestException("Email address missing or invalid");

            var mailNickname =
                userEmail.Split(new char[] { '@' })[0];

            // TODO: This should be stored in a DB table correlated to BrandId
            // CoreDb Brand table i think
            const string verfiedADBrandDomain =
                "nylcoreplatform.onmicrosoft.com";

            var pword =
                _passwordService.GeneratePassword(userEmail);

            var newUser = 
                await
                (await GetGraphServiceClientAsync())
                .Users
                .Request()
                .AddAsync(
                    new Microsoft.Graph.User
                    {
                        // !!! When adding fields here, add to MasterADUserFieldList so fields are returned on GetADUser !!!
                        DisplayName = displayName,
                        GivenName = firstName,
                        Surname = lastName,
                        StreetAddress = address,
                        City = string.IsNullOrWhiteSpace(city) ? "None" : city,
                        State = state,
                        PostalCode = postalCode,
                        Country = country,
                        AccountEnabled = active,
                        Mail = userEmail,
                        MobilePhone = string.IsNullOrWhiteSpace(primaryPhone) ? "555" : primaryPhone,
                        CreatedDateTime = DateTime.Now,
                        MailNickname = mailNickname,
                        UserPrincipalName = $"{mailNickname}@{verfiedADBrandDomain}",
                        EmployeeType = !string.IsNullOrWhiteSpace(employeeType) ? employeeType : string.Empty,
                        EmployeeId = DetermineSSOId(ssoUserId),
                        PasswordProfile =
                            new PasswordProfile
                            {
                                ForceChangePasswordNextSignIn = false,
                                // Password should be generated unless a specific need arises
                                Password = pword
                            }
                    });

            // Add our custom user attributes
            await
            AddCustomAttributesAsync(
                newUser.Id,
                new Dictionary<string, object>
                {
                    { AzureADCustomAttributes.BrandId, brandId },
                    { AzureADCustomAttributes.SSOId, DetermineSSOId(ssoUserId) },
                    { AzureADCustomAttributes.HasPersonalizedWebsiteAgent, hasPersonalizedWebsiteAgent },
                    { AzureADCustomAttributes.HasPersonalizedWebsiteRecruiter, hasPersonalizedWebsiteRecruiter },
                    { AzureADCustomAttributes.EligibleForPersonalizedWebsite, eligibleForPersonalizedWebsite },
                    { AzureADCustomAttributes.EagleAdvisor, eagleAdvisor },
                    { AzureADCustomAttributes.Nautilus, nautilus },
                    { AzureADCustomAttributes.RegisteredRep, registeredRep },
                    { AzureADCustomAttributes.ApprovedDBA, approvedDBA },
                    { AzureADCustomAttributes.LongTermCare, longTermCare },
                    { AzureADCustomAttributes.AARP, aarp }
                });

            // Add groups
            await 
            AddUserToADGroup(newUser, adGroupIds);

            return
                await
                GetADUserAsync(userEmail);
        }

        private async Task AddUserToADGroup(
            Microsoft.Graph.User user,
            List<string> adGroupIds)
        {
            // At present there is no need for the application to have any knowledge of groups so there is not support built to view/remove/auth based on membership.
            // We are only supporting add to group as part of user ingest.
            // This application will never remove a user from a group, that is determined to be an human responsibility.
            foreach (var adGroupId in adGroupIds ?? new List<string>())
                if ((await (await GetGraphServiceClientAsync()).Users[user.Id].CheckMemberGroups(new List<string> { adGroupId }).Request().PostAsync()).FirstOrDefault() == null)
                    await (await GetGraphServiceClientAsync()).Groups[adGroupId].Members.References.Request().AddAsync(user);
        }

        private string DetermineSSOId(string ssoUserId) =>
            !string.IsNullOrWhiteSpace(ssoUserId) ? ssoUserId : General.NONSSO_EmployeeId;

        public async Task UpdateADUserAsync(
            ADUser user)
        {
            // Will default to these if customattriutes is null
            var brandId = user.BrandId;
            var ssoId = user.SSOId;
            var hasPersonalizedWebsiteAgent = user.HasPersonalizedWebsiteAgent;
            var hasPersonalizedWebsiteRecruiter = user.HasPersonalizedWebsiteRecruiter;
            var eligibleForPersonalizedWebsite = user.EligibleForPersonalizedWebsite;
            bool eagleAdvisor = user.EagleAdvisor;
            bool nautilus = user.Nautilus;
            bool registeredRep = user.RegisteredRep;
            bool approvedDBA = user.ApprovedDBA;
            bool longTermCare = user.LongTermCare;
            bool aarp = user.AARP;

            // LAAAAAMMMMEE - Have to kill this from the GraphUserData object or Update will throw exception
            if (user.GraphUserData.Extensions != null)
                user.GraphUserData.Extensions = null;

            // Exception on user does not exist.  We could forward to create but that should be an explicit call, too critical.
            await
            (await GetGraphServiceClientAsync())
            .Users[user.GraphUserData.Id]
            .Request()
            .UpdateAsync(user.GraphUserData);

            await
            UpdateCustomAttributesAsync(
                user.GraphUserData.Id,
                new Dictionary<string, object>
                {
                    { AzureADCustomAttributes.BrandId, user.CustomAttributes?.BrandId ?? brandId },
                    { AzureADCustomAttributes.SSOId, DetermineSSOId(user.CustomAttributes?.SSOId ?? ssoId) },
                    { AzureADCustomAttributes.HasPersonalizedWebsiteAgent, user.CustomAttributes?.HasPersonalizedWebsiteAgent ?? hasPersonalizedWebsiteAgent },
                    { AzureADCustomAttributes.HasPersonalizedWebsiteRecruiter, user.CustomAttributes?.HasPersonalizedWebsiteRecruiter ?? hasPersonalizedWebsiteRecruiter },
                    { AzureADCustomAttributes.EligibleForPersonalizedWebsite, user.CustomAttributes?.EligibleForPersonalizedWebsite ?? eligibleForPersonalizedWebsite },
                    { AzureADCustomAttributes.EagleAdvisor, user.CustomAttributes?.EagleAdvisor ?? eagleAdvisor },
                    { AzureADCustomAttributes.Nautilus, user.CustomAttributes?.Nautilus ?? nautilus },
                    { AzureADCustomAttributes.RegisteredRep, user.CustomAttributes?.RegisteredRep ?? registeredRep },
                    { AzureADCustomAttributes.ApprovedDBA, user.CustomAttributes?.ApprovedDBA ?? approvedDBA },
                    { AzureADCustomAttributes.LongTermCare, user.CustomAttributes?.LongTermCare ?? longTermCare },
                    { AzureADCustomAttributes.AARP, user.CustomAttributes?.AARP ?? aarp },
                });

            // Add groups
            await
            AddUserToADGroup(user.GraphUserData, user.ADGroupIds);
        }

        public string UpdateUserPassword(ADUser user)
        {
            return "This code is work in progress, doesn't work";
            ///
            //var pword =
            //    _passwordService.GeneratePassword(user.GraphUserData.Mail);

            //user.GraphUserData.PasswordProfile =
            //    new PasswordProfile
            //    {
            //        ForceChangePasswordNextSignIn = false,
            //        // Password should be generated unless a specific need arises
            //        Password = pword
            //    };

            //await
            //UpdateADUserAsync(user);
        }

        private async Task AddCustomAttributesAsync(
           string graphUserId,
           IDictionary<string, object> additionalData)
        {
            await
            (await GetGraphServiceClientAsync())
            .Users[graphUserId]
            .Extensions
            .Request()
            .AddAsync(
                new OpenTypeExtension
                {
                    ExtensionName = graphUserId,
                    AdditionalData = additionalData
                });
        }

        private async Task UpdateCustomAttributesAsync(
            string graphUserId,
            IDictionary<string, object> additionalData)
        {
            try
            {
                await
                (await GetGraphServiceClientAsync())
                .Users[graphUserId]
                .Extensions[graphUserId]
                .Request()
                .UpdateAsync(
                    new OpenTypeExtension
                    {
                        ExtensionName = graphUserId,
                        AdditionalData = additionalData
                    });
            }
            catch (Exception)
            {
                // If extensions are missing we must add, not update.  This is a sanity check, should not occur
                await
                AddCustomAttributesAsync(graphUserId, additionalData);
            }
            
        }

        public async Task SetUserAccountStatusAsync(
            string userEmail,
            bool active = true)
        {
            var user =
                await
                GetADUserByFilterAsync(
                    $"{AzureADFields.Mail} eq '{userEmail}'");

            await 
            (await GetGraphServiceClientAsync())
            .Users[user.GraphUserData.Id]
            .Request()
            .UpdateAsync(
                new Microsoft.Graph.User
                {
                    AccountEnabled = active
                });
        }

        //public async Task SetUserPropertyAsync<T, U>(
        //    string userEmail,
        //    U propertyName, //From AzureADFields.cs
        //    T propertyValue)
        //{
        //    // TODO: if I can flesh this out with odata it will elimintate the need for prop specific functions like SetUserAccountStatusAsync()
        //    var graphClient =
        //        await GetGraphServiceCLientAsync(azureTenantId, azureClientId, azureClientSecret);

        //    var user =
        //        await GetADUserAsync(
        //            userEmail,
        //            graphClient);

        //    user[propertyName] = propertyValue;

        //    // 
        //    await graphClient.Users[user.GraphUserData.Id].Request().UpdateAsync(
        //        new Microsoft.Graph.User
        //        {
        //            propertyName = propertyValue
        //        });
        //}
    }
}