using Core.Backend.Extensions;
using Core.CoreLib.Models.Tooling;
using Core.CoreLib.Services.Tooling;
using CoreBackend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Backend.Controllers.Tooling
{
    [Route("api/tooling")]
    [ApiController]
    public class RecaptchaController : ControllerBaseTokenized
    {
        private IGRecaptchaService _recaptchaService;
        private readonly ILogger<Controller> _logger;

        public RecaptchaController(
            IGRecaptchaService recaptchaService, 
            IConfiguration config,
            ILogger<Controller> logger)
        {
            _recaptchaService = recaptchaService;
            _logger = logger;
        }

        [HttpPost("submit/recaptcha")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitRecaptcha(GRRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Missing data or empty request.");

                if (string.IsNullOrWhiteSpace(request.ResponseToken))
                    return BadRequest("Missing the response token.");

                var response =
                    await
                    _recaptchaService
                    .SubmitRecaptcha(
                        new GRRequest(request.ResponseToken));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }
    }
}