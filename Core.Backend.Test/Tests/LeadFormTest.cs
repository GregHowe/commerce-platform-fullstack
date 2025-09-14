
using Core.CoreLib.Models.DTO.LeadForm;
using Core.CoreLib.Services.Client.NYL;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Core.CoreLib.Services.Form;
using Core.CoreLib.Services.Database;
using Core.Backend.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.CoreLib.Services.Http;
using Microsoft.Extensions.Configuration;

namespace Core.Backend.Test.Tests
{
    public class LeadFormTest : TestBase
    {
        private LeadFormService _leadFormService;
        private FormService _formService;
        private ILogger<Controller> _logger;
        private TokenService _tokenService;
        private IConfigurationRoot _config;

        public LeadFormTest()
        {
            // Scaffold
            var config = CreateConfig();

            var services = CreateServices(config);
            var telemetryClient = services?.GetService<TelemetryClient>() ?? throw new Exception("Unable to invoke TelemetryClient");

            _tokenService =
                new TokenService(
                    services?.GetService<IHttpClientFactory>() ?? throw new Exception("Unable to invoke IHttpClientFactory"),
                    services?.GetService<TelemetryClient>() ?? throw new Exception("Unable to invoke TelemetryClient"));

            _leadFormService = 
                new LeadFormService(
                    services?.GetService<IHttpClientFactory>() ?? throw new Exception("Unable to invoke HttpCLientFactory"), 
                    config, 
                    telemetryClient,
                    _tokenService);

            _formService = new FormService(config, new DapperContext(config), telemetryClient);
            _logger = Mock.Of<ILogger<Controller>>();
        }

        [Fact]
        public async Task NYLleadFormSubmissionRawDataTest()
        {
            var sampleBody =
                @"{""referenceNumber"":"""",""firstName"": ""test"",""lastName"": ""test"",""address"": ""135 South 84th Street"",""city"": ""Milwaukee"",""state"": ""WI"",""zip"": ""53214"",""phoneNumber"": 9088494549,""emailAddress"": ""test@newyorklife.com"",""birthDate"": ""1975-08-29T04:00:00.000+0000"",""dateSubmitted"": ""2017-03-31T14:25:03.827+0000"",""marketerNumber"": ""0380246"",""sourceCode"": ""1Z7H4F"",""sourceCodeName"": ""Source Code 01"",""sourceCodeStartDate"": ""2020-07-21T04:00:00.000+0000"",""sourceCodeComments"": """",""sourceCodeEmailAddress"": ""test@newyorklife.com"",""campaignCode"": ""UL0142"",""campaignName"": ""Internet"",""campaignProgramCode"": ""A2"",""giftType"": ""none"",""pageUrl"": ""http://www.google.com"",""linkedinUrl"":""www.linkedin.com/user/"",""languagePref"":""hindi"",""bestTimeToCall"":""morning"",""leadConcerns"":""Lead Concerns"",""leadProcessType"":""LEAD_PROCESS_TYPE"",""orgUnitCD"":""a99"",""metaData"":[{""key"": ""protection"",""value"": ""Protection""},{""key"": ""retirementplanning"",""value"": ""Retirement Planning""},{""key"": ""wealthaccumulation"",""value"": ""Wealth Accumulation""},{""key"": ""estateplanning"",""value"": ""Estate Planning""},{""key"": ""smallbusinessadvisory"",""value"": ""Small Business Advisory""}]}";

            var result =
                await
                _leadFormService
                .SubmitNYLCLTLead(sampleBody);

            Assert.True(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task NYLLeadFormSubmissionObjectTest()
        {
            var payload =
                JsonConvert.SerializeObject(
                    BuildFormDataObject(),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    });

            var result =
                await
                _leadFormService
                .SubmitNYLCLTLead(payload);

            Assert.True(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task SubmitValidLeadForm_Returns200()
        {
            // Arrange
            var controller = 
                new LeadFormController(_leadFormService, _formService, _logger);

            // Act
            var result =
                await 
                controller
                .SubmitLeadForm(
                    BuildFormDataObject());

            //Assert
            var response = Assert.IsType<ObjectResult>(result);
            Assert.True(response.StatusCode == 200);
        }

        [Fact]
        public async Task SubmitEmptyLeadForm_Returns400()
        {
            // Arrange
            var controller = new LeadFormController(_leadFormService, _formService, _logger);

            // Act - Test null form data
            var result = await controller.SubmitLeadForm(null!);

            //Assert
            var response = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(response.StatusCode == 400);
            Assert.Equal("Form data missing or empty.", response.Value);
        }

        private static SubmitLeadFormDTO BuildFormDataObject() =>
            new SubmitLeadFormDTO()
            {
                Address = "135 South 84th Street",
                City = "Milwaukee",
                State = "WI",
                EmailAddress = "test@newyorklife.com",
                FirstName = "test",
                LastName = "test",
                Zip = "53214",
                ReferenceNumber = "",
                PhoneNumber = "9088494549",
                MarketerNumber = "0380246",
                SourceCode = "1Z7H4F",
                SourceCodeStartDate = "2020-07-21T04:00:00.000+0000",
                CampaignCode = "UL0142",
                CampaignName = "Internet",
                CampaignProgramCode = "A2",
                SourceCodeName = "Source Code 01",
                SourceCodeEmailAddress = "test@newyorklife.com",
                GiftType = "none",
                BirthDate = "1975-08-29T04:00:00.000+0000",
                DateSubmitted = "2017-03-31T14:25:03.827+0000",
                PageUrl = "http://www.google.com",
                LinkedinUrl = "www.linkedin.com/user/",
                LanguagePref = "hindi",
                BestTimeToCall = "morning",
                LeadConcerns = "Lead Concerns",
                LeadProcessType = "LEAD_PROCESS_TYPE",
                OrgUnitCD = "a99",
                MetaData =
                    new List<MetaDataItem>()
                    {
                        new MetaDataItem() { Key = "protection", Value = "Protection" },
                        new MetaDataItem() { Key = "retirementplanning", Value = "Retirement Planning" },
                        new MetaDataItem() { Key = "wealthaccumulation", Value = "Wealth Accumulation" },
                        new MetaDataItem() { Key = "estateplanning", Value = "Estate Planning" },
                        new MetaDataItem() { Key = "smallbusinessadvisory", Value = "Small Business Advisory" }
                    }
            };
    }
}