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
    /// <summary>
    /// Class Role Handler.
    /// </summary>
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        /// <summary>
        /// Validate role policy state.
        /// </summary>
        /// <param name="context">The AuthorizationHandlerContext.</param>
        /// <param name="requirement">The RoleRequirement.</param>
        /// <returns></returns>
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
                context.Fail();
            }

            return Task.CompletedTask;
        }

    }
}
