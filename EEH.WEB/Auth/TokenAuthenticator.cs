using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EEH.WEB.Auth
{
    public class TokenAuthenticator : IAuthenticator
    {
        public string Key { get { return "76a3115fc329997e6178fa4c6c05ead35c5cedbc2c85c7ad96f6aa9e47c0a019"; } }
        public string Issuer { get { return "https://co.kr"; } }
        public string Audience { get { return "https://co.kr"; } }
        public int Expired { get { return 24; } }

        public string GenerateKey(Dictionary<string, object> param)
        {
            List<Claim> claimList = new List<Claim>();
            claimList.Add(new Claim("NameIdentifier", Guid.NewGuid().ToString()));
            foreach (var keyValue in param)
            {
                claimList.Add(new Claim(keyValue.Key, keyValue.Value.ToString()));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(Issuer, Audience, claimList.ToArray(), notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(Expired), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public Dictionary<string, string> GetInfo(string token)
        {
            if (token == null) return null;


            Dictionary<string, string> rtn = new Dictionary<string, string>();

            try
            {
                byte[] mySecret = Encoding.UTF8.GetBytes(Key);
                SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(mySecret);
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters tokenParameter = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    // true로 설정하는 경우 validation을 하지 않음
                    ValidateLifetime = false,
                    ValidateAudience = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = mySecurityKey,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token.ToString(), tokenParameter, out SecurityToken validatedToken);
                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                List<Claim> claims = jwtToken.Claims.ToList<Claim>();
                bool removeDefaultType = true;
                foreach (var claim in claims)
                {
                    if (removeDefaultType)
                    {
                        if(claim.Type.Equals("NameIdentifier") ||
                            claim.Type.Equals("nbf") ||
                            claim.Type.Equals("exp") ||
                            claim.Type.Equals("iss") ||
                            claim.Type.Equals("aud") )
                        {
                            continue;
                        }
                    }
                    rtn[claim.Type] = claim.Value;
                }
            }
            catch 
            {

            }
          

            return rtn;
        }

        public bool IsValidate(string token)
        {
            var infos = GetInfo(token);
            return infos.ExNotNull() && infos.Count > 0;
        }
    }
}
