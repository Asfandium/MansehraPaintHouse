using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MansehraPaintHouse.Core.Constants;

namespace MansehraPaintHouse.Admin.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]
    public abstract class BaseController : Controller
    {
        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
        }

        protected bool IsAdmin()
        {
            return User.IsInRole(RoleNames.Admin);
        }
    }
} 