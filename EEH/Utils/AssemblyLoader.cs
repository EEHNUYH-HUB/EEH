using EEH.WEB.Parameters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace EEH.Utils
{
    public class AssemblyLoader
    {
        public static object Run( DynamicParameter rqtParma)
        {
            if (rqtParma != null)
            {
                Assembly assembly = Assembly.Load(rqtParma.AssemblyName);

                if (assembly != null)
                {
                    Type[] types = assembly.GetExportedTypes();

                    if (types != null && types.Length > 0)
                    {
                        for (int i = 0; i < types.Length; i++)
                        {
                            Type objectType = types[i];
                            if (objectType != null
                                && !string.IsNullOrEmpty(objectType.Name)
                                && !string.IsNullOrEmpty(rqtParma.ClassName)
                                && !string.IsNullOrEmpty(rqtParma.MethodName))
                            {

                                if (objectType.Name.ToLower() == rqtParma.ClassName.ToLower())
                                {


                                    object targetObj = Activator.CreateInstance(objectType);

                                    System.Reflection.MethodInfo methodInfo = objectType.GetMethod(rqtParma.MethodName);
                                    object[] param = null;

                                    if (rqtParma.Parameter != null && rqtParma.Parameter.Count > 0)
                                    {
                                        List<object> objParams = new List<object>();


                                        ParameterInfo[] methodParam = methodInfo.GetParameters();


                                        foreach (ParameterInfo pInfo in methodParam)
                                        {
                                            if (rqtParma.Parameter.ContainsKey(pInfo.Name))
                                            {
                                                object p = rqtParma.Parameter[pInfo.Name];
                                                string strTypeName = pInfo.ParameterType.Name.ToLower();
                                                if (strTypeName != "string"
                                                    && strTypeName != "int32"
                                                    && strTypeName != "object"
                                                    && strTypeName != "boolean"
                                                    && strTypeName != "byte"
                                                    && strTypeName != "byte[]"
                                                    && strTypeName != "datetime")
                                                {

                                                    if (p != null)
                                                    {

                                                        p = JsonConvert.DeserializeObject(p.ToString(), pInfo.ParameterType);
                                                    }
                                                    else
                                                        p = JsonConvert.DeserializeObject("", pInfo.ParameterType);
                                                }
                                                if (p != null)
                                                {
                                                    if (strTypeName == "string")
                                                    {
                                                        objParams.Add(p.ToString());
                                                    }
                                                    else if (strTypeName == "byte[]")
                                                    {

                                                        string str = p?.ToString();
                                                        if (!string.IsNullOrEmpty(str))
                                                        {
                                                            byte[] fileBytes = Convert.FromBase64String(str);
                                                            objParams.Add(fileBytes);
                                                        }
                                                        else
                                                        {
                                                            objParams.Add(null);
                                                        }
                                                    }
                                                    else if (strTypeName == "object")
                                                    {
                                                        objParams.Add(p);
                                                    }
                                                    else
                                                    {
                                                        objParams.Add(Convert.ChangeType(p, pInfo.ParameterType));
                                                    }
                                                }
                                                else
                                                    objParams.Add(p);
                                            }
                                            else
                                            {
                                                objParams.Add(pInfo.DefaultValue is DBNull ? null : pInfo.DefaultValue);
                                            }


                                        }

                                        if (objParams != null && objParams.Count > 0)
                                            param = objParams.ToArray();
                                    }



                                    object obj = methodInfo.Invoke(targetObj, param);
                                    return obj;

                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

    }
}
