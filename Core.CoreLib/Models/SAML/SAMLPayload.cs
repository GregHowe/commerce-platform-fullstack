
namespace Core.CoreLib.Models.SAML
{
    public class SAMLPayload
    {
        public string SSOId { get; set; }

        public string EmailAddress { get; set; }

        public bool OBOSession { get; set; }

        public string OBOSSOId { get; set; }
    }
}