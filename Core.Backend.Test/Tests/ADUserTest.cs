using Core.CoreLib.Models.Azure.ActiveDirectory;
using Core.CoreLib.Services.Azure.ActiveDirectory;
using Core.CoreLib.Services.User;
using Core.CoreLib.Services.Security;
using Core.CoreLib.Services.Client.NYL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.CoreLib.Services.Database;
using static Core.CoreLib.Models.Constants.Authorization;
using Core.CoreLib.Models.Exceptions;

namespace Core.Backend.Test.Tests
{
    public class ADUserTest : TestBase
    {
        [Fact]
        public async Task AddUserTest()
        {
            // Required fields that must be unique
            const string UserEmail = "add.test@fusion92core.com";
            const string FirstName = "Add";
            const string LastName = "Test";

            // Scaffold
            var config =
                CreateConfig();

            var userService =
                new UserService(
                    new ActiveDirectoryService(new PasswordService(), config), 
                    config);

            // Add user
            await
            userService
            .AddADUserAsync(
                ScaffoldUser(FirstName, LastName, UserEmail));

            var user =
                await
                userService
                .GetADUserAsync(UserEmail);

            // Assertions
            Assert.NotNull(user);
            Assert.NotNull(user.GraphUserData);
            Assert.NotNull(user.GraphUserData.Id);
            Assert.True(user.GraphUserData.Mail == UserEmail);

            // Cleanup and verify
            await userService.DeleteUserAsync(UserEmail);

            var ex =
                await Assert.ThrowsAsync<DataNotFoundException>(async () => await userService.GetADUserAsync(UserEmail));

            Assert.True(ex.Message == "User not found");
        }

        [Fact]
        public async Task UpdateUserTest()
        {
            // Required fields that must be unique
            const string UserEmail = "update.test@fusion92core.com";
            const string FirstName = "Update";
            const string LastName = "Test";
            const string UpdatedPhone = "555-555-5555";

            // Scaffold
            var config =
                CreateConfig();

            var userService =
                new UserService(
                    new ActiveDirectoryService(new PasswordService(), config), 
                    config);

            // Add user
            await
            userService
            .AddADUserAsync(
                ScaffoldUser(FirstName, LastName, UserEmail));

            var user =
                await
                userService
                .GetADUserAsync(UserEmail);

            // Update user
            user.CustomAttributes =
                new CustomAttribute()
                {
                    BrandId = user.BrandId,
                    SSOId = user.SSOId,
                    HasPersonalizedWebsiteAgent = user.HasPersonalizedWebsiteAgent,
                    HasPersonalizedWebsiteRecruiter = user.HasPersonalizedWebsiteRecruiter,
                    EligibleForPersonalizedWebsite = user.EligibleForPersonalizedWebsite,
                    EagleAdvisor = user.EagleAdvisor,
                    Nautilus = user.Nautilus,
                    ApprovedDBA = user.ApprovedDBA,
                    RegisteredRep = user.RegisteredRep,
                    LongTermCare = user.LongTermCare,
                    AARP = user.AARP,
                };

            user.GraphUserData.MobilePhone = UpdatedPhone;

            await
            userService
            .UpdateADUserAsync(user);

            // Pull user from AD, run assertions
            var updatedUser =
                await
                userService
                .GetADUserAsync(UserEmail);

            Assert.NotNull(updatedUser);
            Assert.NotNull(updatedUser.GraphUserData);
            Assert.NotNull(updatedUser.GraphUserData.Id);
            Assert.True(user.GraphUserData.MobilePhone == UpdatedPhone);

            // Cleanup and verify
            await userService.DeleteUserAsync(UserEmail);

            var ex =
                await Assert.ThrowsAsync<DataNotFoundException>(async () => await userService.GetADUserAsync(UserEmail));

            Assert.True(ex.Message == "User not found");
        }

        [Fact]
        public async Task DeleteUserTest()
        {
            // Required fields that must be unique
            const string UserEmail = "delete.test@fusion92core.com";
            const string FirstName = "Delete";
            const string LastName = "Test";

            // Scaffold
            var config =
                CreateConfig();

            var userService =
                new UserService(
                    new ActiveDirectoryService(new PasswordService(), config), 
                    config);

            // Add user
            await
            userService
            .AddADUserAsync(
                ScaffoldUser(FirstName, LastName, UserEmail));

            // Delete and verify
            await userService.DeleteUserAsync(UserEmail);

            var ex =
                await Assert.ThrowsAsync<DataNotFoundException>(async () => await userService.GetADUserAsync(UserEmail));

            Assert.True(ex.Message == "User not found");
        }

        [Fact]
        public async Task GetSSOUserTest()
        {
            // This user could potentially drop out, just grab another and replace.
            // Don't want to spoof SSO user as they are not us.
            const string ssoId = "ap00p040kv94w6sa";

            // Scaffold
            var config =
                CreateConfig();

            // Get the SSO user
            var user =
                await
                new UserService(
                    new ActiveDirectoryService(new PasswordService(), config),
                    config)
                .GetSSOADUserAsync(ssoId);

            // Assertions
            Assert.NotNull(user);
            Assert.NotNull(user.GraphUserData);
            Assert.NotNull(user.GraphUserData.Id);

            // SSO Id is stored on EmployeeId prop in AD
            Assert.NotNull(user.GraphUserData.EmployeeId);
        }

