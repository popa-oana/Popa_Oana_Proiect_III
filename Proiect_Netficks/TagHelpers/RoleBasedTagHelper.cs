using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Proiect_Netficks.Models;
using System.Threading.Tasks;

namespace Proiect_Netficks.TagHelpers
{
    [HtmlTargetElement("*", Attributes = "asp-authorize")]
    [HtmlTargetElement("*", Attributes = "asp-authorize-roles")]
    [HtmlTargetElement("*", Attributes = "asp-authorize-policy")]
    public class RoleBasedTagHelper : TagHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleBasedTagHelper(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HtmlAttributeName("asp-authorize")]
        public bool RequireAuthorization { get; set; }

        [HtmlAttributeName("asp-authorize-roles")]
        public string? AuthorizationRoles { get; set; }

        [HtmlAttributeName("asp-authorize-policy")]
        public string? AuthorizationPolicy { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Use await to make this method truly asynchronous
            await Task.CompletedTask;
            
            var user = _httpContextAccessor.HttpContext?.User;
            
            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                if (RequireAuthorization)
                {
                    output.SuppressOutput();
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(AuthorizationRoles))
            {
                var roles = AuthorizationRoles.Split(',');
                var isInRole = false;

                foreach (var role in roles)
                {
                    if (user?.IsInRole(role.Trim()) == true)
                    {
                        isInRole = true;
                        break;
                    }
                }

                if (!isInRole)
                {
                    output.SuppressOutput();
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(AuthorizationPolicy))
            {
                // For policy-based authorization, we would need to inject IAuthorizationService
                // and use it to check the policy. For simplicity, we'll just handle roles directly.
                if (AuthorizationPolicy == "PremiumContent" && 
                    !(user?.IsInRole("Premium") == true || user?.IsInRole("Admin") == true))
                {
                    output.SuppressOutput();
                    return;
                }
                else if (AuthorizationPolicy == "AdminOnly" && !(user?.IsInRole("Admin") == true))
                {
                    output.SuppressOutput();
                    return;
                }
            }
        }
    }
}
