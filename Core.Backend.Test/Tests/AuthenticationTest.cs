using Core.CoreLib.Models.Constants;
using Core.CoreLib.Services.Azure.ActiveDirectory;
using Core.CoreLib.Services.Database.Authentication;
using Core.CoreLib.Services.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Backend.Test.Tests
{
    public class AuthenticationTest : TestBase
    {
        [Fact]
        private async Task GenerateTokenTest()
        {
            var config =
                CreateConfig();

            var authService =
                new AuthenticationService(
                    new CoreLib.Services.Database.DapperContext(config),
                    config,
                    new CoreLib.Services.User.UserService(
                        new ActiveDirectoryService(
                            new PasswordService(),
                            config),
                        config));

            var user =
                ScaffoldUser("Test", "Token", "t.token@fusion92.com");

            user.GraphUserData.Id = Guid.NewGuid().ToString();

            // Generate
            var tokens =
                await
                authService
                .GenerateJWTAsync(
                    user,
                    new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"] ?? "missing config value"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(Claims.UserEmail, user.GraphUserData.Mail),
                        new Claim(Claims.BrandId, user.BrandId.ToString())
                    });

            Assert.NotNull(tokens);
            Assert.True(!string.IsNullOrEmpty(tokens.Token));
            Assert.True(!string.IsNullOrEmpty(tokens.RefreshToken));

            // GET Refresh
            var storedRefreshToken =
                await
                authService
                .GetRefreshTokenAsync(tokens.RefreshToken);

            Assert.NotNull(storedRefreshToken);
            Assert.NotNull(storedRefreshToken.JwtId);
            
            var tokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = config["Jwt:Audience"],
                    ValidIssuer = config["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                };

            var principal =
                new JwtSecurityTokenHandler()
                .ValidateToken(
                    tokens.Token, 
                    tokenValidationParameters, 
                    out var validatedToken);

            // get the jwt token id
            var jti =
                principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value ?? null;

            Assert.NotNull(jti);
            Assert.True(jti == storedRefreshToken.JwtId);

            // Set revoked
            await
            authService
            .RevokeReshTokenAsync(storedRefreshToken.Token);

            var exRevoked =
                await Assert.ThrowsAsync<Exception>(async () => await authService.GetRefreshTokenAsync(storedRefreshToken.Token));
            Assert.True(exRevoked.Message == "Token has been revoked");

            // Set used
            await 
            authService
            .UseRefreshTokenAsync(storedRefreshToken.Token);

            var exUsed =
                await Assert.ThrowsAsync<Exception>(async () => await authService.GetRefreshTokenAsync(storedRefreshToken.Token));
            Assert.True(exUsed.Message == "Token has been used");

            // Delete
            await
            authService
            .DeleteRefreshTokenAsync(storedRefreshToken.Token);

            var exRemoved =
                await Assert.ThrowsAsync<Exception>(async () => await authService.GetRefreshTokenAsync(storedRefreshToken.Token));
            Assert.True(exRemoved.Message == "Refresh token doesn't exist");
        }
    }
}
