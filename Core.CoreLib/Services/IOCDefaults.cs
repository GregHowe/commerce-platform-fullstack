using Core.CoreLib.Services.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Core.CoreLib.Services.Database.Brand;
using Core.CoreLib.Services.Database.Authentication;
using Core.CoreLib.Services.User;
using Core.CoreLib.Services.Database.Library;
using Core.CoreLib.Services.Database.Site;
using Core.CoreLib.Services.Azure.Storage;
using Core.CoreLib.Services.Azure.ActiveDirectory;
using Core.CoreLib.Services.Azure.ServiceBus;
using Core.CoreLib.Services.Azure.ServiceBus.Processors;
using Core.CoreLib.Services.Security;
using Core.CoreLib.Services.Client.NYL;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using System.Security.Cryptography.X509Certificates;
using Azure.Identity;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Services.Form;
using Core.CoreLib.Services.Http;
using Core.CoreLib.Services.Database.Profile;
using Core.CoreLib.Services.Tooling;

namespace Core.CoreLib.Services
{
    public static class IOCDefaults
    {
        public static void RegisterDefaults(
            IServiceCollection services,
            IConfiguration configuration)
        {
            // Default client, general use
            services.AddHttpClient();

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
                       configuration["KeyVault:AzureKeyVaultEndpoint"],
                       configuration["KeyVault:AZURE_CLIENT_ID"],
                       configuration["KeyVault:AZURE_TENANT_ID"],
                       configuration["KeyVault:AZURE_CLIENT_SECRET"],
                       configuration["NYL:CertificateName"]);

                leadFormHandler.ClientCertificates.Add(cert);
                
                return leadFormHandler;
            });

            // Services
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenExploder, NYLSSOTokenExploderService>();

            services.AddSingleton<IServiceBusTopicSubscription, ServiceBusTopicSubscription>();
            services.AddScoped<IServiceBusMessageProcessor, UserIngestMessageProcessor>();
            services.AddScoped<IMessageSender, MessageSender>();

            services.AddHostedService<ServiceBusWorker>();

            services.AddScoped<DapperContext>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ILibraryService, LibraryService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<Database.User.IUserService, Database.User.UserService>();
            services.AddScoped<ILeadFormService, LeadFormService>();
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDbaService, DbaService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IGRecaptchaService, GRecaptchaService>();

            services.AddDbContext<CoreDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Azure")));
        }

        private static X509Certificate2 GetCertificateFromKeyValut(
            string vaultUrl,
            string clientId,
            string tenantId,
            string clientIdSecret,
            string certificateName)
        {
            // If missing these will fail to retrieve a cert, avoid startup error
            if (string.IsNullOrWhiteSpace(vaultUrl) ||
                string.IsNullOrWhiteSpace(clientId) ||
                string.IsNullOrWhiteSpace(tenantId) ||
                string.IsNullOrWhiteSpace(clientIdSecret) ||
                string.IsNullOrWhiteSpace(certificateName))
                return null;

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

                var cert =
                    new X509Certificate2(pfx);

                return cert;
            }
            else
                return new X509Certificate2(certificate.Cer);
        }
    }
}