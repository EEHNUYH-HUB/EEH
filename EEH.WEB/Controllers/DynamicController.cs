using EEH.Utils;
using EEH.WEB.Auth;
using EEH.WEB.Parameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEH.WEB.Controllers
{
    public class DynamicController : BaseController
    {
        public DynamicController(IAuthenticator ctx) : base(ctx) { }
        [HttpPost]
        public object LoadAssembly([FromBody] DynamicParameter info)
        {
            return AssemblyLoader.Run(info);
        }
    }
}
