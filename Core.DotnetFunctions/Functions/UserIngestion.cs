using System;
using System.Threading.Tasks;
using Core.DotnetFunctions.AppSettings;
using Core.DotnetFunctions.Services.UserIngest;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.DotnetFunctions.Functions
{
    public class UserIngestion
    {
        [FunctionName("UserIngestion")]
        public void Run([TimerTrigger("0 0 */12 * * *", RunOnStartup = true)] TimerInfo myTimer, ExecutionContext context, ILogger logger)
        {
            logger.LogInformation($"UserIngestion Timer function Invoked at: {DateTime.Now}");

            ServicebusSettings servicebusSettings = new();
            DbSettings dbSettings = new();
            TenantSettings tenantsSettings = new();

            //binding the settings
            var config = 
                new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            config.Bind("ServicebusSettings", servicebusSettings);
            config.Bind("DbSettings", dbSettings);
            config.Bind("TenantSettings", tenantsSettings);

            logger.LogInformation($"UserIngestion - User ingestion process started at: {DateTime.Now}");

            //process user ingestion
            Task.Run(
                async () =>
                await
                new UserIngestService(
                    logger,
                    servicebusSettings,
                    dbSettings,
                    tenantsSettings,
                    Environment.GetEnvironmentVariable("API_ENVIRONMENT"))
                .ProcessUserIngestion());
            
            logger.LogInformation($"UserIngestion - User ingestion process completed at: {DateTime.Now}");
        }
    }
}
