
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;
using Core.CoreLib.Models.Constants;

namespace Core.CoreLib.Services.Http
{
	public class TokenService : ITokenService
    {
        protected IHttpClientFactory _httpClientFactory;
        protected TelemetryClient _telemetryClient;
        protected HttpClient _httpClient;

        private const string TelemetryEventName = "TokenService";

        public TokenService(
            IHttpClientFactory httpClientFactory,
            TelemetryClient telemetryClient)
        {
            _httpClientFactory = httpClientFactory;
            _telemetryClient = telemetryClient;

            _httpClient = _httpClientFactory.CreateClient(HttpNamedClients.NYLCLTLeadSubmissionClient);
        }

        public async Task<string> GenerateBearerTokenAsync(Dictionary<string, string> content, string tokenUrl)
        {           
            _telemetryClient.TrackTrace($"{TelemetryEventName}: Generating Bearer Token");

            var tokenResponse =
                await
                _httpClient
                .PostAsync(
                    tokenUrl,
                    new FormUrlEncodedContent(content));

            var jsonContent =
                await
                tokenResponse.Content.ReadAsStringAsync();

            var token =
                JsonConvert
                .DeserializeObject<Token>(jsonContent ?? string.Empty);

            return
                token?.AccessToken ?? string.Empty;
        }

        internal class Token
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
        }
    }
}