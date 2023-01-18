using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEH.WEB.Parameters
{
    public class APIErrorDetails
    {
        /// <summary>
        /// Http 상태코드
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 오류 메시지
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Exception 소스 정보
        /// </summary>
        public string Source { get; set; }
        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}
