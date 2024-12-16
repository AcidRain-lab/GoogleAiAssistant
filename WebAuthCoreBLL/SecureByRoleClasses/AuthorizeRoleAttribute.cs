using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using WebAuthCoreBLL.Helpers;

public class AuthorizeRolesAttribute : ActionFilterAttribute
{
    private readonly string[] _roles;

    public AuthorizeRolesAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (context.HttpContext.Request.Headers.ContainsKey("Authorization")) // API
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                var jwtRole = JwtAuthHelper.GetRoleFromToken(token);
                if (!_roles.Contains(jwtRole))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            else
            {
                context.Result = new ForbidResult(); // Отказать, если токен отсутствует
                return;
            }
        }
        else // MVC
        {
            var userRole = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!_roles.Contains(userRole))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
        }
    }
}
