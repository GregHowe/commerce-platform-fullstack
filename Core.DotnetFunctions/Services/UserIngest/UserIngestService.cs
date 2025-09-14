using Core.CoreLib.Models.Azure.ActiveDirectory;
using Core.CoreLib.Models.Azure.ServiceBus;
using Core.CoreLib.Services.Azure.ServiceBus;
using Core.DotnetFunctions.AppSettings;
using Core.DotnetFunctions.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Core.DotnetFunctions.Services.UserIngest
{
    public class UserIngestService : IUserIngestService
    {
        private readonly ILogger _logger;
        private readonly ServicebusSettings _servicebusSettings;
        private readonly DbSettings _dbSettings;
        private readonly TenantSettings _tenantSettings;
        private readonly string _environment;

        public UserIngestService(
            ILogger logger,
            ServicebusSettings servicebusSettings,
            DbSettings dbSettings,
            TenantSettings tenantSettings,
            string environment)
        {
            _logger = logger;
            _servicebusSettings = servicebusSettings;
            _dbSettings = dbSettings;
            _tenantSettings = tenantSettings;
            _environment = environment ?? "Dev";
        }

        public async Task SendMessageAsync<T>(
            T message,
            string serviceBusConnectionString,
            string topic)
        {
            await
            new MessageSender()
            .SendMessageAsync(
               message,
               serviceBusConnectionString,
               topic);
        }

        public async Task ProcessUserIngestion()
        {
            //connect to SQL Server and get the user data from the "NYLUserAccessInfo"
            var users =
                GetUsers();

            // only send user information if the query returns data 
            if (users.Any())
            {
                try
                {
                    //Send one user for testing
                    //var TestOnly =
                    //    new List<ADUser>()
                    //    { users.Where(w => w.GraphUserData.GivenName.Contains("Irene")).FirstOrDefault() };

                    var loopControl = 25;
                    for (int i = 0; i < users.Count(); i = i + loopControl)
                    {
                        var items = users.Skip(i).Take(loopControl).ToList();

                        await
                        SendMessageAsync(
                        new UserIngestMessage()
                        {
                            ADUsers = items
                        },
                        _servicebusSettings.ServicebusCoreEventsConnectionstring,
                        _servicebusSettings.ServicebusCoreEventsUserIngestionTopicname);

                        _logger.LogInformation($"UserIngestion Timer function successfully sent the aduser data at: {DateTime.Now}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"UserIngestion Timer function encountered and error sending the user data at: {DateTime.Now}.  {ex.Message}");
                }
            }
        }

        private List<ADUser> GetUsers()
        {
            var users = new List<ADUser>();
            _logger.LogInformation($"UserIngestion -- getting the users at: {DateTime.Now}");

            try
            {
                using SqlConnection nylConn = new(_dbSettings.NylDbConnectionString);
                var nylReader = ExecuteQuery(Query.GetUsers(_environment), nylConn);

                List<string> groupObjectIds = null;

                while (nylReader.Read())
                {
                    if (groupObjectIds == null)
                        groupObjectIds = GetAzureGroupIds(nylReader["BrandId"].ToString());

                    users
                    .Add(
                        new ADUser()
                        {
                            GraphUserData = 
                                new Microsoft.Graph.User()
                                {
                                    DisplayName = $"{nylReader["PrefFName"]} {nylReader["PrefMName"]} {nylReader["PrefLName"]}",
                                    GivenName = $"{nylReader["PrefFName"]}",
                                    Surname = $"{nylReader["PrefLName"]}",
                                    StreetAddress = $"{nylReader["PrefAddr1"]} {nylReader["PrefAddr2"]} {nylReader["PrefAddr3"]}",
                                    City = !string.IsNullOrEmpty(nylReader["City"]?.ToString() ?? "") ? nylReader["City"]?.ToString() : "NA",
                                    State = !string.IsNullOrEmpty(nylReader["State"]?.ToString() ?? "") ? nylReader["State"]?.ToString() : "NA",
                                    PostalCode = !string.IsNullOrEmpty(nylReader["Zip"]?.ToString() ?? "") ? nylReader["Zip"]?.ToString() : "NA",
                                    Country = !string.IsNullOrEmpty(nylReader["Country"]?.ToString() ?? "") ? nylReader["Country"]?.ToString() : "NA",
                                    Mail = nylReader["Email"].ToString(),
                                    MobilePhone = $"{nylReader["AreaCode"]}-{nylReader["Phone"]}",
                                    EmployeeType =
                                        GetEmployeeType(
                                            bool.Parse(nylReader["IsGo"]?.ToString() ?? string.Empty),
                                            bool.Parse(nylReader["IsHomeOffice"]?.ToString() ?? string.Empty),
                                            bool.Parse(nylReader["IsAgent"]?.ToString() ?? string.Empty),
                                            bool.Parse(nylReader["IsOnBehalf"]?.ToString() ?? string.Empty)),
                                    AccountEnabled = (nylReader?["UserActiveFlag"]?.ToString() ?? string.Empty) == "Y"
                                },
                            CustomAttributes = 
                                new CoreLib.Models.Azure.ActiveDirectory.CustomAttribute()
                                {
                                    BrandId = nylReader["BrandId"]?.ToString() ?? string.Empty,
                                    SSOId = nylReader["NYLID"]?.ToString() ?? string.Empty,
                                    HasPersonalizedWebsiteAgent = bool.Parse(nylReader["HasPersonalizedWebsite_Agent"]?.ToString() ?? string.Empty),
                                    HasPersonalizedWebsiteRecruiter = bool.Parse(nylReader["HasPersonalizedWebsite_Recruiter"]?.ToString() ?? string.Empty),
                                    EligibleForPersonalizedWebsite = bool.Parse(nylReader["EligibleForPersonalizedWebsite"]?.ToString() ?? string.Empty),
                                    EagleAdvisor = bool.Parse(nylReader["EagleInd"]?.ToString() ?? string.Empty),
                                    Nautilus = bool.Parse(nylReader["NautilusInd"]?.ToString() ?? string.Empty),
                                    RegisteredRep = bool.Parse(nylReader["RegRepInd"]?.ToString() ?? string.Empty),
                                    ApprovedDBA = bool.Parse(nylReader["DBAInd"]?.ToString() ?? string.Empty),
                                    LongTermCare = bool.Parse(nylReader["LTCInd"]?.ToString() ?? string.Empty),
                                    AARP = bool.Parse(nylReader["AARPInd"]?.ToString() ?? string.Empty)
                                },
                            ADGroupIds = groupObjectIds ?? new List<string>(),
                        });
                }

                return users;
            }
            catch (SqlException e)
            {
                _logger.LogInformation($"UserIngestion -- The connection to the database failed with the following error '{e.Message}' at: {DateTime.Now}");
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"UserIngestion -- '{ex.Message}' at: {DateTime.Now}");
                return users;
            }
        }

        private string GetEmployeeType(
            bool isGo,
            bool isHomeOffice,
            bool isAgent,
            bool isOnBehalf)
        {
            //NYL-1323, NYL-1316
            if (isGo)
                return "GO"; //General Office

            else if (isHomeOffice)
                return "HO"; //Home Office

            else if (isAgent)
                return "Agent";
            else
                return "OBO"; //On Behalf Of
        }

        private List<string> GetAzureGroupIds(
            string brandId)
        {
            using SqlConnection connection = new(_dbSettings.NylDbConnectionString);
            var coreReader = ExecuteQuery(Query.GetGroupId(brandId, _tenantSettings.IsProd), connection);

            var result = new List<string>();

            while (coreReader.Read())
                result.Add(coreReader["AzureObjectId"].ToString());

            return result;
        }

        private SqlDataReader ExecuteQuery(string sql, SqlConnection connection)
        {
            connection.Open();
            SqlCommand command = new(sql, connection);
            return command.ExecuteReader();
        }
    }
}