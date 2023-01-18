using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EEH.Component.Keyword.Naver.Models
{
    public class KeywordsTool
    {
        public string Keyword { get; set; }
        [JsonProperty("keywordList")]
        public List<KeywordModel> KeywordList { get; set; }
    }
}
