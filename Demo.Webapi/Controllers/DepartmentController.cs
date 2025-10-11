using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController<Department>
    {
        public DepartmentController(IBaseBL<Department> baseBL) : base(baseBL)
        {
        }
    }
}
