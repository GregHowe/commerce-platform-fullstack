using Azure.Security.KeyVault.Secrets;
namespace Core.Backend.Services.KeyVault
{
    public class AzureKeyVaultService : IAzureKeyVaultService
    {
        private readonly SecretClient _client;
        public AzureKeyVaultService(SecretClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Retrieve a secret using the secret client.
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        public async Task<string> GetSecret(string secretName)
        {
            var secret = await _client.GetSecretAsync(secretName);

            return secret.Value.Value;
        }
    }
}
