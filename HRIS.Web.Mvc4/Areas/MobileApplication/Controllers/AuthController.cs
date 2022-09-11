using Project.Web.Mvc4.Areas.MobileApplication.Attributes;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.MobileApplication.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        [Route("login")]
        [HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public HttpResponseMessage Login()
        {
            return BuildSuccessResult(HttpStatusCode.OK);
        }
    }
}
