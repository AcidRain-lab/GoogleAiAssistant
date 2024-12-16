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

        var httpContext = context.HttpContext;

        // Если есть заголовок Authorization, предполагаем JWT
        if (httpContext.Request.Headers.ContainsKey("Authorization"))
        {
            HandleJwtAuthorization(context);
        }
        else if (httpContext.User.Identity?.IsAuthenticated == true) // Если пользователь аутентифицирован, предполагаем Cookies
        {
            HandleCookieAuthorization(context);
        }
        else
        {
            // Если ни один механизм аутентификации не прошел
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }

    private void HandleJwtAuthorization(ActionExecutingContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token))
        {
            var jwtRole = JwtAuthHelper.GetRoleFromToken(token);
            if (!_roles.Contains(jwtRole))
            {
                context.Result = new JsonResult(new { message = "Access denied for API." })
                {
                    StatusCode = 403 // Запрет доступа
                };
            }
        }
        else
        {
            context.Result = new JsonResult(new { message = "Missing authorization token." })
            {
                StatusCode = 401 // Неавторизован
            };
        }
    }

    private void HandleCookieAuthorization(ActionExecutingContext context)
    {
        var userRole = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        if (!_roles.Contains(userRole))
        {
            // Перенаправить на AccessDenied, если роль не соответствует
            context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
        }
    }
}
