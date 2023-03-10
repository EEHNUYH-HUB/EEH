using EEH.RestAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace EEH.Component.Ecommerce
{
    public class OpenApi11st : IOpenApi
    {
        RestAPIClient restApiClient = null;
        
        public OpenApi11st()
        {
            restApiClient = new RestAPIClient("https://openapi.11st.co.kr");
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//.net core Encoding 문제로 등록 (등록 안할시 Encoding.GetEncoding("euc-kr") 에러발생)
        }

        public EcommerceType Type => EcommerceType.ET11ST;

        public async Task<int> GetProductTotalCnt(string keyword)
        {
            int returnValue = 0;

            
           string encKeyword = HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("euc-kr"));
            string strParams = string.Format("/openapi/OpenApiService.tmall?key={0}&apiCode=ProductSearch&keyword={1}&pageSize=0", APIInfoSettings.Default.OpenApi11StKey, encKeyword);
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
