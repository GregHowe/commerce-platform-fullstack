namespace Core.CoreLib.Models.Settings
{
    public class AzureAd
    {
        public string Domain { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Instance { get; set; }
        public string CallbackPath { get; set; }
    }
}
