
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.CoreLib.Models.DTO.User
{
    public class UserMigrationInfoDTO
    {
        [JsonIgnore]
        [Column("Id")]
        public int Id { get; set; }

        [JsonPropertyName("marketerNo")]
        public string MarketerNo { get; set; }

        [JsonPropertyName("nylId")]
        public string NYLId { get; set; }

        [JsonPropertyName("agentSearchEmail")]
        public string? AgentSearchEmail { get; set; }

        [JsonPropertyName("agentSearchmarketerId")]
        public string? AgentSearchMarketerId { get; set; }

        // THis is supposed to be some kind of object, need more info from Jake => Define the object no raw strings
        //[JsonPropertyName("currentAgent")]
        //public string CurrentAgent { get; set; }

        [JsonPropertyName("agentTitleExternalDesc")]
        public string? AgentTitleExternalDesc { get; set; }

        [JsonPropertyName("alt_phone_1")]
        public string? AltPhone1 { get; set; }

        [JsonPropertyName("alt_phone_2")]
        public string? AltPhone2 { get; set; }

        [JsonPropertyName("busEmailId")]
        public string? BusinessEmail { get; set; }

        [JsonPropertyName("busLocAddrCityNm")]
        public string? BusinessLocAddrCityName { get; set; }

        [JsonPropertyName("busLocAddrLn1tx")]
        public string? BusinessLocAddrLn1 { get; set; }

        [JsonPropertyName("busLocAddrLn2Tx")]
        public string? BusinessLocAddrLn2 { get; set; }

        [JsonPropertyName("busLocAddrLn3Tx")]
        public string? BusinessLocAddrLn3 { get; set; }

        [JsonPropertyName("busLocAddrStateCode")]
        public string? BusinessLocAddrState { get; set; }

        [JsonPropertyName("busLocAddrZipCode")]
        public string? BusinessLocAddrZipCode { get; set; }

        [JsonPropertyName("busMailAddrCityName")]
        public string? BusinessMailAddrCityName { get; set; }

        [JsonPropertyName("busMailAddrLn1txt")]
        public string? BusinessMailAddrLn1 { get; set; }

        [JsonPropertyName("busMailAddrLn2txt")]
        public string? BusinessMailAddrLn2 { get; set; }

        [JsonPropertyName("busMailAddrLn3txt")]
        public string? BusinessMailAddrLn3 { get; set; }

        [JsonPropertyName("busMailAddrStateCode")]
        public string? BusinessMailAddrState { get; set; }

        [JsonPropertyName("busMailAddrZipCode")]
        public string? BusinessMailAddrZipCode { get; set; }

        [JsonPropertyName("busPhoneExtnNum")]
        public string? BusinessPhoneExtNum { get; set; }

        [JsonPropertyName("busPhoneNum")]
        public string? BusinessPhoneNum { get; set; }

        [JsonPropertyName("calendlyId")]
        public string? CalendlyId { get; set; }

        [JsonPropertyName("cellPhoneNum")]
        public string? CellPhone { get; set; }

        [JsonPropertyName("dbaTitle")]
        public string? DBATitle { get; set; }

        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("eagleInd")]
        public string? EagleIndicator { get; set; }

        [JsonPropertyName("facebookUrlTxt")]
        public string? FacebookUrl { get; set; }

        [JsonPropertyName("faxNumber")]
        public string? FaxNumber { get; set; }

        [JsonPropertyName("lnkdinUrlTxt")]
        public string? LinkedInUrl { get; set; }

        [JsonPropertyName("mktrLglFirstNm")]
        public string? MarketerLegalFirstName { get; set; }

        [JsonPropertyName("mktrLglLastName")]
        public string? MarketerLegalLastName { get; set; }

        [JsonPropertyName("mktrLglMiddleNm")]
        public string? MarketerLegalMiddleName { get; set; }

        [JsonPropertyName("mktrLglSfxCode")]
        public string? MarketerLegalSuffix { get; set; }

        [JsonPropertyName("mktrPrefFirstNm")]
        public string? MarketerPreferredFirstName { get; set; }

        [JsonPropertyName("nautilusInd")]
        public string? NautilusIndicator { get; set; }

        [JsonPropertyName("regRepInd")]
        public string? RegisteredRepIndicator { get; set; }

        [JsonPropertyName("twitterUrlTxt")]
        public string? TwitterUrl { get; set; }

        [JsonPropertyName("templateType")]
        public string? TemplateType { get; set; }

        [JsonPropertyName("isDBA")]
        public string? DBAIndicator { get; set; }

        [JsonPropertyName("prebuiltPages")]
        public string? PrebuiltPages { get; set; }

        [JsonPropertyName("calculators")]
        public string? Calculators { get; set; }

        [JsonIgnore]
        [Column("CreatedDt")]
        public DateTime CreatedDt { get; set; }

        [JsonIgnore]
        [Column("RetiredDt")]
        public DateTime? RetiredDt { get; set; }
    }
}