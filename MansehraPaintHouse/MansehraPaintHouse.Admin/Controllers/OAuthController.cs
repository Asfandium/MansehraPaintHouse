using Microsoft.AspNetCore.Mvc;
using MansehraPaintHouse.Core.Interfaces.IServices;
using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Admin.Controllers
{
    public class OAuthController : Controller
    {
        private readonly IOAuthService _oauthService;

        public OAuthController(IOAuthService oauthService)
        {
            _oauthService = oauthService;
        }

        public async Task<IActionResult> Login()
        {
            var url = await _oauthService.GetAuthorizationUrlAsync();
            return Redirect(url);
        }

        public async Task<IActionResult> Callback(string code, string state)
        {
            var user = await _oauthService.HandleCallbackAsync(code);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Index", "Home");
        }
    }
} 