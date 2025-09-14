
using Core.CoreLib.Models.Constants;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace Core.CoreLib.Models.Azure.ActiveDirectory
{
    public class ADUser
    {
        [JsonProperty("graphUserData")]
        public User GraphUserData { get; set; }

        [JsonProperty("customAttributes")]
        public CustomAttribute CustomAttributes { get; set; }

        [JsonProperty("adGroupIds")]
        public List<string> ADGroupIds { get; set; }

        [JsonProperty("password")]
        public string Password { private get; set; }

        public List<string> Permissions
        {
            get
            {
                // Permissions are set within the app, not passed from ingest, based on Role (EmployeeType)
                return
                    GraphUserData != null && !string.IsNullOrWhiteSpace(GraphUserData.EmployeeType) ?
                        Authorization.GetPermissionsFromEmployeeType(GraphUserData.EmployeeType, EligibleForPersonalizedWebsite) :
                        new List<string>();
            }

            private set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public string ExtensionsId
        {
            get
            {
                return
                    GraphUserData?.Id ??
                    string.Empty;
            }

            private set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public string SSOId 
        {
            get
            {
                return
                    CustomAttributes?.SSOId ??
                    GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.SSOId).FirstOrDefault().Value?.ToString() ??
                    string.Empty;
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public string BrandId 
        {
            get
            {
                return
                    CustomAttributes?.BrandId ??
                    GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.BrandId).FirstOrDefault().Value?.ToString() ??
                    string.Empty;
            }

            set { } 
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool HasPersonalizedWebsiteRecruiter
        {
            get
            {
                return
                    CustomAttributes?.HasPersonalizedWebsiteRecruiter ??
                    bool.Parse(GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.HasPersonalizedWebsiteRecruiter)?.FirstOrDefault().Value?.ToString() ?? "false");
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool HasPersonalizedWebsiteAgent
        {
            get
            {
                return
                    CustomAttributes?.HasPersonalizedWebsiteAgent ??
                    bool.Parse(GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.HasPersonalizedWebsiteAgent)?.FirstOrDefault().Value?.ToString() ?? "false");
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool EligibleForPersonalizedWebsite
        {
            get
            {
                return
                    CustomAttributes?.EligibleForPersonalizedWebsite ??
                    bool.Parse(GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.EligibleForPersonalizedWebsite)?.FirstOrDefault().Value?.ToString() ?? "false");
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool EagleAdvisor
        {
            get
            {
                return
                    CustomAttributes?.EagleAdvisor ??
                    bool.Parse(GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.EagleAdvisor)?.FirstOrDefault().Value?.ToString() ?? "false");
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Nautilus
        {
            get
            {
                return
                    CustomAttributes?.Nautilus ??
                    bool.Parse(GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.Nautilus)?.FirstOrDefault().Value?.ToString() ?? "false");
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool RegisteredRep
        {
            get
            {
                return
                    CustomAttributes?.RegisteredRep ??
                    bool.Parse(GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.RegisteredRep)?.FirstOrDefault().Value?.ToString() ?? "false");
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool ApprovedDBA
        {
            get
            {
                return
                    CustomAttributes?.ApprovedDBA ??
                    bool.Parse(GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.ApprovedDBA)?.FirstOrDefault().Value?.ToString() ?? "false");
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool LongTermCare
        {
            get
            {
                return
                    CustomAttributes?.LongTermCare ??
                    bool.Parse(GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.LongTermCare)?.FirstOrDefault().Value?.ToString() ?? "false");
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool AARP
        {
            get
            {
                return
                    CustomAttributes?.AARP ??
                    bool.Parse(GraphUserData?.Extensions?.CurrentPage?.FirstOrDefault()?.AdditionalData?.Where(w => w.Key == AzureADCustomAttributes.AARP)?.FirstOrDefault().Value?.ToString() ?? "false");
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool AllowBusinessSolutions
        {
            get
            {
                // Need to find out where this comes from
                return false;
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool IsAgent
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(GraphUserData?.EmployeeType ?? string.Empty) &&
                    GraphUserData?.EmployeeType.ToLower() == "agent";
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool IsGeneralOffice
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(GraphUserData?.EmployeeType ?? string.Empty) &&
                    GraphUserData?.EmployeeType.ToLower() == "go";
            }

            set { }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool IsHomeOffice
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(GraphUserData?.EmployeeType ?? string.Empty) &&
                    GraphUserData?.EmployeeType.ToLower() == "ho";
            }

            set { }
        }
    }

    public class CustomAttribute
    {
        [JsonProperty("brandId")]
        public string BrandId { get; set; }

        [JsonProperty("ssoId")]
        public string SSOId { get; set; }

        [JsonProperty("hasPersonalizedWebsiteRecruite")]
        public bool HasPersonalizedWebsiteRecruiter { get; set; }

        [JsonProperty("hasPersonalizedWebsiteAgent")]
        public bool HasPersonalizedWebsiteAgent { get; set; }

        [JsonProperty("eligibleForPersonalizedWebsite")]
        public bool EligibleForPersonalizedWebsite { get; set; }

        [JsonProperty("EagleAdvisor")]
        public bool EagleAdvisor { get; set; }

        [JsonProperty("Nautilus")]
        public bool Nautilus { get; set; }

        [JsonProperty("RegisteredRep")]
        public bool RegisteredRep { get; set; }

        [JsonProperty("ApprovedDBA")]
        public bool ApprovedDBA { get; set; }

        [JsonProperty("LongTermCare")]
        public bool LongTermCare { get; set; }

        [JsonProperty("AARP")]
        public bool AARP { get; set; }
    }
}