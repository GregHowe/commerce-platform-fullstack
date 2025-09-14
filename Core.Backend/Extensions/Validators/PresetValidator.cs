
using Core.Backend.Models.Web.Preset;

namespace Core.Backend.Extensions.Validators
{
    public static class PresetValidator
    {
        public static string Validate<T>(this T preset) =>
            Validate(preset, null, null);
        
        public static string Validate<T>(this T preset, IFormFile file) =>
            Validate(preset, file, null);
        
        public static string Validate<T>(this T preset, int siteId) =>
            Validate(preset, null!, siteId);
    
        public static string Validate<T>(this T preset, IFormFile file, int siteId) =>
            Validate(preset, file, siteId);  

        private static string Validate<T>(this T preset, IFormFile? file = null, int? siteId = null)
        {
            if (preset == null)
                return "Unable to validate null preset";

            // There is some code reduction available here, waiting until done to assess
            if (typeof(T) == typeof(WebCreatePresetDTO))
            {
                if (file != null && siteId != null)
                    return ValidateCreatePreset(preset as WebCreatePresetDTO ?? throw new Exception("Unable to convert object to WebCreatePresetDTO"), file, siteId.Value);

                else if (file == null && siteId != null)
                    return ValidateCreatePreset(preset as WebCreatePresetDTO ?? throw new Exception("Unable to convert object to WebCreatePresetDTO"), siteId.Value);

                else if (file != null && siteId == null)
                    return ValidateCreatePreset(preset as WebCreatePresetDTO ?? throw new Exception("Unable to convert object to WebCreatePresetDTO"), file);

                else
                    return ValidateCreatePreset(preset as WebCreatePresetDTO ?? throw new Exception("Unable to convert object to WebCreatePresetDTO"));
            }
            else
                return string.Empty;
        }

        private static string ValidateCreatePreset(this WebCreatePresetDTO preset)
        {
            if (preset == null)
                return "Unable to validate null preset";

            if (preset.Id != 0)
                return $"Unable to create preset. Preset with id {preset.Id} already exists.";

            //if (preset.ParentPresetId.HasValue)
            //    return "Parent preset can't be assigned to general Content Library asset";

            if (string.IsNullOrEmpty(preset.Type))
                return $"Invalid media type: {preset.Type ?? string.Empty}";

            return string.Empty;
        }

        private static string ValidateCreatePreset(this WebCreatePresetDTO preset, int siteId)
        {
            if (preset == null)
                return "Unable to validate null preset";

            if (siteId == 0)
                return "Invalid site Id";

            //if (preset.ParentPresetId.HasValue)
            //    return "Parent preset can't be assigned to general Content Library asset";

            if (string.IsNullOrEmpty(preset.Type))
                return $"Invalid media type: {preset.Type ?? string.Empty}";

            return string.Empty;
        }

        private static string ValidateCreatePreset(this WebCreatePresetDTO preset, IFormFile file)
        {
            if (file == null)
                return "No asset uploaded";

            if (preset == null)
                return "Unable to validate null preset";

            if (preset.Id != 0)
                return $"Unable to create preset. Preset with id {preset.Id} already exists.";

            if (string.IsNullOrEmpty(preset.Type))
                return $"Invalid media type: {preset.Type ?? string.Empty}";

            if ((preset.Type.ToLower() == "image" ||
                preset.Type.ToLower() == "video" ||
                preset.Type.ToLower() == "pdf") &&
                file == null)
                return $"No file uploaded for medaia type";

            return string.Empty;
        }

        private static string ValidateCreatePreset(this WebCreatePresetDTO preset, IFormFile file, int siteId)
        {
            if (file == null)
                return "No asset uploaded";

            if (preset == null)
                return "Unable to validate null preset";

            if (preset.Id != 0)
                return $"Unable to create preset. Preset with id {preset.Id} already exists.";
            
            if (siteId == 0)
                return "Invalid site Id";

            if (string.IsNullOrEmpty(preset.Type))
                return $"Invalid media type: {preset.Type ?? string.Empty}";

            if ((preset.Type.ToLower() == "image" ||
                preset.Type.ToLower() == "video" ||
                preset.Type.ToLower() == "pdf") &&
                file == null)
                return $"No file uploaded for medaia type";

            return string.Empty;
        }
    }
}