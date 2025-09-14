
using System.ComponentModel.DataAnnotations;

namespace Core.CoreLib.Models.Database.Core
{
    public class UserMigrationInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MarketerNo { get; set; }

        [Required]
        public string NYLId { get; set; }

        public string AgentSearchEmail { get; set; }
        public string AgentSearchMarketerId { get; set; }
        public string CurrentAgent { get; set; }
        public string AgentTitleExternalDesc { get; set; }
        public string AltPhone1 { get; set; }
        public string AltPhone2 { get; set; }
        public string BusinessEmail { get; set; }

        public string BusinessLocAddrCityName { get; set; }
        public string BusinessLocAddrLn1 { get; set; }
        public string BusinessLocAddrLn2 { get; set; }
        public string BusinessLocAddrLn3 { get; set; }
        public string BusinessLocAddrState { get; set; }
        public string BusinessLocAddrZipCode { get; set; }
        public string BusinessMailAddrCityName { get; set; }
        public string BusinessMailAddrLn1 { get; set; }
        public string BusinessMailAddrLn2 { get; set; }
        public string BusinessMailAddrLn3 { get; set; }
        public string BusinessMailAddrState { get; set; }
        public string BusinessMailAddrZipCode { get; set; }
        public string BusinessPhoneExtNum { get; set; }
        public string BusinessPhoneNum { get; set; }
        public string CalendlyId { get; set; }
        public string CellPhone { get; set; }
        public string DBATitle { get; set; }
        public string DisplayName { get; set; }
        public string EagleIndicator { get; set; }
        public string FacebookUrl { get; set; }
        public string FaxNumber { get; set; }
        public string LinkedInUrl { get; set; }
        public string MarketerLegalFirstName { get; set; }
        public string MarketerLegalLastName { get; set; }
        public string MarketerLegalMiddleName { get; set; }
        public string MarketerLegalSuffix { get; set; }
        public string MarketerPreferredFirstName { get; set; }
        public string NautilusIndicator { get; set; }
        public string RegisteredRepIndicator { get; set; }
        public string TwitterUrl { get; set; }
        public string TemplateType { get; set; }
        public string DBAIndicator { get; set; }
        public string PrebuiltPages { get; set; }
        public string Calculators { get; set; }
    }
}