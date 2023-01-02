using EEH.Keyword.Naver.Models;
using EEH.RestAPI;
using EEH.Utils;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EEH.Keyword.Naver
{
    public class NaverAPIClient
    {
        private string naverApiUrl = "https://api.naver.com";
        

        private RestAPIClient apiClient;
        private delegate string SignatureHandler(string timeStamp);
        private SignatureHandler GetSignatureHandler;
        public NaverAPIClient()
        {
            Init();
        }
        void Init()
        {
            apiClient = new RestAPIClient(naverApiUrl);
            apiClient.OnHeaderSettingDelegate = (header) =>
            {
                if (header != null)
                {
                    long timeStamp = DateUtils.GetTimeStamp();
                    string strTimeSTamp = timeStamp.ToString();
                    header.Add("X-API-KEY", APIInfoSettings.Default.KeywordSearchNaverApiKey);
                    header.Add("X-Customer", APIInfoSettings.Default.KeywordSearchNaverCustomerID);
                    header.Add("X-Timestamp", strTimeSTamp);
                    if (GetSignatureHandler != null)
                    {
                        header.Add("X-Signature", GetSignatureHandler(strTimeSTamp));
                    }
                }
            };
        }

        public async Task<KeywordsTool> Search(string keyword)
        {
            string rqtUri = "/keywordstool";

            GetSignatureHandler = (timeStamp) =>
            {
                return GetSignature(timeStamp, "GET", rqtUri);
            };

            string subURL = string.Format("{0}?hintKeywords={1}&includeHintKeywords=1&showDetail=1", rqtUri, keyword);
            KeywordsTool rtn = await apiClient.GetAsync<KeywordsTool>(subURL);

            rtn.Keyword = keyword;
            return rtn;
        }


        string GetSignature(string timeStamp, string method, string rqtUri)
        {
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(APIInfoSettings.Default.KeywordSearchNaverSecret));
            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(timeStamp + "." + method + "." + rqtUri)));
        }

    }
}
