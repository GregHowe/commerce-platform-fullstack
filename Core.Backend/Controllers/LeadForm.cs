using Core.CoreLib.Services.Client.NYL;
using CoreBackend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.CoreLib.Models.DTO.LeadForm;
using System.Text.Json.Serialization;
using System.Text.Json;
using Core.CoreLib.Services.Form;
using Core.Backend.Extensions;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Extensions;

namespace Core.Backend.Controllers
{
    [Route("api/leads")]
    [ApiController]
    public class LeadFormController : ControllerBaseTokenized
    {
        private readonly ILeadFormService _leadFormService;
        private readonly ILogger<Controller> _logger;
        protected IFormService _formService;

        public LeadFormController(
            ILeadFormService leadFormService,
            IFormService formService,
            ILogger<Controller> logger)
        {
            _leadFormService = leadFormService;
            _formService = formService;
            _logger = logger;
        }

        [HttpPost("submit")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitLeadForm(SubmitLeadFormDTO formData)
        {
            try
            {
                if (formData == null)
                    return BadRequest("Form data missing or empty.");
                else
                {
                    formData.LeadSource = LeadFormFields.FusionLeadSource;
                    formData.LastModifiedBy = LeadFormFields.LastModifiedBy;
                    formData.ContactMethod =
                        !string.IsNullOrWhiteSpace(formData.ContactMethod) ?
                            formData.ContactMethod :
                            LeadFormFields.DefaultContactMethod;
                }

                // Geenerate the reference number 
                if (formData.SiteId.HasValue) 
                {
                    // Assign the reference number to the form to be sent
                    formData.ReferenceNumber = await _formService.GenerateReferenceNumber(formData.SiteId.Value);

                    // NYL does not expect this data and may reject it, set to null
                    formData.SiteId = null;
				}

                var payload =
                    JsonSerializer.Serialize(
                        formData,
                        new JsonSerializerOptions
                        {
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        });

                if (string.IsNullOrWhiteSpace(payload))
                    return BadRequest("Unable to serialize form data to payload");

                var response =
                    await
                    _leadFormService
                    .SubmitNYLCLTLead(payload);

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
    }
}