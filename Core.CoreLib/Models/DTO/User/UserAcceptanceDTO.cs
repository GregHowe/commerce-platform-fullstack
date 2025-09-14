
namespace Core.CoreLib.Models.DTO.User
{
    public class UserAcceptanceDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MarketerId { get; set; }
        public bool WelcomePagePresented { get; set; }
        public bool AcceptedTerms { get; set; }
    }
}