using EVMSWeb.ApiRepo;
using EVMSWeb.Models;
using EVMSWeb.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;
using X.PagedList;

namespace EVMSWeb.Controllers
{
    public class PayMethodController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> _PayMethodList()
        {
            string token = getToken();
            var url = $"api/PaymentMethod/GetPayMethodList";
            List<TbPaymentMethod> result = await APIRequest<List<TbPaymentMethod>>.Get(url, token);            
            return PartialView("_PayMethodList", result);
        }

        public async Task<ActionResult> _PayMethodForm(string FormType, int ID)
        {
            string token = getToken();
            TbPaymentMethod result = new TbPaymentMethod();
            if (FormType == "Add")
            {
                return PartialView("_PayMethodForm", result);
            }
            else
            {
                string url = string.Format("api/PaymentMethod/GetPayMethodObj?ID={0}", ID);
                result = await APIRequest<TbPaymentMethod>.Get(url, token);
                return PartialView("_PayMethodForm", result);
            }
        }

        public async Task<ActionResult> UpSertPayMethod(TbPaymentMethod paymethod)
        {
            string token = getToken();
            var url = "api/PaymentMethod/UpSertPayMethod";
            TbPaymentMethod result = await APIRequest<TbPaymentMethod>.Post(url, token, paymethod);
            return Json(result);
        }
        public async Task<ActionResult> DeletePayMethod(int ID)
        {
            string token = getToken();
            string url = string.Format("api/PaymentMethod/DeletePayMethod?ID={0}", ID);
            TbPaymentMethod result = await APIRequest<TbPaymentMethod>.Get(url, token);
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
