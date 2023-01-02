using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        long monthlyQcCnt = long.MinValue;
        public long MonthlyQcCnt
        {
            get
            {
                if (monthlyQcCnt == long.MinValue)
                {
                    monthlyQcCnt = MonthlyPcQcCnt.ExLong() + MonthlyMobileQcCnt.ExLong();
                }
                return monthlyQcCnt;
            }
        }
        double monthlyAveClkCnt = double.MinValue;
        public double MonthlyAveClkCnt
        {
            get
            {
                if (monthlyAveClkCnt == double.MinValue)
                {
                    monthlyAveClkCnt =Math.Round( MonthlyAvePcClkCnt.ExDouble() + MonthlyAveMobileClkCnt.ExDouble(),3);
                }
                return monthlyAveClkCnt;
            }
        }
        double monthlyAveCtr = double.MinValue;
        public double MonthlyAveCtr
        {
            get
            {
                if (monthlyAveCtr == double.MinValue)
                {
                    monthlyAveCtr = Math.Round( (MonthlyAvePcCtr.ExDouble() + MonthlyAveMobileCtr.ExDouble())/2.0,3);
                }
                return monthlyAveCtr;
            }
        }



        public long EcommerceNaverCnt { get; set; }
        public long Ecommerce11stCnt { get; set; }
        public long EcommerceInterparkCnt { get; set; }
    }
}
