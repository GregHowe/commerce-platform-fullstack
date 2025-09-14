
using Microsoft.Extensions.Configuration;
using System.Text;
using Core.CoreLib.Models.Constants;
using Microsoft.ApplicationInsights;
using Core.CoreLib.Extensions;
using Core.CoreLib.Services.Http;

namespace Core.CoreLib.Services.Client.NYL
{
    public class LeadFormService : ILeadFormService
    {
        protected IConfiguration _config;
        protected TelemetryClient _telemetryClient;
        protected ITokenService _tokenService;

        protected HttpClient _httpClient;

        private const string TelemetryEventName = "LeadFormService::SubmitNYLCLTLead";

        public LeadFormService(
            IHttpClientFactory httpClientFactory,
            IConfiguration config,
            TelemetryClient telemetryClient,
            ITokenService tokenService)
        {
            _config = config;
            _telemetryClient = telemetryClient;
            _tokenService = tokenService;

            _httpClient = httpClientFactory.CreateClient(HttpNamedClients.NYLCLTLeadSubmissionClient);
        }

        public async Task<HttpResponseMessage> SubmitNYLCLTLead(string formData)
        {
            _telemetryClient.TrackEvent(
                TelemetryEventName,
                new Dictionary<string, string> { { "formData", formData } });

            var bearerToken = 
                await _tokenService.GenerateBearerTokenAsync(
                    new Dictionary<string, string>
                    {
                        {"grant_type", "client_credentials"},
                        {"client_id", _config["NYL:ClientId"]},
                        {"client_secret", _config["NYL:ClientSecret"]},
                        {"scope", "READ"},
                    },
                    _config["NYL:TokenUrl"]);

            //https://mdl.f92clt.ws.newyorklife.com/PRO-MS/clt-leads-pxy/api/insertLeads/
            var request =
                new HttpRequestMessage(
                    HttpMethod.Post,
                    _config["NYL:LeadFormSubmissionUrl"]);
            request.Headers.Add("Authorization", $"Bearer {bearerToken}");
            request.Content = new StringContent(formData, Encoding.UTF8, "application/json");

            _telemetryClient.TrackEvent(
                TelemetryEventName,
                new Dictionary<string, string> { { "request", request.ToString() } });

            var response =
                await
                _httpClient.SendAsync(request);

            _telemetryClient.TrackEvent(
                 TelemetryEventName,
                 new Dictionary<string, string> { { "response", response.ToString() } });

            return
                response;
        }
    }
}