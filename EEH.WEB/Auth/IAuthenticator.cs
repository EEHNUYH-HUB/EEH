using System;
using System.Collections.Generic;
using System.Text;

namespace EEH.WEB.Auth
{
    
    public interface IAuthenticator
    {
         


        string Key { get; }
        string Issuer { get; }
        string Audience { get; }
        int Expired { get; }
        string GenerateKey(Dictionary<string, object> param);
        bool IsValidate(string token);

        Dictionary<string, string> GetInfo(string token);
    }
}
