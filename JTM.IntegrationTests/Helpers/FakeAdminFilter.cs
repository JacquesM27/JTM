using JTM.Enum;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace JTM.IntegrationTests.Helpers
{
    public class FakeAdminFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "AdminName"),
                    new Claim(ClaimTypes.Role, UserRole.admin.ToString()),
                }));

            context.HttpContext.User = claimsPrincipal;

            await next();
        }
    }
}
