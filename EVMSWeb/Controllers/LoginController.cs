using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EVMSWeb.Models;
using EVMSWeb.ApiRepo;
using EVMSWeb.ViewModels;

namespace EVMSWeb.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            if (!string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password))
            {
                return RedirectToAction("Login");
            }
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;
            var url = $"api/Login/Login?Username={Username}&Password={Password}";
            var result = await APIRequest<LoginViewModel>.GetLogin(url);
            if (result != null)
            {
                identity = CreateClaimsIdentity(result);
                isAuthenticated = true;
                ViewBag.Unauthorize = "Authorize";
            }
            else
            {
                ViewBag.Unauthorize = "Unauthorize";
            }
            if (isAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                props.AllowRefresh = true;
                props.IsPersistent = true;
                props.ExpiresUtc = DateTime.Now.AddDays(1);
                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }

        #region Cookies
        private ClaimsIdentity CreateClaimsIdentity(LoginViewModel result)
        {
            var memID = result.CmsUser.Id.ToString();
            var claims = new Claim[]
            {
              new Claim(ClaimTypes.Sid, memID),
              new Claim(ClaimTypes.Name, result.CmsUser.Name),
              new Claim("urn:Custom:Username", result.CmsUser.Username),
              new Claim("urn:Custom:MemberID", result.CmsUser.Id.ToString()),
              new Claim("urn:Custom:JwtAuth", result.JwtAuth),
            };
            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }
        public UserCookie getmember()
        {
            UserCookie mc = new UserCookie();
            if (User.Identity.IsAuthenticated)
            {
                mc.ID = Convert.ToInt32(User.FindFirst(claim => claim.Type == ClaimTypes.Sid)?.Value);
                mc.Name = User.Identity.Name;
                mc.Username = User.FindFirst(claim => claim.Type == "urn:Custom:Username")?.Value;
                mc.JwtAuth = User.FindFirst(claim => claim.Type == "urn:Custom:JwtAuth")?.Value;
            }
            return mc;
        }
        public int getMemberID()
        {
            UserCookie mc = getmember();
            return mc.ID;
        }
        public string getName()
        {
            UserCookie mc = getmember();
            return mc.Name;
        }
        public string getUsername()
        {
            UserCookie mc = getmember();
            return mc.Username;
        }

        public string getToken()
        {
            UserCookie mc = getmember();
            return mc.JwtAuth;
        }
        #endregion
    }
}
