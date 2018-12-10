using System.Linq;
using System.Security.Claims;

namespace TeduCoreApp.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetSpecificClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Equals(claimType));
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}
