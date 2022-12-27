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
        private string apiKey = string.Empty;
        private string secretKey = string.Empty;

        private RestAPIClient apiClient;
        private delegate string SignatureHandler(string timeStamp);
        private SignatureHandler GetSignatureHandler;
        public NaverAPIClient()
        {
            Init();
        }
        void Init()
        {

            apiKey = "010000000069b184f513c41bd07c3b61de224578c4bd9845b7c7812b874461361cd88b7c23";
            secretKey = "AQAAAABpsYT1E8Qb0Hw7Yd4iRXjEwi047fhafoxiY9HG++/s+Q==";

            apiClient = new RestAPIClient(naverApiUrl);
            apiClient.OnHeaderSettingDelegate = (header) =>
            {
                if (header != null)
                {
                    long timeStamp = DateUtils.GetTimeStamp();
                    string strTimeSTamp = timeStamp.ToString();
                    header.Add("X-API-KEY", apiKey);
                    header.Add("X-Customer", "1631990");
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
            return await apiClient.GetAsync<KeywordsTool>(subURL);

        }


        string GetSignature(string timeStamp, string method, string rqtUri)
        {
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(timeStamp + "." + method + "." + rqtUri)));
        }

    }
}
