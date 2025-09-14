using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Backend.Extensions;

namespace CoreBackend.Controllers;

[Route("api/pipeline")]
[ApiController]
[Authorize]
public sealed class PipelineController : ControllerBaseTokenized
{
    private readonly ILogger<Controller> _logger;

    public PipelineController(
        IConfiguration config,
        ILogger<Controller> logger)
    {
        _logger = logger;
    }

    [HttpGet("queue")]
    [AllowAnonymous]
    public IActionResult GetQueue()
    {
        try
        {
            var queue = new List<string>();

            return Ok(queue);
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }
}