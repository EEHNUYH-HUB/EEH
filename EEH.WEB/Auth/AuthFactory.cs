using System;
using System.Collections.Generic;
using System.Text;


namespace EEH.WEB.Auth
{
    public class AuthFactory
    {
        public static IAuthenticator SingInstance { get; set; }
    }
}
