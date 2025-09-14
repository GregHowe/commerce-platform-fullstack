using System.Text.Json.Serialization;
using Core.CoreLib.Extensions;

namespace Core.CoreLib.Models.DTO.LeadForm
{
    //https://app.swaggerhub.com/apis/azimnitski/Lead/1.0.5   
    public class SubmitLeadFormDTO
	{
		[JsonPropertyName("firstName")]
		public string? FirstName { get; set; }

		[JsonPropertyName("lastName")]
		public string? LastName { get; set; }

		[JsonPropertyName("address")]
		public string? Address { get; set; }

		[JsonPropertyName("city")]
		public string? City { get; set; }

		[JsonPropertyName("state")]
		public string? State { get; set; }

		[JsonPropertyName("zip")]
		public string? Zip { get; set; }

		[JsonPropertyName("emailAddress")]
		public string? EmailAddress { get; set; }

		[JsonPropertyName("phoneNumber")]
		public string? PhoneNumber { get; set; }

		[JsonPropertyName("bestTimeToCall")]
		public string? BestTimeToCall { get; set; }
        //Morning, Evening, Night, Not Provided

		[JsonPropertyName("interestsAgent")]
		public List<string>? InterestsAgent { get; set; }

        [JsonPropertyName("interestsGO")]
        public List<string>? InterestsGO { get; set; }

        [JsonPropertyName("siteId")]
        public int? SiteId { get; set; }

        [JsonPropertyName("sourceCode")]
		public string? SourceCode { get; set; }

		[JsonPropertyName("marketerNumber")]
		public string? MarketerNumber { get; set; }

		[JsonPropertyName("orgUnitCD")]
		public string? OrgUnitCD { get; set; }

        [JsonPropertyName("birthDate")]
        public string? BirthDate { get; set; }

        [JsonPropertyName("sourceCodeName")]
        public string? SourceCodeName { get; set; }

        [JsonPropertyName("sourceCodeStartDate")]
        public string? SourceCodeStartDate { get; set; }

        [JsonPropertyName("sourceCodeComments")]
        public string? SourceCodeComments { get; set; }

        [JsonPropertyName("sourceCodeEmailAddress")]
        public string? SourceCodeEmailAddress { get; set; }

        [JsonPropertyName("campaignCode")]
        public string? CampaignCode { get; set; }

        [JsonPropertyName("campaignName")]
        public string? CampaignName { get; set; }

        [JsonPropertyName("campaignProgramCode")]
        public string? CampaignProgramCode { get; set; }

        [JsonPropertyName("giftType")]
        public string? GiftType { get; set; }

        [JsonPropertyName("pageUrl")]
        public string? PageUrl { get; set; }

        [JsonPropertyName("linkedinUrl")]
        public string? LinkedinUrl { get; set; }

        [JsonPropertyName("languagePref")]
        public string? LanguagePref { get; set; }
        //CANTONESE, FRENCH, GERMAN, HEBREW, HINDI, JAPANESE, KOREAN, MANDARIN, POLISH, PORTUGUE, RUSSIAN,SPANISH, TAGALOG, URDU, VIETNAMESE

        [JsonPropertyName("leadConcerns")]
        public string? LeadConcerns { get; set; }

        [JsonPropertyName("leadProcessType")]
        public string? LeadProcessType { get; set; }
        //agentPersWeb, goWeb, goWebRec

        [JsonPropertyName("metaData")]
        public List<MetaDataItem>? MetaData { get; set; }

        [JsonPropertyName("referenceNumber")]
		public string? ReferenceNumber { get; set; }

        [JsonPropertyName("dateSubmitted")]
        public string? DateSubmitted { get; set; } = DateTime.UtcNow.ToString().ConvertUTCDateTimeStringToEST();

        [JsonPropertyName("leadSource")]
        public string? LeadSource { get; set; }

        [JsonPropertyName("lastModifiedBy")]
        public string? LastModifiedBy { get; set; }
        
        [JsonPropertyName("contactMethod")]
        public string? ContactMethod { get; set; }

        [JsonPropertyName("customFields")]
        public List<CustomField>? CustomFields { get; set; }

        // Available fields we are not currently exposing
        //clientID
        //leadSourceRequestor
        //serviceCenterLocation
        //bestDayToCall
        //comments
        //specialLeadIn
        //specialLeadReasonCd
    }

    public class MetaDataItem
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    public class CustomField
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
