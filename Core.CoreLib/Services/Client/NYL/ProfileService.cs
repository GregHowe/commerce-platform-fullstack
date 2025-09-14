using System.Net;
using System.Text;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Models.DTO.Profile;
using Core.CoreLib.Services.Database.Profile;
using Core.CoreLib.Services.Http;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Core.CoreLib.Services.Client.NYL
{
    public class ProfileService : IProfileService
    {
        protected IHttpClientFactory _httpClientFactory;
        protected IConfiguration _configuration;
        protected ITokenService _tokenService;
        protected IDbaService _dbaService;
        protected TelemetryClient _telemetryClient;
        protected HttpClient _httpClient;

        private const string TelemetryEventName = "ProfileService::";

        public ProfileService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ITokenService tokenService,
            IDbaService dbaService,
            TelemetryClient telemetryClient)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _dbaService = dbaService;
            _telemetryClient = telemetryClient;

            _httpClient = httpClientFactory.CreateClient(HttpNamedClients.NYLCLTLeadSubmissionClient);
        }

        public async Task<HttpResponseMessage> SubmitDba(SubmitDbaDTO dbaData)
        {
            var recordExists =
                await _dbaService.GetDbaUrlGuid(dbaData.WebSite.UrlGuid);

            if (recordExists != null)
                return new HttpResponseMessage(HttpStatusCode.Conflict);

            var dbaLogoGuid = Guid.NewGuid().ToString();

            var request =
               new HttpRequestMessage(
                   HttpMethod.Post,
                   $"{_configuration["NYL:DBASubmissionUrl"]}{dbaLogoGuid}");

            var response = await ProcessRequest(dbaData, dbaLogoGuid, request, "SubmitDBA");

            if (response.IsSuccessStatusCode)
                await _dbaService.InsertDba(dbaData, dbaLogoGuid);

            return
                response;
        }

        public async Task<HttpResponseMessage> UpdateDba(SubmitDbaDTO dbaData)
        {
            var recordExists =
                await _dbaService.GetDbaUrlGuid(dbaData.WebSite.UrlGuid);

            if (recordExists == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            if (recordExists.ImageBinary.Equals(dbaData.Dba.Image.Data))
                dbaData.Dba.Image.Data = string.Empty;

            var request =
               new HttpRequestMessage(
                   HttpMethod.Put,
                   $"{_configuration["NYL:DBASubmissionUrl"]}{recordExists.DbaLogoGuid}");

            var response =
                await ProcessRequest(dbaData, recordExists.DbaLogoGuid, request, "UpdateDBA");

            if (response.IsSuccessStatusCode)
                await _dbaService.UpdateDba(dbaData, dbaData.WebSite.UrlGuid);

            return
                response;
        }

        private async Task<HttpResponseMessage> ProcessRequest(SubmitDbaDTO dbaData, string dbaLogoGuid, HttpRequestMessage request, string requestType)
        {
            var bearerToken =
                await _tokenService.GenerateBearerTokenAsync(
                    new Dictionary<string, string>
                    {
                        {"grant_type", "client_credentials"},
                        {"client_id", _configuration["NYL:ClientId"]},
                        {"client_secret", _configuration["NYL:ClientSecret"]},
                        {"scope", "READ"},
                    },
                    _configuration["NYL:TokenUrl"]);
            request.Headers.Add("Authorization", $"Bearer {bearerToken}");

            var payload =
                JsonConvert.SerializeObject(
                    dbaData,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            _telemetryClient.TrackEvent(
                $"{TelemetryEventName}{requestType}",
                new Dictionary<string, string>
                {
                    { "DbaData", payload },
                    { "Request", request.ToString() },
                    { "dbaLogoGuid", dbaLogoGuid }
                });

            var response =
                await
                _httpClient.SendAsync(request);

            _telemetryClient.TrackEvent(
                $"{TelemetryEventName}{requestType}",
                new Dictionary<string, string>
                {
                    { "response", response.ToString() }
                });

            return
                response;
        }
    }
}