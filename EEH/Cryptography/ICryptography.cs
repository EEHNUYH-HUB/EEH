using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEH.Cryptography
{
    public interface ICryptography
    {
         string EncryptString(string strData, string pwd = CrypEnvironment.DEFAULTPASSWORD);
         string DecryptString(string strData, string pwd = CrypEnvironment.DEFAULTPASSWORD);
         void EncryptFile(string path, string pwd = CrypEnvironment.DEFAULTPASSWORD);
         void DecryptFile(string path, string pwd = CrypEnvironment.DEFAULTPASSWORD);
    }
    public enum CrypType
    {
        AES
    }
    public class CrypEnvironment
    {
        public const string DEFAULTPASSWORD = "shrTkszhffkqhfpdltuswm1!@#";
        public static ICryptography Get(CrypType type)
        {
            if(type == CrypType.AES)
            {
                return new AesCryptography();
            }
            return null;
        }
    }
}
