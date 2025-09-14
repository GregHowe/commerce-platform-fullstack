using Core.CoreLib.Models.Azure.ActiveDirectory;
using Core.CoreLib.Models.Database.Core;
using Core.CoreLib.Models.DTO.Authentication;
using Core.CoreLib.Models.SAML;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.CoreLib.Services.Database.Authentication
{
    public interface IAuthenticationService
    {
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        Task UseRefreshTokenAsync(string token);
        Task RevokeReshTokenAsync(string token);
        Task DeleteRefreshTokenAsync(string token);
        Task<AuthRefreshDTO> GenerateJWTAsync(ADUser user, List<Claim> claims);
        JwtSecurityToken? DecodeJwt(string token);
        Task AuthenticateUser(ADUser user, string password = "");
        Task<GetAuthTokenDTO> GenerateAuthTokenDTO(ADUser user, SAMLPayload? samlPayload = null);
        Task<GetAuthTokenDTO> VerifyToken(AuthRefreshDTO payload, TokenValidationParameters tokenValidationParameters);
    }
}