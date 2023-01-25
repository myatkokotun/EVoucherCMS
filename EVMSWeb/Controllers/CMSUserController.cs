using EVMSWeb.ApiRepo;
using EVMSWeb.Models;
using EVMSWeb.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EVMSWeb.Controllers
{
    public class CMSUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> _UserList()
        {
            string token = getToken();
            var url = $"api/Login/GetUserList";
            List<TbCmsuser> result = await APIRequest<List<TbCmsuser>>.Get(url, token);
            return PartialView("_UserList", result);
        }

        public async Task<ActionResult> _UserForm(string FormType, int ID)
        {
            string token = getToken();
            TbCmsuser result = new TbCmsuser();
            if (FormType == "Add")
            {
                return PartialView("_UserForm", result);
            }
            else
            {
                string url = string.Format("api/Login/GetUserObj?ID={0}", ID);
                result = await APIRequest<TbCmsuser>.Get(url, token);
                return PartialView("_UserForm", result);
            }
        }

        public async Task<ActionResult> UpSertUser(TbCmsuser cmsuser)
        {
            string token = getToken();
            var url = "api/Login/UpSertCMSUser";
            TbCmsuser result = await APIRequest<TbCmsuser>.Post(url, token, cmsuser);
            return Json(result);
        }
        public async Task<ActionResult> DeleteUser(int ID)
        {
            string token = getToken();
            string url = string.Format("api/Login/DeleteUser?ID={0}", ID);
            TbCmsuser result = await APIRequest<TbCmsuser>.Get(url, token);
            return Json(result);
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
