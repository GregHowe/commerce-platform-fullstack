
using Core.CoreLib.Models.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreBackend.Controllers;
//TODO: Put entire user ADUserObject on here after login???
public class ControllerBaseTokenized : ControllerBase
{
    private ClaimsIdentity? ClaimsIdentity =>
        User?.Identity as ClaimsIdentity ?? null;

	private int? GetClaimAsInt(string claimName) =>
		ClaimsIdentity != null &&
        (ClaimsIdentity.Claims?.Count() ?? 0) > 0 &&
        ClaimsIdentity.Claims?.Where(c => c.Type == claimName).FirstOrDefault() != null &&
        ClaimsIdentity.Claims?.First(c => c.Type == claimName)?.Value != null &&
		int.TryParse(ClaimsIdentity.Claims.First(c => c.Type == claimName).Value, out var id) ?
			id : 
			null;

    private string? GetIdClaimAsString(string claimName) =>
        ClaimsIdentity != null &&
        (ClaimsIdentity.Claims?.Count() ?? 0) > 0 &&
        ClaimsIdentity.Claims?.Where(c => c.Type == claimName).FirstOrDefault() != null &&
        ClaimsIdentity.Claims?.First(c => c.Type == claimName)?.Value != null ?
            ClaimsIdentity.Claims.First(c => c.Type == claimName).Value :
            null;

    private bool GetIdClaimAsBool(string claimName) =>
        ClaimsIdentity != null &&
        (ClaimsIdentity.Claims?.Count() ?? 0) > 0 &&
        ClaimsIdentity.Claims?.Where(c => c.Type == claimName).FirstOrDefault() != null &&
        ClaimsIdentity.Claims?.First(c => c.Type == claimName)?.Value != null ?
            string.Equals(ClaimsIdentity.Claims.First(c => c.Type == claimName).Value, "true", StringComparison.OrdinalIgnoreCase) :
            false;

    protected int GetCurrentBrandId()
    {
        var brandId = GetClaimAsInt(Claims.BrandId);

        if (brandId == null)
            throw new Exception("Session missing brand id");
        else if (brandId == 1)
            return 3;
        else
            return brandId.Value;
    }

    protected string GetCurrentUserId() =>
        GetIdClaimAsString(Claims.UserEmail) ?? throw new Exception("Session missing user id");

    protected bool IsOBOSession() =>
        GetIdClaimAsBool(Claims.OBOSession);

    protected string GetOBOSessionId() =>
        GetIdClaimAsString(Claims.OBOSessionSSOId) ?? string.Empty;

    protected List<string> GetCurrentUserPermissions()
	{
        if (ClaimsIdentity != null &&
            (ClaimsIdentity.Claims?.Count() ?? 0) > 0)
        {
            var permissions = 
				ClaimsIdentity.Claims?.Where(c => c.Type == Claims.Permission)?.Select(s => s.Value).ToList() ?? new List<string>() { "readonly" };

            if (permissions != null)
                return permissions;
        }

        return new List<string>() { "readonly" };
    }

    protected bool HasPermission(string permissionKey) => 
        GetCurrentUserPermissions().Contains(permissionKey);
}