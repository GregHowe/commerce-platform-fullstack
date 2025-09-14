using System.Web;
using Core.CoreLib.Models.Tooling;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;

namespace Core.CoreLib.Services.Tooling
{
    public class GRecaptchaService : IGRecaptchaService
    {
        protected TelemetryClient _telemetryClient;
        protected IConfiguration _configuration;
        protected HttpClient _httpClient;

        private const string TelemetryEventName = "RecaptchaService::SubmitRecaptcha";

        public GRecaptchaService(IHttpClientFactory httpClientFactory,IConfiguration configuration, TelemetryClient telemetryClient)
        {
            _configuration = configuration;
            _telemetryClient = telemetryClient;

            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> SubmitRecaptcha(GRRequest recaptchaReq)
        {
            var builtURL =
                _configuration["GoogleRecaptcha:ApiUrl"] + '?' + 
                HttpUtility.UrlPathEncode($"secret={_configuration["GoogleRecaptcha:Secret"]}&response={recaptchaReq.ResponseToken}");

            var request =
                new HttpRequestMessage(HttpMethod.Post, builtURL)
                {
                    Content = new StringContent(builtURL)
                };

            _telemetryClient.TrackEvent(
                TelemetryEventName,
                new Dictionary<string, string> { { "request", request.ToString() } });

            var response =
                await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Recaptcha request: {response.RequestMessage}. Response: {response}. request data: {recaptchaReq}. Status Code: {response.StatusCode}");

            return
                await response.Content.ReadAsStringAsync();
        }
    }
}