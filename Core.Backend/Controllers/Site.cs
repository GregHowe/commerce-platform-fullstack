
using Core.Backend.Extensions;
using Core.CoreLib.Services.Azure.Storage;
using Core.CoreLib.Services.Database.Site;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Core.CoreLib.Models.Constants.Authorization;
using Core.CoreLib.Services.Database.Library;
using Core.Backend.Models.Web.Preset;
using Core.Backend.Extensions.Validators;
using Core.Backend.Extensions.DBOs;
using Core.Backend.Controllers.Attributes;

namespace CoreBackend.Controllers;

[Route("api/sites")]
[ApiController]
public sealed class SiteController : ControllerBaseTokenized
{
    private readonly ISiteService _siteRepo;
    private readonly ILibraryService _libraryService;
    private readonly ILibraryService _libraryRepo;
    private readonly IStorageService _storageRepo;
    private readonly Core.CoreLib.Services.User.IUserService _userService;
    private readonly Core.CoreLib.Services.Database.User.IUserService _dbUserService;
    private readonly ILogger<Controller> _logger;

    public SiteController(
        ISiteService siteRepo,
        ILibraryService libraryService,
        ILibraryService libraryRepo,
        IStorageService storageRepo,
        Core.CoreLib.Services.User.IUserService userService,
        Core.CoreLib.Services.Database.User.IUserService dbUserService,
        ILogger<Controller> logger)
    {
        _siteRepo = siteRepo;
        _libraryService = libraryService;
        _libraryRepo = libraryRepo;
        _storageRepo = storageRepo;
        _userService = userService;
        _dbUserService = dbUserService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetSiteList()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(GetCurrentUserId()))
                return NotFound("User not found");

            if (HasPermission(Permissions.ListSites))
                return Ok(await _siteRepo.GetSiteListByBrand(GetCurrentBrandId()));

            else if (IsOBOSession())
            {
                var adUser = 
                    await _userService.GetSSOADUserAsync(GetOBOSessionId());

                return
                    adUser == null ?
                        NotFound("OBO user not found") :
                        Ok(await _siteRepo.GetSiteListByUser(adUser.GraphUserData.Mail));
            }
            
