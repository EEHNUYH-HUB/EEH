using EEH.RestAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EEH.Ecommerce
{
    public class OpenApi11st : IOpenApi
    {
        RestAPIClient restApiClient = null;
        string key = string.Empty;
        public OpenApi11st()
        {
            restApiClient = new RestAPIClient("https://openapi.11st.co.kr");
            key = APIInfoSettings.Default.OpenApi11StKey;
        }



        public async Task<int> GetProductTotalCnt(string keyword)
        {
            int returnValue = 0;
            string strParams = string.Format("/openapi/OpenApiService.tmall?key={0}&apiCode=ProductSearch&keyword={1}&pageSize=0", key, keyword);
            string strXml = await restApiClient.GetAsync(strParams);

            XmlDocument xmlDoc = new XmlDocument();
            
            xmlDoc.LoadXml(strXml);

            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("TotalCount");
            if(nodeList != null && nodeList.Count > 0)
            {
                string strCnt = nodeList[0].InnerText;
                int.TryParse(strCnt, out returnValue);
            }
            

            return returnValue;
        }
    }
}