        [Fact]
        public async Task SetUserAccountEnabaledStatusTest()
        {
            // Required fields that must be unique
            const string UserEmail = "enable.test@fusion92core.com";
            const string FirstName = "Enable";
            const string LastName = "Test";

            // Scaffold
            var config =
                CreateConfig();

            var userService =
                new UserService(
                    new ActiveDirectoryService(new PasswordService(), config), 
                    config);

            // Add user, disabled by default
            await
             userService
            .AddADUserAsync(
                 ScaffoldUser(FirstName, LastName, UserEmail));

            var user =
                await
                userService
                .GetADUserAsync(UserEmail);

            // Assertions
            Assert.NotNull(user);
            Assert.NotNull(user.GraphUserData);
            Assert.NotNull(user.GraphUserData.Id);
            Assert.True(user.GraphUserData.Mail == UserEmail);

            // Set as enabled
            await
            userService
            .SetUserActiveStatusAsync(UserEmail);

            // Re-pull and verfiy enabled
            var enabeldUser =
               await
               userService
               .GetADUserAsync(UserEmail);

            Assert.NotNull(enabeldUser);
            Assert.NotNull(enabeldUser.GraphUserData);
            Assert.NotNull(enabeldUser.GraphUserData.Id);
            Assert.True(enabeldUser.GraphUserData.Mail == UserEmail);
            Assert.True(enabeldUser.GraphUserData.AccountEnabled ?? false);

            // Delete and verify
            await 
                userService
                .DeleteUserAsync(UserEmail);

            var ex =
                await Assert.ThrowsAsync<DataNotFoundException>(async () => await userService.GetADUserAsync(UserEmail));

            Assert.True(ex.Message == "User not found");
        }

        [Fact]
        public async Task ADUserMatchesDBRecordTest()
        {
            // Scaffold
            var config =
                CreateConfig();

            // Get DB user
            var dbUser =
                await 
                new CoreLib.Services.Database.User.UserService(new DapperContext(config))
                .GetRandomUser(ProdTest(config));

            Assert.NotNull(dbUser);
            Assert.NotEmpty(dbUser.NYLId);
            Assert.NotNull(dbUser.NYLId);

            // Get AD user
            var user =
                await
                new UserService(
                    new ActiveDirectoryService(new PasswordService(), config),
                    config)
                .GetSSOADUserAsync(dbUser.NYLId);

            // Assertions
            Assert.NotNull(user);
            Assert.NotNull(user.GraphUserData);
            Assert.NotNull(user.GraphUserData.Id);
            Assert.NotNull(user.GraphUserData.EmployeeId);
            Assert.NotNull(user.GraphUserData.EmployeeType);

            if (dbUser.IsGO.HasValue && dbUser.IsGO.Value)
                Assert.True(user.GraphUserData.EmployeeType.ToLower() == EmployeeType.GeneralOffice);

            if (dbUser.IsHO.HasValue && dbUser.IsHO.Value)
                Assert.True(user.GraphUserData.EmployeeType.ToLower() == EmployeeType.HomeOffice);

            if (dbUser.IsOBO.HasValue && dbUser.IsOBO.Value)
                Assert.True(user.GraphUserData.EmployeeType.ToLower() == EmployeeType.OnBehalfOf);

            if (dbUser.IsAgent.HasValue && dbUser.IsAgent.Value)
                Assert.True(user.GraphUserData.EmployeeType.ToLower() == EmployeeType.Agent);

            Assert.True((dbUser.HasPersonalizedWebsiteAgent ?? false) == user.HasPersonalizedWebsiteAgent);
            Assert.True((dbUser.HasPersonalizedWebsiteRecruiter ?? false) == user.HasPersonalizedWebsiteRecruiter);
            Assert.True((dbUser.EligibleForPersonalizedWebsite ?? false) == user.EligibleForPersonalizedWebsite);
            Assert.True((dbUser.EagleInd ?? false) == user.EagleAdvisor);
            Assert.True((dbUser.NautilusInd ?? false) == user.Nautilus);
            Assert.True((dbUser.RegisteredRep ?? false) == user.RegisteredRep);
            Assert.True((dbUser.DBAInd ?? false) == user.ApprovedDBA);
            Assert.True((dbUser.LTCInd ?? false) == user.LongTermCare);
            Assert.True((dbUser.AARPInd ?? false) == user.AARP);
        }

        [Fact]
        public async Task UserExistsTest()
        {
            // Scaffold
            var config =
                CreateConfig();

            var userService =
                new UserService(
                    new ActiveDirectoryService(new PasswordService(), config),
                    config);

            // Get DB user
            var dbUser =
                await
                new CoreLib.Services.Database.User.UserService(new DapperContext(config))
                .GetRandomUser(ProdTest(config));

            Assert.NotNull(dbUser);
            Assert.NotEmpty(dbUser.NYLId);
            Assert.NotNull(dbUser.NYLId);

            // Get AD user
            var user =
                await
                userService
                .GetSSOADUserAsync(dbUser.NYLId);

            Assert.NotNull(user);

            var result =
                await
                userService
                .UserExists(user);

            Assert.True(result);

            var ex =
                await Assert.ThrowsAsync<DataNotFoundException>(async () => await userService.GetADUserAsync("doesnotexist@fusion92.com"));

            Assert.True(ex.Message == "User not found");
        }
    }
}