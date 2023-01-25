using EVMSWeb.ApiRepo;
using EVMSWeb.Models;
using EVMSWeb.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using X.PagedList;

namespace EVMSWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int page = 1, int pageSize = 8, string FilterName = null, string Keyword = null)
        {
            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            ViewBag.FilterName = FilterName;
            ViewBag.Keyword = Keyword;
            return View();
        }

        public async Task<IActionResult> _EVoucherList(int page = 1, int pageSize = 8, string FilterName = null, string Keyword = null)
        {
            string token = getToken();
            var url = $"api/EVoucher/GetEVoucherList?page={page}&pageSize={pageSize}&FilterName={FilterName}&Keyword={HttpUtility.UrlEncode(Keyword)}";
            PagedListModel<TbEvoucher> result = await APIRequest<PagedListModel<TbEvoucher>>.Get(url, token);
            var model = new PagingModel<TbEvoucher>();
            if (result.TotalCount > 0 && result.TotalPages > 0)
            {
                var pagedList = new StaticPagedList<TbEvoucher>(result.Results, page, pageSize, result.TotalCount);
                model.Results = pagedList;
                model.TotalPages = result.TotalPages;
                model.TotalCount = result.TotalCount;
            }
            else
            {
                model.Results = null;
                model.TotalPages = 0;
                model.TotalCount = 0;
            }
            return PartialView("_EVoucherList", model);
        }

        public async Task<ActionResult> _EVoucherForm(string FormType, int ID)
        {
            string token = getToken();
            TbEvoucher result = new TbEvoucher();
            if (FormType == "Add")
            {
                return PartialView("_EVoucherForm", result);
            }
            else
            {
                string url = string.Format("api/EVoucher/GetEVoucherObj?ID={0}", ID);
                result = await APIRequest<TbEvoucher>.Get(url, token);
                return PartialView("_EVoucherForm", result);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpSertEVoucher(TbEvoucher evoucher)
        {
            string token = getToken();
            var url = "api/EVoucher/UpSertEVoucher";
            TbEvoucher result = await APIRequest<TbEvoucher>.Post(url, token, evoucher);
            return Json(result);
        }
        public async Task<ActionResult> DeleteEVoucher(int ID)
        {
            string token = getToken();
            string url = string.Format("api/EVoucher/DeleteEVoucher?ID={0}", ID);
            TbEvoucher result = await APIRequest<TbEvoucher>.Get(url, token);
            return Json(result);
        }

        public async Task<ActionResult> getPayment()
        {
            string token = getToken();
            string Url = "api/PaymentMethod/GetPayMethodList";
            List<TbPaymentMethod> result = await APIRequest<List<TbPaymentMethod>>.Get(Url, token);
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