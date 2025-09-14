
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Core.Backend.Models.Web
{
    public class NYLUser
    {
        [JsonPropertyName("accountEnabled")]
        public bool AccountEnabled { get; set; }

        [JsonPropertyName("brandId")]
        public string BrandId { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName => $"{FirstName ?? string.Empty} {LastName ?? string.Empty}";

        [JsonPropertyName("eligiblePersonalizedWebsite")]
        public bool EligiblePersonalizedWebsite { get; set; }

        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; }

        [JsonPropertyName("employeeType")]
        public string EmployeeType { get; set; }

        // Need change to JsonPropertyName, not sure how front is accessing, need to verify before changing
        [JsonProperty("givenName")]
        public string FirstName { get; set; }

        [JsonPropertyName("hasWebsiteAgent")]
        public bool HasWebsiteAgent { get; set; }

        [JsonPropertyName("hasWebsiteRecruiter")]
        public bool HasWebsiteRecruiter { get; set; }

        [JsonProperty("surname")]
        public string LastName { get; set; }

        [JsonPropertyName("mail")]
        public string Mail { get; set; }

        [JsonPropertyName("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonPropertyName("permissions")]
        public List<string> Permissions { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        //[JsonProperty("ssoId")]
        public string SSOId { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("streetAddress")]
        public string StreetAddress { get; set; }

        //[JsonProperty("isOBOSession")]
        public bool IsOBOSession { get; set; }

        //[JsonProperty("oboNYLUser")]
        public NYLUser OBONYLUser { get; set; }

        //[JsonProperty("ddcuserData")]
        public DDCUserData DDCUserData { get; set; }

        [JsonPropertyName("welcomePagePresented")]
        public bool WelcomePagePresented { get; set; }

        [JsonPropertyName("acceptedTerms")]
        public bool AcceptedTerms { get; set; }

        [JsonPropertyName("eagleAdvisor")]
        public bool EagleAdvisor { get; set; }

        [JsonPropertyName("nautilus")]
        public bool Nautilus  { get; set; }

        [JsonPropertyName("registeredRep")]
        public bool RegisteredRep { get; set; }

        //[JsonProperty("approvedDBA")]
        public bool ApprovedDBA { get; set; }

        [JsonPropertyName("longTermCare")]
        public bool LongTermCare { get; set; }

        //[JsonProperty("aarp")]
        public bool AARP { get; set; }
    }
}