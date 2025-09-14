
using Core.CoreLib.Models.Database.Core;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Backend.Models.Web
{
    public class DDCUserData
    {
        [Column("marketerNo")]
        public string MarketerNo { get; set; }

        [JsonPropertyName("agentTitleExternalDesc")]
        public string AgentTitleExternalDesc { get; set; }

        //[JsonProperty("dbaTitle")]
        public string DBATitle { get; set; }
    
        [JsonPropertyName("busEmailId")]
        public string BusEmailId { get; set; }

        [JsonPropertyName("busLocAddrCityNm")]
        public string BusLocAddrCityNm { get; set; }

        [JsonPropertyName("busLocAddrLn1tx")]
        public string BusLocAddrLn1tx { get; set; }

        [JsonProperty("businessLocAddrLn2Tx")]
        public string BusLocAddrLn2Tx { get; set; }

        [JsonPropertyName("busLocAddrLn3Tx")]
        public string BusLocAddrLn3Tx { get; set; }

        [JsonPropertyName("busLocAddrStateCode")]
        public string BusLocAddrStateCode { get; set; }

        [JsonPropertyName("busLocAddrZipCode")]
        public string BusLocAddrZipCode { get; set; }

        [JsonPropertyName("busMailAddrCityName")]
        public string BusMailAddrCityName { get; set; }

        [JsonPropertyName("busMailAddrLn1txt")]
        public string BusMailAddrLn1txt { get; set; }

        [JsonPropertyName("busMailAddrLn2txt")]
        public string BusMailAddrLn2txt { get; set; }

        [JsonPropertyName("busMailAddrLn3txt")]
        public string BusMailAddrLn3txt { get; set; }

        [JsonPropertyName("busMailAddrStateCode")]
        public string BusMailAddrStateCode { get; set; }

        [JsonPropertyName("busMailAddrZipCode")]
        public string BusMailAddrZipCode { get; set; }

        [JsonPropertyName("busPhoneExtnNum")]
        public string BusPhoneExtnNum { get; set; }

        [JsonPropertyName("busPhoneNum")]
        public string BusPhoneNum { get; set; }

        [JsonPropertyName("cellPhoneNum")]
        public string CellPhoneNum { get; set; }

        [Column("dba1Nm")]
        public string Dba1Nm { get; set; }

        [Column("dba1EmailAddrTxt")]
        public string Dba1EmailAddrTxt { get; set; }

        [Column("dba1LogoTxt")]
        public string Dba1LogoTxt { get; set; }

        [Column("dba1websiteURLTxt")]
        public string Dba1websiteURLTxt { get; set; }

        [Column("dba1ApprovalDt")]
        public string Dba1ApprovalDt { get; set; }

        [Column("dba2Nm")]
        public string Dba2Nm { get; set; }

        [Column("dba2EmailAddrTxt")]
        public string Dba2EmailAddrTxt { get; set; }

        [Column("dba2LogoTxt")]
        public string Dba2LogoTxt { get; set; }

        [Column("dba2websiteURLTxt")]
        public string Dba2websiteURLTxt { get; set; }

        [Column("dba2ApprovalDt")]
        public string Dba2ApprovalDt { get; set; }

        [Column("dba3Nm")]
        public string Dba3Nm { get; set; }

        [Column("dba3EmailAddrTxt")]
        public string Dba3EmailAddrTxt { get; set; }

        [Column("dba3LogoTxt")]
        public string Dba3LogoTxt { get; set; }

        [Column("dba3websiteURLTxt")]
        public string Dba3websiteURLTxt { get; set; }

        [Column("dba3ApprovalDt")]
        public string Dba3ApprovalDt { get; set; }

        [Column("dba4Nm")]
        public string Dba4Nm { get; set; }

        [Column("dba4EmailAddrTxt")]
        public string Dba4EmailAddrTxt { get; set; }

        [Column("dba4LogoTxt")]
        public string Dba4LogoTxt { get; set; }

        [Column("dba4websiteURLTxt")]
        public string Dba4websiteURLTxt { get; set; }

        [Column("dba4ApprovalDt")]
        public string Dba4ApprovalDt { get; set; }

        [Column("dba5Nm")]
        public string Dba5Nm { get; set; }

        [Column("dba5EmailAddrTxt")]
        public string Dba5EmailAddrTxt { get; set; }

        [Column("dba5LogoTxt")]
        public string Dba5LogoTxt { get; set; }

        [Column("dba5websiteURLTxt")]
        public string Dba5websiteURLTxt { get; set; }

        [Column("dba5ApprovalDt")]
        public string Dba5ApprovalDt { get; set; }

        [JsonPropertyName("eagleInd")]
        public string EagleInd { get; set; }

        [JsonPropertyName("facebookUrlTxt")]
        public string FacebookUrlTxt { get; set; }

        [JsonPropertyName("faxNumber")]
        public string FaxNumber { get; set; }

        [JsonPropertyName("lnkdinUrlTxt")]
        public string LnkdinUrlTxt { get; set; }

        [JsonPropertyName("marketerTitleTpDesc")]
        public string MarketerTitleTpDesc { get; set; }

        [JsonPropertyName("mdrtLastYear")]
        public string MDRTLastYear { get; set; }

        [JsonPropertyName("mktrLglFirstNm")]
        public string MktrLglFirstNm { get; set; }

        [JsonPropertyName("mktrLglLastName")]
        public string MktrLglLastName { get; set; }

        [JsonPropertyName("mktrLglMiddleNm")]
        public string MktrLglMiddleNm { get; set; }

        [JsonPropertyName("mktrLglSfxCode")]
        public string MktrLglSfxCode { get; set; }

        [JsonPropertyName("mktrPrefFirstNm")]
        public string MktrPrefFirstNm { get; set; }

        [JsonPropertyName("nautilusInd")]
        public string NautilusInd { get; set; }

        //[JsonPropertyName("photoURLProfile")]
        public string PhotoURLProfile { get; set; }

        [JsonPropertyName("regRepInd")]
        public string RegRepInd { get; set; }

        [JsonPropertyName("twitterUrlTxt")]
        public string TwitterUrlTxt { get; set; }

        [JsonPropertyName("orgUnitDesc")]
        public string OrgUnitDesc { get; set; }

        [JsonPropertyName("orgUnitCode")]
        public string OrgUnitCode { get; set; }

        //[JsonProperty("ddcLicenseData")]
        public List<DDCLicenseData> DDCLicenseData { get; set; } = new List<DDCLicenseData>();
    }
}
