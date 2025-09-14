
using Core.Backend.Models.Web.Preset;
using Core.CoreLib.Extensions;
using Core.CoreLib.Models.Azure.ActiveDirectory;
using Core.CoreLib.Models.Azure.Blob;
using Core.CoreLib.Models.DTO.Common;
using Core.CoreLib.Models.DTO.Library;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace Core.Backend.Extensions.DBOs
{
    public static class PresetExtension
    {
        public static PresetDTO ToPresetDBO(this WebCreatePresetDTO createPresetDTO) =>
            ConvertToPresetDBO(createPresetDTO);

        public static PresetDTO ToPresetDBO(this WebCreatePresetDTO createPresetDTO, int siteId) =>
            ConvertToPresetDBO(createPresetDTO, null, siteId);

        public static PresetDTO ToPresetDBO(this WebCreatePresetDTO createPresetDTO, BlobDTO blobAsset) =>
            ConvertToPresetDBO(createPresetDTO, blobAsset);

        public static PresetDTO ToPresetDBO(this WebCreatePresetDTO createPresetDTO, BlobDTO blobAsset, int siteId) =>
            ConvertToPresetDBO(createPresetDTO, blobAsset, siteId);

        public static PresetDTO MergeCreateDTO(this WebCreatePresetDTO createPresetDTO, PresetDTO existingDTO, int siteId) =>
            MergeToPresetDBO(createPresetDTO, existingDTO, siteId);

        public static PresetDTO MergeUpdateDTO(this WebUpdatePresetDTO updatePresetDTO, PresetDTO existingDTO) =>
            MergeToPresetDBO(updatePresetDTO, existingDTO);

        public static PresetDTO MergeUpdateDTO(this WebUpdatePresetDTO updatePresetDTO, PresetDTO existingDTO, int siteId) =>
            MergeToPresetDBO(updatePresetDTO, existingDTO, siteId);

        private static PresetDTO ConvertToPresetDBO(this WebCreatePresetDTO webPresetDTO, BlobDTO? blobAsset = null, int? siteId = null) =>
            new PresetDTO
            {
                AllowAARP = webPresetDTO.AllowAARP,
                AllowAgent = webPresetDTO.AllowAgent,
                AllowBusinessSolutions = webPresetDTO.AllowBusinessSolutions,
                AllowDBA = webPresetDTO.AllowDBA,
                AllowEagle = webPresetDTO.AllowEagle,
                AllowGO = webPresetDTO.AllowGO,
                AllowHO = webPresetDTO.AllowHO,
                AllowLongTermCare = webPresetDTO.AllowLongTermCare,
                AllowNautilus = webPresetDTO.AllowNautilus,
                AllowRegisteredRep = webPresetDTO.AllowRegisteredRep,
                CategoryIds = webPresetDTO.CategoryIds,
                ClientCode = webPresetDTO.ClientCode,
                Description = webPresetDTO.Description,
                ExpirationDt = webPresetDTO.ExpirationDt,
                Id = webPresetDTO.Id,
                IsDeleted = false,
                IsPrivate =
                    // If uploading media, can't have a parent.
                    // Set to true when preset is being created to site and not inheritted from CL (by agent for agent)
                    siteId.HasValue && siteId != 0 &&
                    !webPresetDTO.ParentPresetId.HasValue,
                ParentPresetId =
                    // Set to value when a Preset is inheritted/leveraged from CL (not created by agent). existingDTO.Id should match webPresetDTO.ParentPresetId
                    blobAsset == null &&
                    siteId.HasValue && siteId != 0 &&
                    webPresetDTO.ParentPresetId.HasValue && webPresetDTO.ParentPresetId != 0 ?
                        // ParentPresetIds match, this is an update of existing site level Preset, else existing is parent, this is a copy (update) of a global Preset, set ParentId to existing
                        webPresetDTO.ParentPresetId.HasValue &&
                        webPresetDTO.ParentPresetId.HasValue &&
                        webPresetDTO.ParentPresetId == webPresetDTO.ParentPresetId ?
                            webPresetDTO.ParentPresetId.Value :
                            webPresetDTO.Id :
                        null,
                PresetSettings =
                    new CommonAndHostedAssetSettings()
                    {
                        // immutable
                        FileName = blobAsset != null ? blobAsset.FileName : null,
                        FileType = blobAsset != null ? (blobAsset.FileName ?? string.Empty).FindBetweenTwoStrings(".", string.Empty) : null,
                        Url = blobAsset != null ? blobAsset.Uri : null,

                        // mutable
                        Action = webPresetDTO.PresetSettings.Action,
                        AltText = webPresetDTO.PresetSettings.AltText,
                        Dimension = webPresetDTO.PresetSettings.Dimension,
                        LicenseType = webPresetDTO.PresetSettings.LicenseType,
                        Source = webPresetDTO.PresetSettings.Source,
                        SourceUrl = webPresetDTO.PresetSettings.SourceUrl,
                        Text = webPresetDTO.PresetSettings.Text,
                        WebHostedAssetUrl = webPresetDTO.PresetSettings.WebHostedAssetUrl,
                        HasAgreedToLicensing = webPresetDTO.PresetSettings.HasAgreedToLicensing
                    },
                // SiteId can only be set on create
                SiteId = siteId.HasValue && siteId != 0 ? siteId.Value : null,
                SMRUCode = webPresetDTO.SMRUCode,
                Title = webPresetDTO.Title,
                Type = webPresetDTO.Type
            };

        private static PresetDTO MergeToPresetDBO(this WebUpdatePresetDTO webPresetDTO, PresetDTO existingDTO, int? siteId = null) =>
            // This can be called when updating and existing preset OR when creating site preset from a parent (CL/global preset)
            new PresetDTO
            {
                AllowAARP = existingDTO.AllowAARP,
                AllowAgent = existingDTO.AllowAgent,
                AllowBusinessSolutions = existingDTO.AllowBusinessSolutions,
                AllowDBA = existingDTO.AllowDBA,
                AllowEagle = existingDTO.AllowEagle,
                AllowGO = existingDTO.AllowGO,
                AllowHO = existingDTO.AllowHO,
                AllowLongTermCare = existingDTO.AllowLongTermCare,
                AllowNautilus = existingDTO.AllowNautilus,
                AllowRegisteredRep = existingDTO.AllowRegisteredRep,
                BrandId = existingDTO.BrandId,
                CategoryIds = webPresetDTO.CategoryIds,
                ClientCode = webPresetDTO.ClientCode,
                Description = webPresetDTO.Description,
                ExpirationDt = webPresetDTO.ExpirationDt,
                Id = 0,
                IsDeleted = false,
                IsPrivate =
                    // Set to true when preset is being created for site and not inheritted from CL (by agent for agent)
                    siteId.HasValue && siteId != 0 &&
                    !webPresetDTO.ParentPresetId.HasValue,
                ParentPresetId =
                    // Set to value when a Preset is inheritted/leveraged from CL (not created by agent). existingDTO.Id should match webPresetDTO.ParentPresetId
                    siteId.HasValue && siteId != 0 &&
                    webPresetDTO.ParentPresetId.HasValue && webPresetDTO.ParentPresetId != 0 ?
                        // ParentPresetIds match, this is an update of existing site level Preset, else existing is parent, this is a copy (update) of a global Preset, set ParentId to existing
                        webPresetDTO.ParentPresetId.HasValue &&
                        existingDTO.ParentPresetId.HasValue &&
                        webPresetDTO.ParentPresetId == existingDTO.ParentPresetId ? 
                            webPresetDTO.ParentPresetId.Value : 
                            existingDTO.Id :
                        null,
                PresetSettings =
                    siteId.HasValue && siteId != 0 &&
                    webPresetDTO.ParentPresetId.HasValue && webPresetDTO.ParentPresetId != 0 &&
                    webPresetDTO.ParentPresetId != existingDTO.ParentPresetId ?
                        // immutable when inheritting from parent (may change for use case, but FileName, FileType, Url can't be updated)
                        existingDTO.PresetSettings :
                        new CommonAndHostedAssetSettings()
                        {
                            // immutable
                            FileName = existingDTO.PresetSettings.FileName,
                            FileType = existingDTO.PresetSettings.FileType,
                            Url = existingDTO.PresetSettings.Url,

                            // mutable
                            Action = webPresetDTO.PresetSettings.Action,
                            AltText = webPresetDTO.PresetSettings.AltText,
                            Dimension= webPresetDTO.PresetSettings.Dimension,
                            LicenseType = webPresetDTO.PresetSettings.LicenseType,
                            Source = webPresetDTO.PresetSettings.Source,
                            SourceUrl = webPresetDTO.PresetSettings.SourceUrl,
                            Text = webPresetDTO.PresetSettings.Text,
                            WebHostedAssetUrl = webPresetDTO.PresetSettings.WebHostedAssetUrl,
                            HasAgreedToLicensing = webPresetDTO.PresetSettings.HasAgreedToLicensing
                        },
                // SiteId can only be set on create
                SiteId = siteId.HasValue && siteId != 0 ? siteId.Value : null,
                SMRUCode = webPresetDTO.SMRUCode,
                Title = webPresetDTO.Title,
                Type = existingDTO.Type
            };

        public static List<PresetDTO> FilterForUserAttributes(this IEnumerable<PresetDTO> presets, ADUser user) =>
            presets.Where(w =>
                (user.AARP ? w.AllowAARP == user.AARP : false) ||
                (user.AllowBusinessSolutions ?  w.AllowBusinessSolutions == user.AllowBusinessSolutions : false) ||
                (user.ApprovedDBA ? w.AllowDBA == user.ApprovedDBA : false) ||
                (user.EagleAdvisor ? w.AllowEagle == user.EagleAdvisor : false) ||
                (user.LongTermCare ? w.AllowLongTermCare == user.LongTermCare : false) ||
                (user.Nautilus ? w.AllowNautilus == user.Nautilus : false) ||
                (user.RegisteredRep ? w.AllowRegisteredRep == user.RegisteredRep : false) ||
                (user.IsAgent ? w.AllowAgent == user.IsAgent : false) ||
                (user.IsGeneralOffice ? w.AllowGO == user.IsGeneralOffice : false) ||
                (user.IsHomeOffice ? w.AllowHO == user.IsHomeOffice : false))
            .ToList();
    }
}