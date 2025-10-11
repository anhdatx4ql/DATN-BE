using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginAccountController : BaseController<login_account>
    {
        public LoginAccountController(IBaseBL<login_account> baseBL) : base(baseBL)
        {
        }
    }
}