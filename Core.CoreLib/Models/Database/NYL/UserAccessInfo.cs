
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.CoreLib.Models.Database.NYL
{
    public class UserAccessInfo
    {
        [Column("NYLID")]
        public string NYLId { get; set; }

        [Column("PrefFName")]
        public string FirstName { get; set; }

        [Column("PrefMName")]
        public string MiddleName { get; set; }

        [Column("PrefLName")]
        public string LastName { get; set; }

        [Column("PrefAddr1")]
        public string Address { get; set; }

        [Column("PrefAddr2")]
        public string Address2 { get; set; }

        [Column("PrefAddr3")]
        public string Address3 { get; set; }

        [Column("City")]
        public string City { get; set; }

        [Column("State")]
        public string State { get; set; }

        [Column("Zip")]
        public string PostalCode { get; set; }

        [Column("AreaCode")]
        public string AreaCode { get; set; }

        [Column("Phone")]
        public string PhoneNumber { get; set; }

        [Column("Email")]
        public string Eamail { get; set; }

        [Column("LocationName")]
        public string LocationName { get; set; }

        [Column("Fusion92Role")]
        public string Fusion92Role { get; set; }

        [Column("Role")]
        public string Role { get; set; }

        [Column("UserActiveFlag")]
        public string UserActiveFlag { get; set; }

        [Column("Country")]
        public string Country { get; set; }

        [Column("BrandId")]
        public int? BrandId { get; set; }

        [Column("IsGO")]
        public bool? IsGO { get; set; }

        [Column("IsHomeOffice")]
        public bool? IsHO { get; set; }

        [Column("IsAgent")]
        public bool? IsAgent { get; set; }

        [Column("IsOnBehalf")]
        public bool? IsOBO { get; set; }

        [Column("HasPersonalizedWebsite_Agent")]
        public bool? HasPersonalizedWebsiteAgent { get; set; }

        [Column("HasPersonalizedWebsite_Recruiter")]
        public bool? HasPersonalizedWebsiteRecruiter { get; set; }

        [Column("EligibleForPersonalizedWebsite")]
        public bool? EligibleForPersonalizedWebsite { get; set; }

        [Column("MarketerNo")]
        public string MarketerNo { get; set; }

        [Column("EagleInd")]
        public bool? EagleInd { get; set; }

        [Column("NautilusInd")]
        public bool? NautilusInd { get; set; }

        [Column("RegRepInd")]
        public bool? RegisteredRep { get; set; }

        [Column("DBAInd")]
        public bool? DBAInd { get; set; }

        [Column("LTCInd")]
        public bool? LTCInd { get; set; }

        [Column("AARPInd")]
        public bool? AARPInd { get; set; }
    }
}