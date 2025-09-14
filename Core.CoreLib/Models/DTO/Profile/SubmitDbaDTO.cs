using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Core.CoreLib.Models.DTO.Profile
{
	public class SubmitDbaDTO : ValidationAttribute
    {
        [JsonProperty("smruApprovalDate")]
        public DateTime SmruApprovalDate { get; set; }

        [JsonProperty("dba")]
        public Dba Dba { get; set; }

        [JsonProperty("webSite")]
        public WebSite WebSite { get; set; }

        [JsonProperty("marketers")]
        public List<Marketer> Marketers { get; set; }
    }

    public class Marketer
    {
        [JsonProperty("marketerId")]
        public string MarketerId { get; set; }

        [JsonProperty("optInInd")]
        public bool OptInInd { get; set; }

        [Required(ErrorMessage = "Marketer title is required")]
        [MaxLength(100, ErrorMessage = "The title max characters can't be greater then 100.")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Incorrect email format.")]
        [JsonProperty("prefEmailAddress")]
        public string PrefEmailAddress { get; set; }

        [JsonProperty("isPrimary")]
        public bool IsPrimary { get; set; }

        [MaxLength(15, ErrorMessage = "The phone number max characters can't be greater then 15.")]
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class Dba
    {
        [Required(ErrorMessage = "Name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Display name is required")]
        [MaxLength(300, ErrorMessage = "The display name max characters can't be greater then 300.")]
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("logoCreatedDate")]
        public DateTime LogoCreatedDate { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }
    }

    public class Image
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }

    public class WebSite
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("urlGuid")]
        public string UrlGuid { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }
    }


}

