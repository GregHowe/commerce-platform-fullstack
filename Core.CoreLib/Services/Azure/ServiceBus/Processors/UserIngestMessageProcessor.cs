using Azure.Messaging.ServiceBus;
using Core.CoreLib.Models.Azure.ServiceBus;
using Core.CoreLib.Services.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.CoreLib.Services.Azure.ServiceBus.Processors
{
    public class UserIngestMessageProcessor : IServiceBusMessageProcessor
    {
        protected IHost _host;
        protected IConfiguration _configuration;

        public UserIngestMessageProcessor(
            IConfiguration configuration,
            IHost host)
        {
            _host = host;
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetConnectionString("SBUserIngest") ?? string.Empty;
        public string Topic => "useringest";
        public string Subscription => "CoreUserMgmt";
        public string Subject => Topic;

        public async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            if (args == null)
                throw new Exception($"Invalid service bus args received");
            
            if (args.Message == null)
                throw new Exception($"Invalid service bus message received");
            
            if (string.IsNullOrWhiteSpace(args.Message.Subject))
                throw new Exception($"Invalid service bus subject received");
            
            var scope = _host.Services.CreateScope();

            var userService =
                scope.ServiceProvider.GetService<IUserService>();

            if (userService == null)
                throw new Exception($"Unable to invoke User Service");

            try
            {
                var messageBody =
                    args.Message.Body.ToObjectFromJson<UserIngestMessage>();

                // TODO: Might want to change this to not deadletter is one user in the batch has bad data.  Currently, 1 bad user in batch ruins batch (25 per batch)
                // TODO: Integrate telemetryClient!!!!
                foreach (var user in messageBody.ADUsers)
                {
                    if (user != null)
                    {
                        if (await userService.UserExists(user))
                        {
                            var existing =
                                await
                                userService
                                .GetADUserAsync(
                                    user.GraphUserData.Mail);

                            existing.GraphUserData = 
                                CopyUser(existing.GraphUserData, user.GraphUserData);

                            existing.ADGroupIds = user.ADGroupIds;
                            existing.CustomAttributes = user.CustomAttributes;

                            await userService.UpdateADUserAsync(existing);
                        }
                        else
                            await userService.AddADUserAsync(user);
                    }
                }
            }
            catch (Exception ex)
            {   
                await args.DeadLetterMessageAsync(args.Message, ex.Message).ConfigureAwait(false);
               // _logger.LogError(ex, "UserIngestMessageProcessor", args);
            }
        }

        private Microsoft.Graph.User CopyUser(
            Microsoft.Graph.User existing, 
            Microsoft.Graph.User newUser)
        {
            // Don't update mail, this is the key id in our AD, new mail is a new user
            //existing.Mail = newUser.Mail;
            existing.DisplayName = newUser.DisplayName;
            existing.GivenName = newUser.GivenName;
            existing.Surname = newUser.Surname;
            existing.StreetAddress = newUser.StreetAddress;
            existing.City = string.IsNullOrWhiteSpace(newUser.City) ? "None" : newUser.City;
            existing.State = newUser.State;
            existing.PostalCode = newUser.PostalCode;
            existing.Country = newUser.Country;
            existing.AccountEnabled = newUser.AccountEnabled;
            existing.MobilePhone = string.IsNullOrWhiteSpace(newUser.MobilePhone) ? "555" : newUser.MobilePhone;
            existing.CompanyName = newUser.CompanyName;
            existing.MailNickname = newUser.MailNickname;
            existing.EmployeeType = newUser.EmployeeType;
            existing.EmployeeId = newUser.EmployeeId;

            return existing;
        }
    }
}
