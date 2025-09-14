using Dapper;
using System.Data;
using Core.CoreLib.Models.Database.Core;
using Core.CoreLib.Models.Azure.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using Core.CoreLib.Models.DTO.Authentication;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Services.User;
using Core.CoreLib.Models.SAML;
using static Core.CoreLib.Models.Constants.Authorization;
using Core.CoreLib.Services.Database.Base;

namespace Core.CoreLib.Services.Database.Authentication
{
    public partial class AuthenticationService : DBBase, IAuthenticationService
    {
        protected IConfiguration _configuration;
        protected readonly IUserService _userService;

        public AuthenticationService(
            DapperContext dapperContext,
            IConfiguration configuration,
            IUserService userService) : base(dapperContext)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
                var storedRefreshToken =
                    (await ExecuteQuery<RefreshToken>($"SELECT * FROM {DatabaseTables.RefreshToken} WHERE Token = @Token", new { @Token = token }))
                    .FirstOrDefault();

                if (storedRefreshToken == null)
                    throw new Exception("Refresh token doesn't exist");
                
                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                    throw new Exception("Token has expired, user needs to log in again");
                
                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed)
                    throw new Exception("Token has been used");
                
                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked)
                    throw new Exception("Token has been revoked");

                return storedRefreshToken;
        }

        public async Task UseRefreshTokenAsync(string token)
        {
            await ExecuteQuery($"UPDATE {DatabaseTables.RefreshToken} SET IsUsed = 1 WHERE Token = @Token", new { @Token = token });
        }

        public async Task RevokeReshTokenAsync(string token)
        {
            await ExecuteQuery($"UPDATE {DatabaseTables.RefreshToken} SET IsRevoked = 1 WHERE Token = @Token", new { @Token = token });
        }

        public async Task DeleteRefreshTokenAsync(string token)
        {
            await ExecuteQuery($"DELETE From {DatabaseTables.RefreshToken} WHERE Token = @Token", new { @Token = token });
        }

        public async Task<AuthRefreshDTO> GenerateJWTAsync(
            ADUser user, 
            List<Claim> claims)
        {
            if (string.IsNullOrWhiteSpace(_configuration["Jwt:Subject"]))
                throw new Exception("Missing config value for Jwt:Subject");

            if (string.IsNullOrWhiteSpace(_configuration["Jwt:Key"]))
                throw new Exception("Missing config value for Jwt:Key");

            if (string.IsNullOrWhiteSpace(_configuration["JWT:TokenValidityInMinutes"]))
                throw new Exception("Missing config value for Jwt:TokenValidityInMinutes");

            if (string.IsNullOrWhiteSpace(_configuration["Jwt:Issuer"]))
                throw new Exception("Missing config value for Jwt:Issuer");

            if (string.IsNullOrWhiteSpace(_configuration["Jwt:Audience"]))
                throw new Exception("Missing config value for Jwt:Audience");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "missing config value"));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token =
                new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                    signingCredentials: signIn);

            var refreshToken =
                await
                InsertRefreshTokenAsync(
                    new RefreshToken()
                    {
                        JwtId = token.Id,
                        IsUsed = false,
                        UserId = user.GraphUserData.Mail,
                        UserObjectId = user.GraphUserData.Id,
                        AddedDate = DateTime.UtcNow,
                        ExpiryDate = DateTime.UtcNow.AddYears(1),
                        IsRevoked = false,
                        Token = RandomString(25) + Guid.NewGuid()
                    });

            var jwtToken =
                new JwtSecurityTokenHandler().WriteToken(token);

            return
                new AuthRefreshDTO()
                {
                    Token = jwtToken,
                    RefreshToken = refreshToken.Token
                };
        }

        public JwtSecurityToken? DecodeJwt(string token) =>
            new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken; 

        private async Task<RefreshToken> InsertRefreshTokenAsync(RefreshToken refreshToken)
        {
            var query =
                @$"INSERT INTO {DatabaseTables.RefreshToken}
			    (
				    UserId,
                    UserObjectId,
				    Token,
				    JwtId,
				    IsUsed,
				    IsRevoked,
				    AddedDate,
				    ExpiryDate
			    )
			    VALUES 
                (
				    @UserId,
                    @UserObjectId,
				    @Token,
				    @JwtId,
				    @IsUsed,
				    @IsRevoked,
				    @AddedDate,
				    @ExpiryDate
			    )";

            var queryParams = new DynamicParameters();
            queryParams.Add("UserId", refreshToken.UserId, DbType.String);
            queryParams.Add("UserObjectId", refreshToken.UserObjectId, DbType.String);
            queryParams.Add("Token", refreshToken.Token, DbType.String);
            queryParams.Add("JwtId", refreshToken.JwtId, DbType.String);
            queryParams.Add("IsUsed", refreshToken.IsUsed, DbType.Boolean);
            queryParams.Add("IsRevoked", refreshToken.IsRevoked, DbType.Boolean);
            queryParams.Add("AddedDate", refreshToken.AddedDate, DbType.DateTime);
            queryParams.Add("ExpiryDate", refreshToken.ExpiryDate, DbType.DateTime);

            await ExecuteQuery(query, queryParams);
            return refreshToken;
        }

        public async Task AuthenticateUser(ADUser user, string password = "")
        {
            const string internalKey = "@fusion92.com";

            if (user.GraphUserData.AccountEnabled.HasValue &&
                !user.GraphUserData.AccountEnabled.Value)
                throw new Exception("User account disabled");

            try
            {
                // Internal users enter their azure object id (from AD profile) as password.  Unique across users and must exist in AD.
                // Need for this became evident when AD users in Core 2.0 tenant derive from Fusion92 tenant and we could not ensure
                // fidelity across profiles on password...setting password in one tenant seemed to affect other, devops guys were not
                // sure of impact/didn't know the scope of changing one affecting the other.
                if (user.GraphUserData.Mail.EndsWith(internalKey) &&
                    (password != user.GraphUserData.Id))
                    throw new Exception("Incorrect password for internal user.");

                // Non-internal users
                // Will throw exception on failure.  SSO users are authenticated via third party, let them in without password check
                // SSO users will (Auth/Success path) will not require a password (Auth/login sends password).
                else if (!user.GraphUserData.Mail.EndsWith(internalKey) &&
                    !string.IsNullOrWhiteSpace(password))
                    await
                    _userService
                    .ValidateUserPasswordAsync(
                        user,
                        password);
            }
            catch (Exception ex)
            {
                throw new Exception($"Password validation failed. Additional info: {ex.Message}");
            }

            // Permissions
            if (!user.Permissions.Any())
                throw new Exception("No permissions");
        }

        public async Task<GetAuthTokenDTO> VerifyToken(AuthRefreshDTO payload, TokenValidationParameters tokenValidationParameters)
        {
            var principal =
                new JwtSecurityTokenHandler()
                .ValidateToken(payload.Token, tokenValidationParameters, out var validatedToken);

            // Now we need to check if the token has a valid security algorithm
            if (validatedToken is JwtSecurityToken jwtSecurityToken &&
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception("Invalid token");
            
            // Will get the time stamp in unix time
            var utcExpiryDate =
                long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value ?? "0");

            // we convert the expiry date from seconds to the date
            var expDate = UnixTimeStampToDateTime(utcExpiryDate);

            if (expDate > DateTime.UtcNow)
                throw new Exception("Unable to refresh token since the token has not expired");

            var storedRefreshToken =
                await
                GetRefreshTokenAsync(payload.RefreshToken);

            // get the jwt token id
            var jti =
                principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value ?? string.Empty;

            // check the id that the recieved token has against the id saved in the db
            if (storedRefreshToken.JwtId != jti)
                throw new Exception("Token does not match the saved token");

            await UseRefreshTokenAsync(storedRefreshToken.Token);
            var user = await _userService.GetADUserAsync(storedRefreshToken.UserId);

            // TODO: Check OBO session support on refresh
            var tokens = await GenerateJwtToken(user);

            return
                new GetAuthTokenDTO()
                {
                    Token = tokens.Token,
                    RefreshToken = tokens.RefreshToken,
                };
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp) =>
            // Unix timestamp is seconds past epoch
            new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(unixTimeStamp)
            .ToUniversalTime();

        public async Task<GetAuthTokenDTO> GenerateAuthTokenDTO(
            ADUser user,
            SAMLPayload? samlPayload = null)
        {
            var tokens =
                await
                GenerateJwtToken(user, samlPayload);

            return
                new GetAuthTokenDTO()
                {
                    Token = tokens.Token,
                    Success = true,
                    RefreshToken = tokens.RefreshToken
                };
        }

        private async Task<AuthRefreshDTO> GenerateJwtToken(
            ADUser user,
            SAMLPayload? samlPayload = null)
        {
            //create claims details based on the user information
            var claims =
                new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"] ?? "missing config value"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(Claims.UserEmail, user.GraphUserData.Mail),
                    new Claim(Claims.SSOId, user.GraphUserData.EmployeeId ?? General.NONSSO_EmployeeId),
                    new Claim(Claims.BrandId, user.BrandId.ToString())
                };

            if (samlPayload != null)
            {
                claims.Add(new Claim(Claims.OBOSessionSSOId, samlPayload.OBOSSOId ?? string.Empty));
                claims.Add(new Claim(Claims.OBOSession, samlPayload.OBOSession.ToString()));
            }

            claims.AddRange(AddPermissionsToClaims(user));

            return
                await
                GenerateJWTAsync(user, claims);
        }

        private List<Claim> AddPermissionsToClaims(ADUser user)
        {
            var claims = new List<Claim>();

            // Claims should probably be broken out of here to ControllerBase or service
            if (user != null && user.Permissions.Count > 0)
                foreach (var permission in user.Permissions)
                    claims.Add(new Claim(Claims.Permission, permission));
            else
                claims.Add(new Claim(Claims.Permission, Permissions.ReadSite));

            return claims;
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return
                new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}