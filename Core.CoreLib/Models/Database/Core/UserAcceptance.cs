
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.CoreLib.Models.Database.Core
{
    public class UserAcceptance
    {
        [Column("UserAcceptanceId")]
        public int UserAcceptanceId { get; set; }

        [Column("WelcomePagePresented")]
        public bool WelcomePagePresented { get; set; }

        [Column("AcceptedTerms")]
        public bool AcceptedTerms { get; set; }

        [Column("UserId")]
        public string UserId { get; set; }

        [Column("UserObjectId")]
        public string UserObjectId { get; set; }

        [Column("Created")]
        public DateTime Created { get; set; }
    }
}