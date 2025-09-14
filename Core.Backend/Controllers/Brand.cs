using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.CoreLib.Services.Database.Brand;
using Core.CoreLib.Models.DTO.Brand;
using Core.Backend.Extensions;

namespace CoreBackend.Controllers;

[Route("api/brands")]
[ApiController]
[Authorize]
public sealed class BrandController : ControllerBase
{
    private readonly IBrandService _brandRepo;
    private readonly ILogger<Controller> _logger;

    public BrandController(
        IBrandService brandRepo,
        ILogger<Controller> logger)
    {
        _brandRepo = brandRepo;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrandList()
    {
        try
        {
            var brands = await _brandRepo.GetBrandList();
            return Ok(brands);
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [AllowAnonymous]
    [HttpGet("{brandId}", Name = "BrandById")]
    public async Task<IActionResult> GetBrand(int brandId)
    {
        try
        {
            var brand = await _brandRepo.GetBrand(brandId);
            return Ok(brand);
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [AllowAnonymous]
    [HttpGet("by/host", Name = "BrandByHost")]
    public async Task<IActionResult> GetBrandByHost()
    {
        try
        {
            var header = Request.GetTypedHeaders().Referer;

            if (header != null && !string.IsNullOrWhiteSpace(header.Host))
                return Ok(
                    await 
                    _brandRepo.GetBrandByHost(header.Host.ToLower().ToString()));
            else
                return Ok(null);

            //var host = ;
            //var brand = await connection.QuerySingleOrDefaultAsync<GetBrandDTO>("SELECT * FROM Brands WHERE Host = @Host", new { host });
            //return brand;
            //}
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPut("{brandId}")]
    public async Task<IActionResult> UpdateBrand(int brandId, UpdateBrandDTO brand)
    {
        try
        {
            await _brandRepo.UpdateBrand(brandId, brand);
            return NoContent();
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }
}