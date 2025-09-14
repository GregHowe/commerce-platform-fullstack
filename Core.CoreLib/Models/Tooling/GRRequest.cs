using Newtonsoft.Json;

namespace Core.CoreLib.Models.Tooling
{
    public class GRRequest
    {
        public string ResponseToken { get; set; }

        [JsonConstructor]
        public GRRequest(string responseToken)
        {
            ResponseToken = responseToken;
        }
    }
}

