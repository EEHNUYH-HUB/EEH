using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace EEH.Keyword.Naver.Models
{
    public class KeywordModel
    {
        [JsonProperty("relKeyword")]
        public string RelKeyword { get; set; }

        [JsonProperty("monthlyPcQcCnt")]
        public string MonthlyPcQcCnt { get; set; }

        [JsonProperty("monthlyMobileQcCnt")]
        public string MonthlyMobileQcCnt { get; set; }

        [JsonProperty("monthlyAvePcClkCnt")]
        public string MonthlyAvePcClkCnt { get; set; }

        [JsonProperty("monthlyAveMobileClkCnt")]
        public string MonthlyAveMobileClkCnt { get; set; }

        [JsonProperty("monthlyAvePcCtr")]
        public string MonthlyAvePcCtr { get; set; }

        [JsonProperty("monthlyAveMobileCtr")]
        public string MonthlyAveMobileCtr { get; set; }

        [JsonProperty("plAvgDepth")]
        public string PlAvgDepth { get; set; }

        [JsonProperty("compIdx")]
        public string CompIdx { get; set; }


    }
}
