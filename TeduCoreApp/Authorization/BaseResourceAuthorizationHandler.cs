using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Utilities.Constants;

namespace TeduCoreApp.Authorization
{
    public class BaseResourceAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, string>
    {
        private readonly IRoleService _roleService;

        public BaseResourceAuthorizationHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, string resource)
        {
            var roles = (context.User.Identity as ClaimsIdentity)?
                            .Claims.FirstOrDefault(c => c.Type == CommonConstants.UserClaims.Roles);
            if (roles != null)
            {
                var arrRole = roles.Value.Split(";");
                var hasPermission = await _roleService.CheckPermission(resource, requirement.Name, arrRole);
                if (hasPermission || arrRole.Contains(CommonConstants.AppRoles.AdminRole))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }
        }
    }
}
