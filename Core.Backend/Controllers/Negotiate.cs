using Core.Backend.Extensions;
using CoreBackend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Backend.Controllers;

[Route("api/negotiate")]
[ApiController]
[Authorize]
public class Negotiate : ControllerBaseTokenized
{
    private readonly ILogger<Controller> _logger;

    public Negotiate(
        IConfiguration config,
        ILogger<Controller> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
		try
		{
            return Ok();
        }
		catch (Exception ex)
		{
            return ex.PackageExceptionResponse(_logger);
        }
    }
}