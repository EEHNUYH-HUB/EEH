using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEH.WEB.Parameters
{
    public class DynamicParameter
    {/// <summary>
     /// Assembly 명
     /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// Class 명
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 메서드 명
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 메서드 파라미터
        /// </summary>
        public Dictionary<string, object> Parameter { get; set; }
        /// <summary>
        /// RequestAssembly 를 생성 합니다.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static DynamicParameter GetInstance(string assemblyName, string className, string methodName)
        {
            DynamicParameter info = new DynamicParameter();
            info.AssemblyName = assemblyName;
            info.ClassName = className;
            info.MethodName = methodName;
            info.Parameter = new Dictionary<string, object>();
            return info;
        }
    }
}
