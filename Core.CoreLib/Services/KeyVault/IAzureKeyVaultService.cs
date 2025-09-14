namespace Core.Backend.Services.KeyVault
{
    public interface IAzureKeyVaultService
    {
        /// <summary>
        /// Gets the secret name
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        Task<string> GetSecret(string secretName);
    }
}
