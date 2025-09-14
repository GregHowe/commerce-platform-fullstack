
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.CoreLib.Models.Database.Core
{
    public class UserLicenseDetailDDC
    {
        [Column("NYLID")]
        public string NYLID { get; set; }

        [Column("MarketerNo")]
        public string MarketerNo { get; set; }

        [Column("MarketerStatusTpCode")]
        public string MarketerStatusTpCode { get; set; }

        [Column("MarketerStatusTpDesc")]
        public string MarketerStatusTpDesc { get; set; }

        [Column("LicenseTpCode")]
        public string LicenseTpCode { get; set; }

        [Column("LicenseTpDescription")]
        public string LicenseTpDescription { get; set; }

        [Column("LicenseExpiryDt")]
        public string LicenseExpiryDt { get; set; }

        [Column("LicenseIdNumber")]
        public string LicenseIdNumber { get; set; }

        [Column("LicenseIssueDt")]
        public string LicenseIssueDt { get; set; }

        [Column("LicenseLobCode")]
        public string LicenseLobCode { get; set; }

        [Column("StateCountyCode")]
        public string StateCountyCode { get; set; }

        [Column("BusLicenseTpCode")]
        public string BusLicenseTpCode { get; set; }

        [Column("BusEntityCode")]
        public string BusEntityCode { get; set; }

        [Column("AgentData")]
        public string AgentData { get; set; }

        [Column("EagleData")]
        public string EagleData { get; set; }
    }
}