			else
                return Ok(await _siteRepo.GetSiteListByUser(GetCurrentUserId()));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPost("create", Name = "CreateSite")]
    //[ClaimRequirement(new string[] { Permissions.CreateSite })]
    [AllowAnonymous]
    public async Task<IActionResult> CreateSite()
    {
        try
        {
            var user =
                await
                _userService.GetADUserAsync(GetCurrentUserId());

            var site = 
                await _siteRepo.CreateSite(GetCurrentBrandId(), user.GraphUserData.Mail, user.GraphUserData.Id, 3);

            return
                site != null ?
                    Ok(await site.ToSiteDTO(_userService)) :
                    NotFound($"Site matching brand {GetCurrentBrandId()} and user {GetCurrentUserId()} not found");
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("{siteId}", Name = "SiteById")]
    //[ClaimRequirement(new string[] { Permissions.ListSites })]
    [AllowAnonymous]
    public async Task<IActionResult> GetSite(int siteId)
    {
        try
        {
            var site = 
                await _siteRepo.GetSite(siteId);

            if (site == null)
                return NotFound($"Site matching id {siteId} not found");

            if (string.IsNullOrWhiteSpace(site.UserId))
                return NotFound($"No user attached to site with Id {siteId}");

            var user =
                await _userService.GetADUserAsync(site.UserId);

            // Get DDCData
            var detail =
                await
                _dbUserService.GetUserAccessInfoDetailDDCData(user.SSOId);

            if (detail != null)
                detail.UserLicensesDetailDDC =
                    await
                    _dbUserService.GetUserLicenseInfoDetailDDCData(user.SSOId);

            return 
                Ok(await site.ToSiteDTO(_userService, detail!));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPut("{siteId}")]
    //[ClaimRequirement(new string[] { Permissions.SaveSite })]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateSite(int siteId, Core.CoreLib.Models.DTO.Site.SiteDTO site)
    {
        try
        {
            // Inheritted code: Seems like Id field in site should match siteId???
            if ((await _siteRepo.GetSite(siteId)) == null)
                return NotFound($"Site matching id {siteId} not found");

            var record = 
                await _siteRepo.UpdateSite(siteId, site);

            return
                record != null ?
                    Ok(await record.ToSiteDTO(_userService)) :
                    NotFound($"Update failed, no site returned for Id {siteId}");
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPut("{siteId}/publish")]
    //[ClaimRequirement(new string[] { Permissions.PublishSite })]
    [AllowAnonymous]
    public async Task<IActionResult> PublishSite(int siteId, Core.CoreLib.Models.DTO.Site.SiteDTO site)
    {
        try
        {
            // Inheritted code: Seems like Id field in site should match siteId???
            if ((await _siteRepo.GetSite(siteId)) == null)
                return NotFound($"Site matching id {siteId} not found");

            return Ok(await _siteRepo.PublishSite(siteId, site));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpDelete("{siteId}")]
    //[ClaimRequirement(new string[] { Permissions.DeleteSite })]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteSite(int siteId)
    {
        try
        {
            if ((await _siteRepo.GetSite(siteId)) == null)
                return NotFound($"Site matching id {siteId} not found");

            await _siteRepo.DeleteSite(siteId);

            return NoContent();
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPost("{siteId}/pages/create")]
    // [ClaimRequirement(new string[] { Permissions.CreateSite })]
    [AllowAnonymous]
    public async Task<IActionResult> CreateSitePage(int siteId)
    {
        try
        {
            if ((await _siteRepo.GetSite(siteId)) == null)
                return NotFound($"Site matching id {siteId} not found");

            return 
                Ok(await _siteRepo.CreateSitePage(Guid.NewGuid().ToString(), siteId));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpDelete("{siteId}/pages/{pageId}")]
    //[ClaimRequirement(new string[] { Permissions.DeleteSite })]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteSitePage(int siteId, int pageId)
    {
        try
        {
            if (await _siteRepo.GetSitePage(siteId, pageId) == null)
                return NotFound($"No page found with page Id {pageId} and site Id {siteId}");
            
            await _siteRepo.DeleteSitePage(siteId, pageId);

            return NoContent();
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("{siteId}/presets")]
    //[ClaimRequirement(new string[] { Permissions.ReadSite })]
    [AllowAnonymous]
    public async Task<IActionResult> GetSitePresetList(int siteId)
    {
        try
        {
            var user =
                await _userService.GetADUserAsync(GetCurrentUserId());

            var globalPresets =
                await _libraryRepo.GetPresetList(GetCurrentBrandId());

            var filteredGlobalPresets =
                globalPresets.FilterForUserAttributes(user);

            var sitePresets =
                await _libraryService.GetSitePresetList(siteId);

            filteredGlobalPresets.AddRange(sitePresets);

            return 
                Ok(filteredGlobalPresets);
		}
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("{siteId}/presets/{presetId}")]
    //[ClaimRequirement(new string[] { Permissions.ReadSite })]
    [AllowAnonymous]
    public async Task<IActionResult> GetSitePreset(int siteId, int presetId)
    {
        try
        {
            var record = 
                await _libraryRepo.GetPreset(presetId);

            if (record.SiteId != siteId)
                throw new Exception($"Site Id ({siteId}) does not match site Id on requested Preset ({presetId})");

            return Ok(record);
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPost("{siteId}/presets/create")]
    //[ClaimRequirement(new string[] { Permissions.CreateSite })]
    [AllowAnonymous]
    public async Task<IActionResult> CreateSitePreset(int siteId, WebCreatePresetDTO preset)
    {
        try
        {
            var result =
               preset.Validate(siteId);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            if ((await _siteRepo.GetSite(siteId)) == null)
                return NotFound($"Site matching id {siteId} not found");

            if (preset.ParentPresetId.HasValue)
            {
                // Create site preset from CL (has parent). Get global preset, merge and create.
                var parentPreset =
                    await
                    _libraryRepo.GetPreset(preset.ParentPresetId.Value);

                if (parentPreset == null)
                    return BadRequest($"Invalid parent preset Id {preset.ParentPresetId.Value}");

                return
                    Ok(await _libraryRepo.CreatePreset(GetCurrentBrandId(), preset.MergeCreateDTO(parentPreset, siteId)));
            }
            else
                return
                    Ok(await _libraryRepo.CreatePreset(GetCurrentBrandId(), preset.ToPresetDBO(siteId)));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPut("{siteId}/presets/{presetId}")]
    //[ClaimRequirement(new string[] { Permissions.CreateSite })]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateSitePreset(int siteId, int presetId, WebUpdatePresetDTO preset)
    {
        try
        {
            if (preset.Id == 0 ||
                presetId != preset.Id ||
                (await _libraryRepo.GetPreset(preset.Id)) == null)
                return BadRequest($"Unable to update preset, no preset found in data matching id {preset.Id} ({presetId})");

            await
            _libraryRepo.DeletePreset(preset.Id);

            var existingPreset =
                await _libraryRepo.GetPreset(preset.Id);

            if (!existingPreset.IsDeleted)
                throw new Exception("Update failed. Failed to mark previous Preset as deleted");

            // TODO: Move all this to a validate extension
            if (siteId == 0)
            return BadRequest($"Invalid site Id");

            if (!existingPreset.SiteId.HasValue ||
                (existingPreset.SiteId.Value != siteId))
                return BadRequest($"Site Id ({siteId}) does not match site Id on requested Preset ({preset.Id})");

            // Two paths, update private and update inheritted from CL
            if (existingPreset.IsPrivate)
            {
                if (preset.ParentPresetId.HasValue)
                    return BadRequest($"Preset marked as private, unable to derive from global content, use create preset");

                // Exists at the site level and is not inheritted from CL (no parentId)
                return
                    Ok(await _libraryRepo.CreatePreset(GetCurrentBrandId(), preset.MergeUpdateDTO(existingPreset, siteId)));
            }
            else
            {
                if (existingPreset.ParentPresetId != preset.ParentPresetId)
                    return BadRequest($"Unable to change parent on preset update, use create preset");

                // Existing preset is built off parent so we can merge that, Parent is a global preset
                return
                    Ok(await _libraryRepo.CreatePreset(GetCurrentBrandId(), preset.MergeUpdateDTO(existingPreset, siteId)));
            }
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpDelete("{siteId}/presets/{presetId}")]
    //[ClaimRequirement(new string[] { Permissions.DeleteSite })]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteSitePreset(int siteId, int presetId)
    {
        try
        {
            var preset = 
                await _libraryRepo.GetPreset(presetId);

            if (preset == null)
                return NotFound($"No Preset found matching id {presetId}");

            if (!preset.SiteId.HasValue ||
                preset.SiteId.Value != siteId)
                return BadRequest($"Site Id ({siteId}) does not match site Id on requested Preset ({presetId})");

            await 
            _libraryRepo.DeletePreset(presetId);

            return NoContent();
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPost("{siteId}/media/upload")]
    [ClaimRequirement(new string[] { Permissions.CreateSite })]
    public async Task<IActionResult> UploadMedia(
        int siteId, 
        IFormFile file, 
        [FromForm] WebCreatePresetDTO preset)
    {
        try
        {
            // Validate, upload, create
            var result =
               preset.Validate(file, siteId);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            // Cant upload media for preset with parent

            var brandId = 
                GetCurrentBrandId();

            var uploadResult = 
                await 
                _storageRepo
                .UploadSiteMediaAsync(
                    brandId, 
                    siteId, 
                    file.FileName, 
                    file.OpenReadStream());

            return 
                Ok(
                    await 
                    _libraryRepo
                    .CreatePreset(
                        brandId,
                        preset.ToPresetDBO(uploadResult.Blob, siteId)));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPut("{siteId}/media/upload")]
    [ClaimRequirement(new string[] { Permissions.UpdateContent })]
    public async Task<IActionResult> UpdateMedia(
        int siteId,
        IFormFile file,
        [FromForm] WebUpdatePresetDTO preset)
    {
        try
        {
            if (siteId == 0)
                return BadRequest($"Invalid site Id");

            if (preset.Id == 0 ||
                (await _libraryRepo.GetPreset(preset.Id)) == null)
                return BadRequest($"Unable to update preset, no preset found in data matching id {preset.Id}");

            await
            _libraryRepo.DeletePreset(preset.Id);

            var existingPreset =
                await
                _libraryRepo.GetPreset(preset.Id, true);

            if (!existingPreset.IsDeleted)
                throw new Exception("Update failed. Failed to mark previous Preset as deleted");

            if (string.IsNullOrEmpty(existingPreset.PresetSettings.FileName))
                return BadRequest($"Unable to read file name on existing media preset");

            var uploadResult =
                await
                _storageRepo
                .UploadSiteMediaAsync(
                    GetCurrentBrandId(),
                    siteId,
                    existingPreset.PresetSettings.FileName,
                    file.OpenReadStream());

            return
                Ok(
                    await
                    _libraryRepo
                    .CreatePreset(
                        GetCurrentBrandId(),
                         preset.MergeUpdateDTO(existingPreset, siteId)));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }
}