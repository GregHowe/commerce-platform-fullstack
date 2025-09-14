
namespace Core.CoreLib.Models.DTO.Common
{
    public interface ICommonAssetSettings
    {
        public string? Text { get; set; }
        public string? AltText { get; set; }
        public string? Action { get; set; }
        public string? LicenseType { get; set; }
        public string? Source { get; set; }
        public string? SourceUrl { get; set; }
        public string? Dimension { get; set; }
        public string? WebHostedAssetUrl { get; set; }
        public bool? HasAgreedToLicensing { get; set; }
    }
}