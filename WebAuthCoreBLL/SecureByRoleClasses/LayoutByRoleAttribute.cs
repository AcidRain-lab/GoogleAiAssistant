//  зависимость лайоута от сессии 
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WebAuthCoreBLL.SecureByRoleClasses
{
  public class LayoutByRoleAttribute : ActionFilterAttribute
  {
    public override void OnResultExecuting(ResultExecutingContext context)
    {
      var controller = context.Controller as Controller;
      var userRole = context.HttpContext.Session.GetString("UserRole");

      if (controller != null && !string.IsNullOrEmpty(userRole))
      {
        controller.ViewBag.UserRole = userRole;
        switch (userRole)
        {
          case ApplicationConstants.AdminRoleName:
            controller.ViewData["Layout"] = "_AdminLayout";
            break;
          case ApplicationConstants.UserRoleName:
            controller.ViewData["Layout"] = "_UserLayout";
            break;
          default:
            controller.ViewData["Layout"] = "_UserLayout";
            break;
        }

      }
    }
  }
}

