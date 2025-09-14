namespace Core.CoreLib.Models.Database.Core
{
    public class LeadForm
    {
        /// <summary>
        /// Unique control reference number
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Id of the site submitting the lead form
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Was the form submitted or not
        /// </summary>
        public bool IsSubmitted { get; set; }

        /// <summary>
        /// Date when the form was submitted
        /// </summary>
        public DateTime DateSubmitted { get; set; }
    }
}
