using EEH.WEB.Auth;
using EEH.WEB.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEH.WEB.Controllers
{
    [ApiController]
    [AuthFilterAttribute]
    [Route("api/[controller]/[action]")]
    public class BaseController : ControllerBase
    {
        public IAuthenticator CurrentContext { get { return AuthFactory.SingInstance; } }
        public BaseController(IAuthenticator ctx)
        {
            AuthFactory.SingInstance = ctx;
        }
    }
}
