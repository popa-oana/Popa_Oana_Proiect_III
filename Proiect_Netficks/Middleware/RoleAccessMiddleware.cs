using Microsoft.AspNetCore.Identity;
using Proiect_Netficks.Models;
using System.Threading.Tasks;

namespace Proiect_Netficks.Middleware
{
    public class RoleAccessMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
        {
            // Process the request
            await _next(context);

            // Check if the response is a 403 Forbidden
            if (context.Response.StatusCode == 403)
            {
                // Redirect to the custom access denied page
                context.Response.Redirect("/Account/AccessDenied");
            }
        }
    }

    // Extension method to make it easier to add the middleware to the pipeline
    public static class RoleAccessMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleAccess(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoleAccessMiddleware>();
        }
    }
}
