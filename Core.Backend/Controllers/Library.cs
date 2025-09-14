
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.CoreLib.Models.DTO.Library;
using Core.CoreLib.Services.Database.Library;
using Core.CoreLib.Services.Azure.Storage;
using Core.Backend.Extensions;
using Core.Backend.Controllers.Attributes;
using Core.CoreLib.Models.Constants;
using Core.Backend.Models.Web.Preset;
using Core.Backend.Extensions.DBOs;
using Core.Backend.Extensions.Validators;

namespace CoreBackend.Controllers;

[Route("api/library")]
[ApiController]
[Authorize]
public sealed class LibraryController : ControllerBaseTokenized
{
    private readonly ILibraryService _libraryRepo;
    private readonly IStorageService _storageRepo;
    private readonly ILogger<Controller> _logger;

    public LibraryController(
        ILibraryService libraryRepo, 
        IStorageService storageRepo,
        ILogger<Controller> logger)
    {
        _libraryRepo = libraryRepo;
        _storageRepo = storageRepo;
        _logger = logger;
    }

    [HttpGet("categories")]
    [ClaimRequirement(new string[] { Authorization.Permissions.ReadContent })]
    public async Task<IActionResult> GetCategoryList()
    {
        try
        {           
            var result = 
                await 
                _libraryRepo
                .GetCategoryList(
                    GetCurrentBrandId());

                return Ok(result);
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPost("categories/create")]
    [ClaimRequirement(new string[] { Authorization.Permissions.CreateContent })]
    public async Task<IActionResult> CreateCategory(CategoryDTO category)
    {
        try
        {           
            var result = 
                await 
                _libraryRepo
                .CreateCategory(
                    GetCurrentBrandId(), 
                    category);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPut("categories/{categoryId}")]
    [ClaimRequirement(new string[] { Authorization.Permissions.UpdateContent })]
    public async Task<IActionResult> UpdateCategory(int categoryId, CategoryDTO category)
    {
        try
        {
            return
                (await _libraryRepo.GetCategory(categoryId)) == null ?
                    NotFound($"No available category found matching id {categoryId}") :
                    Ok(await _libraryRepo.UpdateCategory(GetCurrentBrandId(), category));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpDelete("categories/{categoryId}")]
    [ClaimRequirement(new string[] { Authorization.Permissions.DeleteContent })]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        try
        {           
            if ((await _libraryRepo.GetCategory(categoryId)) == null)
                return NotFound($"No available category found matching id {categoryId}");
            
            await _libraryRepo.DeleteCategory(categoryId);

            // Inherited this code, is front end expecting this?
            return NoContent();
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("themes")]
    [ClaimRequirement(new string[] { Authorization.Permissions.ReadContent })]
    public async Task<IActionResult> GetLibraryThemes()
    {
        try
        {
            return 
                Ok(await _libraryRepo.GetThemeList(GetCurrentBrandId()));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("themes/{themeId}")]
    [ClaimRequirement(new string[] { Authorization.Permissions.ReadContent })]
    public async Task<IActionResult> GetTheme(int themeId)
    {
        try
        {
            return 
                Ok(await _libraryRepo.GetTheme(themeId));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("presets")]
    //[ClaimRequirement(new string[] { Authorization.Permissions.ReadContent })]
    [AllowAnonymous]
    public async Task<IActionResult> GetPresetList()
    {
        try
        {
            return 
                Ok(await _libraryRepo.GetPresetList(GetCurrentBrandId()));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("presets/type/{type}")]
    //[ClaimRequirement(new string[] { Authorization.Permissions.ReadContent })]
    [AllowAnonymous]
    public async Task<IActionResult> GetPresetListByType(string type)
    {
        try
        {
            return 
                Ok(await _libraryRepo.GetPresetList(GetCurrentBrandId(), type));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }
    
    [HttpGet("presets/{presetId}")]
    // [ClaimRequirement(new string[] { Authorization.Permissions.ReadContent })]
    [AllowAnonymous]
    public async Task<IActionResult> GetPreset(int presetId)
    {
        try
        {
            return 
                Ok(await _libraryRepo.GetPreset(presetId));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPost("presets/create")]
    //[ClaimRequirement(new string[] { Authorization.Permissions.CreateContent })]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePreset(WebCreatePresetDTO preset)
    {
        try
        {
            var result =
                preset.Validate();

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return
                Ok(await _libraryRepo.CreatePreset(GetCurrentBrandId(), preset.ToPresetDBO()));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPut("presets")]
    //[ClaimRequirement(new string[] { Authorization.Permissions.UpdateContent })]
    [AllowAnonymous]
    public async Task<IActionResult> UpdatePreset(WebUpdatePresetDTO preset)
    {
        try
        {
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

            return
                Ok(await _libraryRepo.CreatePreset(GetCurrentBrandId(), preset.MergeUpdateDTO(existingPreset)));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpDelete("presets/{presetId}")]
    [AllowAnonymous]
    //[ClaimRequirement(new string[] { Authorization.Permissions.DeleteContent })]
    public async Task<IActionResult> DeletePreset(int presetId)
    {
        try
        {           
            if ((await _libraryRepo.GetPreset(presetId)) == null)
                return NotFound($"No Preset found matching id {presetId}");
            
            await _libraryRepo.DeletePreset(presetId);

            return NoContent();
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPost("media/upload")]
    [ClaimRequirement(new string[] { Authorization.Permissions.CreateContent })]
    public async Task<IActionResult> UploadMedia(
        IFormFile file,
        [FromForm] WebCreatePresetDTO preset)
    {
        try
        {
            // Validate, upload, create
            var result =
               preset.Validate(file);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            var uploadResult =
                await
                _storageRepo
                .UploadMediaAsync(
                    GetCurrentBrandId(),
                    file.FileName,
                    file.OpenReadStream());
            
            return
                Ok(
                    await
                    _libraryRepo
                    .CreatePreset(
                        GetCurrentBrandId(),
                        preset.ToPresetDBO(uploadResult.Blob)));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPut("media/upload")]
    [ClaimRequirement(new string[] { Authorization.Permissions.UpdateContent })]
    public async Task<IActionResult> UpdateMedia(
        IFormFile file,
        [FromForm] WebUpdatePresetDTO preset)
    {
        try
        {
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
                .UploadMediaAsync(
                    GetCurrentBrandId(),
                    existingPreset.PresetSettings.FileName,
                    file.OpenReadStream());

            return
                Ok(
                    await
                    _libraryRepo
                    .CreatePreset(
                        GetCurrentBrandId(),
                         preset.MergeUpdateDTO(existingPreset)));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }
}