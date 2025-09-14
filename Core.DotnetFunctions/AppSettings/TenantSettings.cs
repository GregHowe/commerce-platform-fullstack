
namespace Core.DotnetFunctions.AppSettings
{
    public class TenantSettings
    {
        public string Environment { get; set; }

        public bool IsProd =>
            Environment == "Prod";
    }
}
