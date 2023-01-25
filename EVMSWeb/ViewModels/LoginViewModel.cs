using EVMSWeb.Models;

namespace EVMSWeb.ViewModels
{
    public class LoginViewModel
    {
        public TbCmsuser? CmsUser { get; set; }
        public string? JwtAuth { get; set; }
    }
}
