using EEH.RestAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EEH.OpenMarket
{
    public class SearchInterpark : IOpenApi
    {
        RestAPIClient restApiClient = null;

        public SearchInterpark()
        {
            restApiClient = new RestAPIClient("https://isearch.interpark.com");

        }


        public async Task<int> GetProductTotalCnt(string keyword)
        {
            int returnValue = 0;
            string textHtml = await restApiClient.GetAsync(string.Format("/isearch?q={0}", keyword));
            if (!string.IsNullOrEmpty(textHtml))
            {
                string findString = "통합검색";
                int startIndex = textHtml.IndexOf(findString)+findString.Length;
                string subString = textHtml.Substring(startIndex, 50);
                string[] split = subString.Split('(');
                if(split != null && split.Length > 1)
                {
                    string tmpStr = split[1];
                    if (!string.IsNullOrEmpty(tmpStr))
                    {
                        string[] split2 = tmpStr.Split(')');
                        if (split2 !=null && split2.Length > 0)
                        {
                            string strValue = split2[0];
                            if (!string.IsNullOrEmpty(strValue))
                            {
                                strValue = strValue.Replace(",","");
                                int.TryParse(strValue, out returnValue);
                            }
                        }
                    }
                  
                }
            }
            return returnValue;
        }
    }
}
