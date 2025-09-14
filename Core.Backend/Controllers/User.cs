
using Core.Backend.Extensions;
using Core.CoreLib.Models.Azure.ActiveDirectory;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Models.DTO.User;
using Core.CoreLib.Models.Exceptions;
using Core.CoreLib.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using static Core.CoreLib.Models.Constants.Authorization;

namespace CoreBackend.Controllers;

[Route("api/users")]
[ApiController]
public sealed class UserController : ControllerBaseTokenized
{
    private readonly Core.CoreLib.Services.User.IUserService _userService;
    private readonly Core.CoreLib.Services.Database.User.IUserService _dbUserService;
    private readonly ILogger<Controller> _logger;

    public UserController(
        IUserService userRepo,
        Core.CoreLib.Services.Database.User.IUserService dbUserService,
        ILogger<Controller> logger)
    {
        _userService = userRepo;
        _dbUserService = dbUserService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetUserList()
    {
        try
        {
            return Ok(new List<GetUserListDTO>());
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("account")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            var user =
                await _userService.GetADUserAsync(GetCurrentUserId());

            if (user == null)
                throw new DataNotFoundException("User Id not found");

            var acceptanceData =
                await
                _dbUserService.GetUserAcceptanceData(user.GraphUserData.Id);

            if (IsOBOSession())
            {
                var oboUser =
                    await _userService.GetSSOADUserAsync(GetOBOSessionId());

                if (oboUser == null)
                    throw new DataNotFoundException($"OBO session detected, OBO user not found: {GetOBOSessionId()}");

                return Ok(user.ToWebNYLUser(oboUser.ToWebNYLUser(acceptanceData!)));
            }
            else
                return Ok(user.ToWebNYLUser(acceptanceData!));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("permissions", Name = "Permissions")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserPermissions(string userEmail)
    {
        var claims = new List<Claim>();

        var userPermissions =
            await
            _userService
            .GetADUserAsync(
                userEmail);

        if (userPermissions != null && userPermissions.Permissions.Count > 0)
            foreach (var permission in userPermissions.Permissions)
                claims.Add(new Claim(Claims.Permission, permission));
        else
            claims.Add(new Claim(Claims.Permission, Permissions.ReadSite));

        return Ok(claims);
    }

    [HttpPost("setacceptance")]
    [AllowAnonymous]
    public async Task<IActionResult> SetAcceptance(UserAcceptanceDTO userAcceptance)
    {
        try
        {
            if (userAcceptance == null ||
                string.IsNullOrEmpty(userAcceptance.FirstName) ||
                string.IsNullOrEmpty(userAcceptance.LastName) ||
                string.IsNullOrEmpty(userAcceptance.MarketerId))
                return BadRequest("Invalid data");

            var users =
                await _userService.GetADUsersAsync(userAcceptance.FirstName, userAcceptance.LastName);

            if (users == null || users.Count == 0)
                return BadRequest("User not found matching First Name and Last Name");

            ADUser? userFound = null;
            foreach (var user in users)
            {
                // Get DDCData
                var detail =
                    await
                    _dbUserService.GetUserAccessInfoDetailDDCData(user.SSOId);

                if (detail == null ||
                    detail.MarketerNo != userAcceptance.MarketerId)
                    continue;
                else
                {
                    userFound = user;
                    break;
                }
            }

            if (userFound == null)
                return BadRequest($"User not found matching Marketer Id {userAcceptance.MarketerId}");

            var result =
                await
                _dbUserService.SetUserAcceptanceData(
                    userFound.GraphUserData.Mail,
                    userFound.GraphUserData.Id, 
                    userAcceptance.WelcomePagePresented, 
                    userAcceptance.AcceptedTerms);

            return Ok($"Success");
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpGet("usermigrationinfo")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserMigrationInfo(string marketerNo)
    {
        try
        {
            if (string.IsNullOrEmpty(marketerNo))
                return BadRequest("Invalid marketer number");

            return
                Ok(await _dbUserService.GetUserMigrationInfo(marketerNo));
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }

    [HttpPost("setusermigrationinfo")]
    [AllowAnonymous]
    public async Task<IActionResult> SetUserMigrationInfo(UserMigrationInfoDTO userMigrationInfo)
    {
        try
        {
            if (string.IsNullOrEmpty(userMigrationInfo.MarketerNo))
                return BadRequest("Invalid or missing marketer number");

            if (string.IsNullOrEmpty(userMigrationInfo.MarketerNo))
                return BadRequest("Invalid or missing NYL Id");

            return
                Ok(await _dbUserService.SetUserMigrationInfo(userMigrationInfo) + " row(s) updated");
        }
        catch (Exception ex)
        {
            return ex.PackageExceptionResponse(_logger);
        }
    }
}