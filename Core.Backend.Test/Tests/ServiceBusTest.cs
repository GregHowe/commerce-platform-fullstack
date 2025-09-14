using Core.CoreLib.Models.Azure.ActiveDirectory;
using Core.CoreLib.Models.Azure.ServiceBus;
using Core.CoreLib.Services.Azure.ActiveDirectory;
using Core.CoreLib.Services.Azure.ServiceBus;
using Core.CoreLib.Services.User;
using Core.CoreLib.Services.Security;
using Microsoft.Extensions.Configuration;
using Core.CoreLib.Models.Exceptions;

namespace Core.Backend.Test.Tests
{
    public class ServiceBusTest : TestBase
    {
        [Fact]
        public async Task SendMessageNewUserTest()
        {
            // Scaffold
            const string UserEmail = "sb.addtest@fusion92.com";

            var config =
                CreateConfig();

            var adService =
                new ActiveDirectoryService(new PasswordService(), config);

            var userService =
                new UserService(adService, config);

            // Send the message to add user
            await
            new MessageSender()
            .SendMessageAsync(
                new UserIngestMessage()
                {
                    ADUsers =
                        new List<ADUser>()
                        {
                            ScaffoldUser("sb", "addtest", UserEmail, "248-555-1212")
                        }
                },
                config.GetConnectionString("SBUserIngest") ?? string.Empty,
                "useringest");

            // Pause for the message to be received and processed, 5 seconds is arbitrary, may need to be longer if
            // false negatives persist
            Thread.Sleep(5000);

            // Verify
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
        public async Task SendMessageUpdateUserTest()
        {
            // Scaffold
            const string FirstName = "sb";
            const string LastName = "updatetest";
            const string UserEmail = "sb.updatetest@fusion92.com";
            const string UpdatedPhone = "248-555-5555";

            var config =
                CreateConfig();

            var adService =
                new ActiveDirectoryService(new PasswordService(), config);

            var userService =
                new UserService(adService, config);

            // Send the message to add user
            await
            new MessageSender()
            .SendMessageAsync(
                new UserIngestMessage()
                {
                    ADUsers =
                        new List<ADUser>()
                        {
                            ScaffoldUser(FirstName, LastName, UserEmail, "248-555-1212")
                        }
                },
                config.GetConnectionString("SBUserIngest") ?? string.Empty,
                "useringest");

            // Pause for the message to be received and processed, 5 seconds is arbitrary, may need to be longer if
            // false negatives persist
            Thread.Sleep(5000);

            await
            new MessageSender()
            .SendMessageAsync(
                new UserIngestMessage()
                {
                    ADUsers =
                        new List<ADUser>()
                        {
                            ScaffoldUser(FirstName, LastName, UserEmail, UpdatedPhone)
                        }
                },
                config.GetConnectionString("SBUserIngest") ?? string.Empty,
                "useringest");

            Thread.Sleep(5000);

            // Verify
            var user =
                await
                userService
                .GetADUserAsync(UserEmail);

            // Assertions
            Assert.NotNull(user);
            Assert.NotNull(user.GraphUserData);
            Assert.NotNull(user.GraphUserData.Id);
            Assert.True(user.GraphUserData.Mail == UserEmail);
            Assert.True(user.GraphUserData.MobilePhone == UpdatedPhone);

            // Cleanup and verify
            await userService.DeleteUserAsync(UserEmail);

            var ex =
                await Assert.ThrowsAsync<DataNotFoundException>(async () => await userService.GetADUserAsync(UserEmail));

            Assert.True(ex.Message == "User not found");
        }
    }
}