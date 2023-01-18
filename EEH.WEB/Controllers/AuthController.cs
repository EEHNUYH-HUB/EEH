using EEH.WEB.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEH.WEB.Controllers
{
    public class AuthController :BaseController
    {
        public AuthController(IAuthenticator ctx) : base(ctx) { }
        /// <summary>
        /// 인증키를 생성한다
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public object GenerateKey([FromBody] Dictionary<string,object> requestParameters)
        {
            return CurrentContext.GenerateKey(requestParameters);
        }

        /// <summary>
        /// 인증정보를 가지고온다
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public object CertificationInfo(string key)
        {
            return CurrentContext.GetInfo(key.ToString());
        }
    }
}
