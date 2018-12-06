using DS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DS.Extensions
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RoleRequirement requirement)
        {
            var roles = context.User.FindFirst(c => c.Type == "RoleUser");
            if (roles != null && roles.Value.IndexOf(requirement.Role) >= 0)
            {
                context.Succeed(requirement);
            }
            else
            {
                var redirectContext = context.Resource as AuthorizationFilterContext;
                redirectContext.Result = new RedirectToActionResult("NotPermission", "ErrorHandler", null);
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }
}
