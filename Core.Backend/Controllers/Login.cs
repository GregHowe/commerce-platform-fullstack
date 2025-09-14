using Core.Backend.Extensions;
using Core.CoreLib.Models.Database.Core.Settings;
using Core.CoreLib.Services.Database.Brand;
using CoreBackend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Core.Backend.Controllers
{
    [Route("api/login")]
    [ApiController]
    [Authorize]
    public sealed class LoginController : ControllerBaseTokenized
    {
        private readonly IBrandService _brandService;
        private readonly ILogger<Controller> _logger;

        public LoginController(
            IBrandService brandService,
            ILogger<Controller> logger)
        {
            _brandService = brandService;
            _logger = logger;
        }

        [HttpGet("settings")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginSettings(string brand)
        {
            try
            {
                if (!int.TryParse(brand, out var brandId))
                    return BadRequest("Invalid brand");

                var brandSettings =
                    await _brandService.GetBrand(brandId);

                if (brandSettings == null ||
                    string.IsNullOrWhiteSpace(brandSettings.Settings))
                    return BadRequest($"Brand settings missing for brand: {brand}");

                return 
                    Ok(JsonSerializer.Deserialize<BrandSettings>(brandSettings.Settings));
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }
    }
}
