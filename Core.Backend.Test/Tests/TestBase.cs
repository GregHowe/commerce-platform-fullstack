using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Models.Azure.ActiveDirectory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.ApplicationInsights;

namespace Core.Backend.Test.Tests
{
    public class TestBase
    {
        public IConfigurationRoot CreateConfig()
        {
            // Not sure if this is correct way to do app settings in tests, works but interested in feedback
            return
                new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public bool ProdTest(IConfigurationRoot config) => config["ASPNETCORE_ENVIRONMENT"] == "true";

        public ServiceProvider CreateServices(
            IConfigurationRoot config)
        {
            var services = new ServiceCollection();

            // Disabled telemetry client, will instatiate, no reporting
            var telemetryClient = new TelemetryClient(new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration() { ConnectionString = config["APPLICATIONINSIGHTS_CONNECTION_STRING"], DisableTelemetry = true });
            services.AddSingleton(telemetryClient);

            // Client handlers, http factory support
            CreateNamedClients(services, config);

            return 
                services.BuildServiceProvider();
        }

        private static void CreateNamedClients(
            ServiceCollection services,
            IConfigurationRoot config)
        {
            // Named clients, specific implementations

            services.AddHttpClient(HttpNamedClients.NYLCLTLeadSubmissionClient, c =>
            {
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var leadFormHandler =
                    new HttpClientHandler()
                    {
                        SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                        ClientCertificateOptions = ClientCertificateOption.Manual
                    };

                var cert =
                   GetCertificateFromKeyValut(
                       config["KeyVault:AzureKeyVaultEndpoint"],
                       config["KeyVault:AZURE_CLIENT_ID"],
                       config["KeyVault:AZURE_TENANT_ID"],
                       config["KeyVault:AZURE_CLIENT_SECRET"],
                       config["NYL:CertificateName"]);

                leadFormHandler.ClientCertificates.Add(cert);

                return leadFormHandler;
            });
        }

        private static X509Certificate2 GetCertificateFromKeyValut(
            string vaultUrl,
            string clientId,
            string tenantId,
            string clientIdSecret,
            string certificateName)
        {
            if (string.IsNullOrWhiteSpace(vaultUrl))
                throw new InvalidOperationException($"MIssing Key Vault url.");

            var credentials = new ClientSecretCredential(tenantId, clientId, clientIdSecret);
            var certificateClient = new CertificateClient(new Uri(vaultUrl), credentials);
            var secretClient = new SecretClient(new Uri(vaultUrl), credentials);

            var certificate =
                certificateClient.GetCertificate(certificateName).Value;

            // Return a certificate with only the public key if the private key is not exportable.
            if (!certificate.Policy?.Exportable ?? false)
                return new X509Certificate2(certificate.Cer);

            // Parse the secret ID and version to retrieve the private key.
            var segments = certificate.SecretId.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length != 3)
                return new X509Certificate2(certificate.Cer);

            var secretName = segments[1];
            var secretVersion = segments[2];

            var secret =
                secretClient.GetSecret(secretName, secretVersion).Value;

            // For PEM, you'll need to extract the base64-encoded message body.
            if ("application/x-pkcs12".Equals(secret.Properties.ContentType, StringComparison.InvariantCultureIgnoreCase))
            {
                var pfx = Convert.FromBase64String(secret.Value);

                return 
                    new X509Certificate2(pfx);
            }
            else
                throw new NotSupportedException($"Only PKCS#12 is supported. Found Content-Type: {secret.Properties.ContentType}");
        }

        public ADUser ScaffoldUser(
            string firstName,
            string lastName,
            string email,
            string mobilePhone = "888.550.4864")
        {
            return
                new ADUser()
                {
                    GraphUserData =
                        new Microsoft.Graph.User()
                        {
                            DisplayName = $"{firstName} {lastName}",
                            GivenName = firstName,
                            Surname = lastName,
                            StreetAddress = "444 Ontario St.",
                            City = "Chicago",
                            State = "IL",
                            PostalCode = "60654",
                            Country = "USA",
                            Mail = email,
                            MobilePhone = mobilePhone,
                            EmployeeType = "Admin",
                            CreatedDateTime = DateTime.Now,
                            AccountEnabled = true
                        },
                    CustomAttributes =
                        new CustomAttribute()
                        {
                            BrandId = "3",
                            SSOId = string.Empty,
                            HasPersonalizedWebsiteAgent = false,
                            HasPersonalizedWebsiteRecruiter = false,
                            EligibleForPersonalizedWebsite = false,
                            EagleAdvisor = false, 
                            Nautilus = false,
                            RegisteredRep = false,
                            ApprovedDBA = false,
                            LongTermCare = false,
                            AARP = false
                        },
                    ADGroupIds = new List<string>()
                };
        }
    }
}
