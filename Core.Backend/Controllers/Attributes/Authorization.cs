using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.CoreLib.Models.Constants;
using static Core.CoreLib.Models.Constants.Authorization;

namespace Core.Backend.Controllers.Attributes
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string[] requiredClaims, bool requireAll = true) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { string.Join(",", requiredClaims), requireAll };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly List<Claim> _claims;
        readonly bool _requireAll;

        public ClaimRequirementFilter(string claims, bool requireAll = true)
        {
            _claims = claims.Split(new char[] { ',' }).Select(s => new Claim(Claims.Permission, s)).ToList();
            _requireAll = requireAll;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // This code can be shortened, leaving long for readability
            var userClaims = 
                context.HttpContext.User.Claims.Where(w => w.Type == Claims.Permission).Select(s => s.Value).ToList();

            // Ignore any filters
            if (userClaims.Contains(EmployeeType.God))
                return;

            // Require all the permissions else require any (at least one) of the permissions (AND vs OR)
            if ((_requireAll && 
                _claims.Select(s => s.Value).Except(userClaims).ToList().Count > 0) ||
                userClaims.Intersect(_claims.Select(s => s.Value).Except(userClaims).ToList()).Any())
                context.Result = new ForbidResult();
        }
    }
}