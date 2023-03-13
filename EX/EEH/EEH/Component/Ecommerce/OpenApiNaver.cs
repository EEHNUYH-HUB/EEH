using EEH.RestAPI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EEH.Component.Ecommerce
{
    public class OpenApiNaver : IOpenApi
    {
        RestAPIClient restApiClient = null;
        public EcommerceType Type => EcommerceType.ETNAVER;
        public OpenApiNaver()
        {
            
            restApiClient = new RestAPIClient("https://openapi.naver.com");
            restApiClient.OnHeaderSettingDelegate = (header) =>
            {
                if (header != null)
                {
                    header.Add("X-Naver-Client-Id", APIInfoSettings.Default.OpenApiNaverClientID);
                    header.Add("X-Naver-Client-Secret", APIInfoSettings.Default.OpenApiNaverSecret);
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
