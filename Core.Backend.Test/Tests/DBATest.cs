using Core.Backend.Controllers;
using Core.CoreLib.Extensions;
using Core.CoreLib.Models.DTO.Profile;
using Core.CoreLib.Services.Client.NYL;
using Core.CoreLib.Services.Database;
using Core.CoreLib.Services.Database.Profile;
using Core.CoreLib.Services.Http;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Backend.Test.Tests
{
    public class DBATest : TestBase
    {
        private IConfigurationRoot _config;
        private ProfileService _profileService;
        private ILogger<Controller> _logger;
        private TelemetryClient _telemetryClient;
        private IDbaService _dbaService;

        public DBATest()
        {
            // Scaffold
            _config = CreateConfig();
            var services = CreateServices(_config);

            _telemetryClient = services?.GetService<TelemetryClient>() ?? throw new Exception("Unable to invoke TelemetryClient");

            var clientFactory =
                services?.GetService<IHttpClientFactory>() ?? throw new Exception("Unable to invoke IHttpClientFactory");

            _dbaService =
                new DbaService(new DapperContext(_config), _telemetryClient) ??
                throw new Exception("Unable to invoke DBA Service");

            _profileService = 
                new ProfileService(
                    clientFactory,
                    _config,
                    new TokenService(
                        clientFactory,
                        _telemetryClient),
                    _dbaService,
                    _telemetryClient);
            
            _logger = Mock.Of<ILogger<Controller>>();
        }

        [Fact]
        public async Task DBACRUDTests()
        {
            // I think DBAService could use some adjsutments as far as query/returntypes
            var data = BuildFormDataObject();
            var testGUID = Guid.NewGuid().ToString();

            // Create
            var result =
                await
                _dbaService.InsertDba(data, testGUID);

            Assert.True(result != null);

            // Read
            var result2 =
                await
                _dbaService.GetDbaUrlGuid(data.WebSite.UrlGuid);

            Assert.True(result2 != null);
            Assert.True(result2.UrlGuid == data.WebSite.UrlGuid);

            // Update
            data.WebSite.LastModifiedDate = DateTime.UtcNow;

            var result3 =
                await
                _dbaService.UpdateDba(data, data.WebSite.UrlGuid);

            Assert.True(result3 != null);
        }

        [Fact]
        public async Task SubmitEmpty_Dba_Request_Returns400()
        {
            // Arrange

            var controller = new DbaController(_profileService, _telemetryClient, _logger);

            // Act
            var result = await controller.SubmitDba(null!);

            //Assert
            var response = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(response.StatusCode == 400);
            Assert.Equal("Request is empty.", response.Value);
        }

        [Fact]
        public async Task Update_NonExistent_DbaLogo_Returns404()
        {
            // Arrange
            var formData = BuildFormDataObject();

            var controller = new DbaController(_profileService, _telemetryClient, _logger);

            // Act
            var result = await controller.UpdateDba(formData);

            //Assert
            var response = Assert.IsType<ObjectResult>(result);
            Assert.True(response.StatusCode == 404);
        }

        [Fact]
        public async Task Submit_Dba_With_Field_Containing_Special_Characters_Returns400()
        {
            // Arrange
            var formData = BuildFormDataObject();
            formData.Dba.DisplayName = "Test—Title?!@#$%^&*=*+-©";

            Assert.False(formData.Dba.DisplayName.ContainsOnlyAllowedCharacters());

            var controller = new DbaController(_profileService, _telemetryClient, _logger);

            // Act
            var result = await controller.SubmitDba(formData);

            //Assert
            var response = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(response.StatusCode == 400);
            Assert.Contains("contains invalid characters", (response.Value ?? string.Empty).ToString());
        }

        [Fact]
        public async Task Submit_Dba_With_Invalid_PhoneNumber_Returns400()
        {
            // Arrange
            var formData = BuildFormDataObject();

            formData.Marketers.ForEach(x =>
            {
                x.PhoneNumber = "312455-a456-226";
                Assert.False(x.PhoneNumber.ContainsNumbersOnly());
            });

            var controller = new DbaController(_profileService, _telemetryClient, _logger);

            // Act
            var result = await controller.SubmitDba(formData);

            //Assert
            var response = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(response.StatusCode == 400);
            Assert.Contains("Invalid phone number", (response.Value ?? string.Empty).ToString());
        }

        private static SubmitDbaDTO BuildFormDataObject()
        {
            return 
                new SubmitDbaDTO()
                {
                    SmruApprovalDate = DateTime.UtcNow,
                    Dba = 
                        new Dba()
                        {
                            DisplayName = "Test Display Name",
                            Name = "Test Name",
                            LogoCreatedDate = DateTime.UtcNow,
                            Image = 
                                new Image
                                {
                                    Data = "iVBORw0KGgoAAAANSUhEUgAAAsoAAAKMBAMAAAAAuYMCAAAAGFBMVEX/////",
                                    Type = "image/png"
                                }
                        },
                    WebSite = 
                        new WebSite()
                        {
                            UrlGuid = Guid.NewGuid().ToString(),
                            Owner = "Test Owner",
                            Status = "active",
                            Url = "https://somewebsite.com",
                            CreatedDate = DateTime.UtcNow,
                            LastModifiedDate = DateTime.UtcNow
                        },
                    Marketers = 
                        new List<Marketer>()
                        {
                            new Marketer()
                            {
                                MarketerId = "0845357",
                                OptInInd = false,
                                Title = "Test Title",
                                PrefEmailAddress = "some_email@newyorklife.com",
                                IsPrimary = true,
                                PhoneNumber = ""
                            },
                            new Marketer()
                            {
                                MarketerId = "0084621",
                                OptInInd = false,
                                Title = "Test Title",
                                PrefEmailAddress = "some_email@newyorklife.com",
                                IsPrimary = false,
                                PhoneNumber = "2144030103"
                            }
                        }
                };
        }
    }
}