using EEH.RestAPI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EEH.OpenMarket
{
    public class OpenApiNaver : IOpenApi
    {
        RestAPIClient restApiClient = null;

        public OpenApiNaver()
        {
            string clientID = "";// Properties.APIInfoSettings.Default.OpenApiNaverClientID;
            string secret = "";//Properties.APIInfoSettings.Default.OpenApiNaverSecret;
            restApiClient = new RestAPIClient("https://openapi.naver.com");
            restApiClient.OnHeaderSettingDelegate = (header) =>
            {
                if (header != null)
                {
                    header.Add("X-Naver-Client-Id", clientID);
                    header.Add("X-Naver-Client-Secret", secret);
                }
            };
        }
        public async Task<int> GetProductTotalCnt(string keyword)
        {
            int returnValue = 0;
            string strParams = string.Format("/v1/search/shop?query={0}&display=1", keyword);
            JObject jObj = await restApiClient.GetAsync<JObject>(strParams);
            if (jObj.ContainsKey("total"))
            {
                string strCnt = jObj["total"].ToString();
                int.TryParse(strCnt, out returnValue);
            }


            return returnValue;
        }
    }
}
