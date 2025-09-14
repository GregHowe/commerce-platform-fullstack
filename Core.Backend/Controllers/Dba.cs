using Core.Backend.Extensions;
using Core.CoreLib.Extensions;
using Core.CoreLib.Models.DTO.Profile;
using Core.CoreLib.Services.Client.NYL;
using CoreBackend.Controllers;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Backend.Controllers
{
	[Route("api/dba")]
    [ApiController]
    public class DbaController : ControllerBaseTokenized
    {
        private readonly IProfileService _profileService;
        private readonly ILogger<Controller> _logger;
        protected TelemetryClient _telemetryClient;

        public DbaController(
            IProfileService profileService, 
            TelemetryClient telemetryClient, 
            ILogger<Controller> logger)
        {
            _profileService = profileService;
            _telemetryClient = telemetryClient;
            _logger = logger;
		}

        [HttpPost("submit")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitDba(SubmitDbaDTO dbaData)
        {
            try
            {
                var result = ValidateRequest(dbaData);
                if (result.Length > 0)
                    return BadRequest(result);

                var response = 
                    await _profileService.SubmitDba(dbaData);

                return 
                    StatusCode(
                        (int)response.StatusCode, 
                        await response.Content.ContentToStringAsync());
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpPut("update")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateDba(SubmitDbaDTO dbaData)
        {
            try
            {
                var result = ValidateRequest(dbaData);
                if (result.Length > 0)
                    return BadRequest(result);

                var response =
                    await _profileService.UpdateDba(dbaData);

                return
                    StatusCode(
                        (int)response.StatusCode,
                        await response.Content.ContentToStringAsync());
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        private static string ValidateRequest(SubmitDbaDTO dbaData)
        {
            if (dbaData == null)
                return "Request is empty.";

            if (!(dbaData.Dba?.DisplayName ?? string.Empty).ContainsOnlyAllowedCharacters())
                return $"Display name contains invalid characters: {(dbaData.Dba?.DisplayName ?? string.Empty)}.";

            foreach (var marketer in dbaData.Marketers)
                if (!(marketer.Title ?? string.Empty).ContainsOnlyAllowedCharacters())
                    return $"Marketer title contains invalid characters: {marketer.Title}";

            foreach (var marketer in dbaData.Marketers)
                if (!string.IsNullOrWhiteSpace(marketer.PhoneNumber) && !marketer.IsPrimary && !marketer.PhoneNumber.ContainsNumbersOnly())
                    return $"Invalid phone number '{marketer.PhoneNumber}' for marketer id: {marketer.MarketerId}";

            return string.Empty;
        }
    }
}