
using Core.CoreLib.Models.SAML;

namespace Core.CoreLib.Services.Security
{
    public interface ITokenExploder
    {
        SAMLPayload ExplodeToken(string token);
    }
}