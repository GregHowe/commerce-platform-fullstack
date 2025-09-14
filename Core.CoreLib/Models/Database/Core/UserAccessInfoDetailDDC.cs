
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.CoreLib.Models.Database.Core
{
    public class UserAccessInfoDetailDDC
    {
        [Column("NYLID")]
        public string NYLId { get; set; }

        [Column("MarketerNo")]
        public string MarketerNo { get; set; }

        [Column("AgentTitleExternalDesc")]
        public string AgentTitleExternalDesc { get; set; }

        [Column("DBATitle")]
        public string DBATitle { get; set; }

        [Column("BusEmailId")]
        public string BusEmailId { get; set; }

        [Column("BusLocAddrCityNm")]
        public string BusLocAddrCityNm { get; set; }

        [Column("BusLocAddrLn1tx")]
        public string BusLocAddrLn1tx { get; set; }

        [Column("BusLocAddrLn2Tx")]
        public string BusLocAddrLn2Tx { get; set; }

        [Column("BusLocAddrLn3Tx")]
        public string BusLocAddrLn3Tx { get; set; }

        [Column("BusLocAddrStateCode")]
        public string BusLocAddrStateCode { get; set; }

        [Column("BusLocAddrZipCode")]
        public string BusLocAddrZipCode { get; set; }

        [Column("BusMailAddrCityName")]
        public string BusMailAddrCityName { get; set; }

        [Column("BusMailAddrLn1txt")]
        public string BusMailAddrLn1txt { get; set; }

        [Column("BusMailAddrLn2txt")]
        public string BusMailAddrLn2txt { get; set; }

        [Column("BusMailAddrLn3txt")]
        public string BusMailAddrLn3txt { get; set; }

        [Column("BusMailAddrStateCode")]
        public string BusMailAddrStateCode { get; set; }

        [Column("BusMailAddrZipCode")]
        public string BusMailAddrZipCode { get; set; }

        [Column("BusPhoneExtnNum")]
        public string BusPhoneExtnNum { get; set; }

        [Column("BusPhoneNum")]
        public string BusPhoneNum { get; set; }

        [Column("CellPhoneNum")]
        public string CellPhoneNum { get; set; }

        [Column("Dba1Nm")]
        public string Dba1Nm { get; set; }

        [Column("Dba1EmailAddrTxt")]
        public string Dba1EmailAddrTxt { get; set; }

        [Column("Dba1LogoTxt")]
        public string Dba1LogoTxt { get; set; }

        [Column("Dba1websiteURLTxt")]
        public string Dba1websiteURLTxt { get; set; }

        [Column("Dba1ApprovalDt")]
        public string Dba1ApprovalDt { get; set; }

        [Column("Dba2Nm")]
        public string Dba2Nm { get; set; }

        [Column("Dba2EmailAddrTxt")]
        public string Dba2EmailAddrTxt { get; set; }

        [Column("Dba2LogoTxt")]
        public string Dba2LogoTxt { get; set; }

        [Column("Dba2websiteURLTxt")]
        public string Dba2websiteURLTxt { get; set; }

        [Column("Dba2ApprovalDt")]
        public string Dba2ApprovalDt { get; set; }

        [Column("Dba3Nm")]
        public string Dba3Nm { get; set; }

        [Column("Dba3EmailAddrTxt")]
        public string Dba3EmailAddrTxt { get; set; }

        [Column("Dba3LogoTxt")]
        public string Dba3LogoTxt { get; set; }

        [Column("Dba3websiteURLTxt")]
        public string Dba3websiteURLTxt { get; set; }

        [Column("Dba3ApprovalDt")]
        public string Dba3ApprovalDt { get; set; }

        [Column("Dba4Nm")]
        public string Dba4Nm { get; set; }

        [Column("Dba4EmailAddrTxt")]
        public string Dba4EmailAddrTxt { get; set; }

        [Column("Dba4LogoTxt")]
        public string Dba4LogoTxt { get; set; }

        [Column("Dba4websiteURLTxt")]
        public string Dba4websiteURLTxt { get; set; }

        [Column("Dba4ApprovalDt")]
        public string Dba4ApprovalDt { get; set; }

        [Column("Dba5Nm")]
        public string Dba5Nm { get; set; }

        [Column("Dba5EmailAddrTxt")]
        public string Dba5EmailAddrTxt { get; set; }

        [Column("Dba5LogoTxt")]
        public string Dba5LogoTxt { get; set; }

        [Column("Dba5websiteURLTxt")]
        public string Dba5websiteURLTxt { get; set; }

        [Column("Dba5ApprovalDt")]
        public string Dba5ApprovalDt { get; set; }

        [Column("EagleInd")]
        public string EagleInd { get; set; }

        [Column("FacebookUrlTxt")]
        public string FacebookUrlTxt { get; set; }

        [Column("FaxNumber")]
        public string FaxNumber { get; set; }

        [Column("LnkdinUrlTxt")]
        public string LnkdinUrlTxt { get; set; }

        [Column("MarketerTitleTpDesc")]
        public string MarketerTitleTpDesc { get; set; }

        [Column("MDRTLastYear")]
        public string MDRTLastYear { get; set; }

        [Column("MktrLglFirstNm")]
        public string MktrLglFirstNm { get; set; }

        [Column("MktrLglLastName")]
        public string MktrLglLastName { get; set; }

        [Column("MktrLglMiddleNm")]
        public string MktrLglMiddleNm { get; set; }

        [Column("MktrLglSfxCode")]
        public string MktrLglSfxCode { get; set; }

        [Column("MktrPrefFirstNm")]
        public string MktrPrefFirstNm { get; set; }

        [Column("NautilusInd")]
        public string NautilusInd { get; set; }

        [Column("PhotoURLProfile")]
        public string PhotoURLProfile { get; set; }

        [Column("RegRepInd")]
        public string RegRepInd { get; set; }

        [Column("TwitterUrlTxt")]
        public string TwitterUrlTxt { get; set; }

        [Column("OrgUnitDesc")]
        public string OrgUnitDesc { get; set; }

        [Column("OrgUnitCode")]
        public string OrgUnitCode { get; set; }

        public List<UserLicenseDetailDDC>? UserLicensesDetailDDC { get; set; } = new List<UserLicenseDetailDDC>();
    }
}