
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Core.CoreLib.Models.DTO.Authentication;
using Core.CoreLib.Services.Database.Authentication;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Services.User;
using Core.CoreLib.Services.Security;
using Core.Backend.Extensions;
using Core.CoreLib.Models.SAML;
using Core.CoreLib.Extensions;

namespace CoreBackend.Controllers;

/*
lots of things to implement here
https://www.c-sharpcorner.com/article/jwt-authentication-with-refresh-tokens-in-net-6-0/
*/



[Route("api/auth")]
[Route("auth")]
[ApiController]
public sealed class AuthController : ControllerBaseTokenized
{
    private readonly IConfiguration _configuration;
    private readonly IAuthenticationService _authRepo;
    private readonly IUserService _userService;
    private readonly ITokenExploder _nylTokenExploder;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly ILogger<Controller> _logger;

    public AuthController(
        IConfiguration config,
        IAuthenticationService authRepo,
        IUserService userService,
        ITokenExploder nylTokenExploder,
        TokenValidationParameters tokenValidationParameters,
        ILogger<Controller> logger)
    {
        _configuration = config;
        _authRepo = authRepo;
        _userService = userService;
        _nylTokenExploder = nylTokenExploder;
        _tokenValidationParameters = tokenValidationParameters;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> AuthLogin(AuthLoginDTO payload)
    {
        // NON-SSO path, username/password
        try
        {
            if (payload != null &&
                !string.IsNullOrWhiteSpace(payload.Username) &&
                !string.IsNullOrWhiteSpace(payload.Password))
            {
                var user =
                    await
                    _userService
                    .GetADUserAsync(
                        payload.Username);

                if (user == null)
                    return BadRequest("User not found in system");

                try
                {
                    await
                    _authRepo
                    .AuthenticateUser(user, payload.Password);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return
                    Ok(
                        await
                        _authRepo
                        .GenerateAuthTokenDTO(user));
            }
            else
            {
                return BadRequest("Payload empty or missing data");
            }
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("SPOOFssologinfromSAML")]
    [AllowAnonymous]
    public async Task<IActionResult> SSOSpoof(bool asOBO)
    {
        try
        {
            // Explode token, get userID
            var samlPayload =
                _nylTokenExploder
                .ExplodeToken(
                    asOBO ?
                        NYLSSOTokenExploderService.EncodedNONOBOSampleToken :
                        NYLSSOTokenExploderService.EncodedSampleToken);

            // Look up user in our AD from this tenant userId (ssoId) and get system userId (AD field Email)
            var user =
                await
                _userService
                .GetSSOADUserAsync(samlPayload?.SSOId ?? string.Empty);

            if (user == null)
                return BadRequest("SSO User not found in system");

            try
            {
                await
                _authRepo
                .AuthenticateUser(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            await
            _authRepo
            .GenerateAuthTokenDTO(user, samlPayload);

            var coreToken =
                GenerateCoreToken(samlPayload ?? throw new Exception($"SAML Payload empty, encoded token from SSO: {NYLSSOTokenExploderService.EncodedSampleToken}"));

            // WOuld never do all this casting in real code, this is only for swagger so we can see output
            var result =
                (OkObjectResult)
                await
                LoginSSO(coreToken) ??
                new OkObjectResult("Spoof login failed");

            var jwt =
                _authRepo
                .DecodeJwt(
                    (result?.Value as GetAuthTokenDTO ?? new GetAuthTokenDTO())
                .Token ?? string.Empty);

            return
                Ok(
                    new
                    {
                        GetAuthTokenDTO = result?.Value as GetAuthTokenDTO ?? new GetAuthTokenDTO(),
                        ExplodedToken = jwt != null ? jwt.ToString() : "Failed to decode token"
                    });
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPost("success")]
    [AllowAnonymous]
    public async Task<IActionResult> SSOSuccess([FromForm] string SAMLResponse)
    {
        // NYL SSO redirects here, we validate, send to front end and they call /loginsso with coretoken
        try
        {
            if (!string.IsNullOrWhiteSpace(SAMLResponse))
            {
                // Explode token, get userID
                var samlPayload =
                    _nylTokenExploder
                    .ExplodeToken(SAMLResponse);

                // Look up user in our AD from this tenant userId (ssoId) and get system userId (AD field Email)
                var user =
                    await
                    _userService
                    .GetSSOADUserAsync(samlPayload?.SSOId ?? string.Empty);

                if (user == null)
                    return BadRequest("SSO User not found in system");

                try
                {
                    await
                    _authRepo
                    .AuthenticateUser(user);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                await
                _authRepo
                .GenerateAuthTokenDTO(user, samlPayload);

                var endcodedUserData =
                    GenerateCoreToken(samlPayload ?? throw new Exception($"SAML Payload empty, encoded token from SSO: {NYLSSOTokenExploderService.EncodedSampleToken}"));

                return
                    Redirect($"{_configuration["SSORedirect:Path"] ?? ""}?coretoken={endcodedUserData}");
            }
            else
                return BadRequest("SSO token missing");
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    private string GenerateCoreToken(SAMLPayload samlPayload)
    {
        var inputBytes =
            Encoding.ASCII.GetBytes(
                $"{CoreToken.UserId}{samlPayload.EmailAddress}&{CoreToken.SSOId}{samlPayload.SSOId}&{CoreToken.OBOSSOId}{samlPayload.OBOSSOId}&{CoreToken.OBOSession}{samlPayload.OBOSession}");

        var endcodedUserData = Convert.ToBase64String(inputBytes);

        return
            endcodedUserData;
    }

    private SAMLPayload? GenerateSAMLPayloadFromCoreToken(string coreToken)
    {
        var bytes = Convert.FromBase64String(coreToken);
        var decodedUserData = Encoding.ASCII.GetString(bytes);

        return
            string.IsNullOrWhiteSpace(decodedUserData) ?
                null :
                new SAMLPayload()
                {
                    SSOId = decodedUserData.FindBetweenTwoStrings(CoreToken.SSOId, "&"),
                    EmailAddress = decodedUserData.FindBetweenTwoStrings(CoreToken.UserId, "&"),
                    OBOSSOId = decodedUserData.FindBetweenTwoStrings(CoreToken.OBOSSOId, "&"),
                    OBOSession = string.Equals(decodedUserData.FindBetweenTwoStrings(CoreToken.OBOSession, ""), "true", StringComparison.OrdinalIgnoreCase)
                };
    }

    [HttpGet("loginsso")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginSSO(string coretoken)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(coretoken))
            {
                var samlPayload =
                    GenerateSAMLPayloadFromCoreToken(coretoken);

                if (samlPayload == null)
                    return BadRequest("Missing user data from core token");

                var user =
                    await
                    _userService
                    .GetSSOADUserAsync(samlPayload.SSOId);

                if (user == null)
                    return BadRequest("SSO User not found in system");

                try
                {
                    await
                    _authRepo
                    .AuthenticateUser(user);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return
                    Ok(
                        await
                        _authRepo
                        .GenerateAuthTokenDTO(
                            user,
                            samlPayload));
            }
            else
                return BadRequest("SSO core token missing");

        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> AuthRefresh(AuthRefreshDTO payload)
    {
        if (payload == null)
            return
                BadRequest(
                    new GetAuthTokenDTO()
                    {
                        Errors = new List<string>() { "Invalid payload" }
                    });

        GetAuthTokenDTO result;
        try
        {
            result =
                await _authRepo.VerifyToken(payload, _tokenValidationParameters);
        }
        catch (Exception ex)
        {
            result =
                new GetAuthTokenDTO()
                {
                    Errors = new List<string>() { ex.Detail() }
                };
        }

        return
            result != null ?
                Ok(result) :
                BadRequest(
                    new GetAuthTokenDTO()
                    {
                        Errors = new List<string>() { "Invalid tokens" },
                    });
    }
}