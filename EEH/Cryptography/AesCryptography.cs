using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using EEH;
using System.IO;

namespace EEH.Cryptography
{
    public class AesCryptography : ICryptography
    {
        private static readonly int saltSize = 256;

        #region FILE
        void DecryptFile(Stream inputStream, Stream outputStream, string key = CrypEnvironment.DEFAULTPASSWORD)
        {

            using (DeriveBytes rgb = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(key)))
            {
                using (SymmetricAlgorithm aes = Aes.Create())
                {
                    aes.BlockSize = 128;
                    aes.KeySize = saltSize;
                    aes.Key = rgb.GetBytes(aes.KeySize >> 3);
                    aes.IV = rgb.GetBytes(aes.BlockSize >> 3);
                    aes.Mode = CipherMode.CBC;


                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    using (CryptoStream cs = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                    {

                        long size = 1024000;
                        long totalLen = inputStream.Length;
                        long currentLen = 0;
                        bool isWhile = true;

                        while (isWhile)
                        {
                            if (currentLen + size >= totalLen)
                            {
                                size = totalLen - currentLen;
                                size -= 16;
                                isWhile = false;
                            }
                            if (size > 0)
                            {
                                byte[] buffer = new byte[size];

                                cs.Read(buffer, 0, buffer.Length);
                                outputStream.Write(buffer, 0, buffer.Length);


                                currentLen += size;
                            }
                        }

                        int data;
                        while ((data = cs.ReadByte()) != -1)
                        {
                            outputStream.WriteByte((byte)data);
                        }

                    }
                }
            }
        }

        void EncryptFile(Stream inputStream, Stream outputStream, string key = CrypEnvironment.DEFAULTPASSWORD)
        {


            using (DeriveBytes rgb = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(key)))

            {

                //System.Security.Cryptography.Aes a = new 
                using (SymmetricAlgorithm aes = Aes.Create())
                {

                    aes.BlockSize = 128;
                    aes.KeySize = saltSize;
                    aes.Key = rgb.GetBytes(aes.KeySize >> 3);
                    aes.IV = rgb.GetBytes(aes.BlockSize >> 3);
                    aes.Mode = CipherMode.CBC;


                    using (ICryptoTransform cryptor = aes.CreateEncryptor())
                    {
                        using (CryptoStream cs = new CryptoStream(outputStream, cryptor, CryptoStreamMode.Write))
                        {
                            long size = 1024000;
                            long totalLen = inputStream.Length;
                            long currentLen = 0;
                            bool isWhile = true;

                            while (isWhile)
                            {
                                if (currentLen + size >= totalLen)
                                {
                                    size = totalLen - currentLen;
                                    size -= 16;
                                    isWhile = false;
                                }
                                if (size > 0)
                                {
                                    byte[] buffer = new byte[size];

                                    inputStream.Read(buffer, 0, buffer.Length);
                                    cs.Write(buffer, 0, buffer.Length);


                                    currentLen += size;
                                }
                            }

                            int data;
                            while ((data = inputStream.ReadByte()) != -1)
                                cs.WriteByte((byte)data);


                        }
                    }
                }
            }
        }

        public void DecryptFile(string inputPath, string pwd = CrypEnvironment.DEFAULTPASSWORD)
        {
            string outputPath = inputPath.ExDirectoryName().ExCombine(Guid.NewGuid() + ".tmp");
            using (FileStream output = new FileStream(outputPath, FileMode.Create))
            {
                using (FileStream input = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
                {

                    DecryptFile(input, output, pwd);
                }
            }
            inputPath.ExFileDelete();
            File.Move(outputPath, inputPath);
        }

        public void EncryptFile(string inputPath, string pwd = CrypEnvironment.DEFAULTPASSWORD)
        {
            string outputPath = inputPath.ExDirectoryName().ExCombine(Guid.NewGuid() + ".tmp");
            using (FileStream output = new FileStream(outputPath, FileMode.Create))
            {
                using (FileStream input = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
                {

                    EncryptFile(input, output, pwd);
                }
            }
            inputPath.ExFileDelete();
            File.Move(outputPath, inputPath);
        }
        #endregion

        #region STRING
        public string DecryptString(string strData, string pwd = CrypEnvironment.DEFAULTPASSWORD)
        {
            try
            {
                using (SymmetricAlgorithm rijndaelCipher = Aes.Create())
                {

                    byte[] encryptedData = Convert.FromBase64String(strData);
                    byte[] salt = Encoding.ASCII.GetBytes(pwd.Length.ToString());

                    using (PasswordDeriveBytes secretKey = new PasswordDeriveBytes(pwd, salt))
                    {

                        // Decryptor 객체를 만듭니다.
                        using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                        {

                            using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                            {

                                // 데이터 읽기(복호화이므로) 용도로 cryptoStream객체를 선언, 초기화
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {

                                    // 복호화된 데이터를 담을 바이트 배열을 선언합니다.
                                    // 길이는 알 수 없지만, 일단 복호화되기 전의 데이터의 길이보다는
                                    // 길지 않을 것이기 때문에 그 길이로 선언합니다
                                    byte[] plainText = new byte[encryptedData.Length];

                                    // 복호화 시작
                                    int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);


                                    // 복호화된 데이터를 문자열로 바꿉니다.
                                    string decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);

                                    // 최종 결과 리턴
                                    return decryptedData;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return strData;
            }
        }
        public string EncryptString(string strData, string pwd = CrypEnvironment.DEFAULTPASSWORD)
        {
            try
            {
                //Rijndael class를 선언하고, 초기화 합니다
                using (SymmetricAlgorithm rijndaelCipher = Aes.Create())
                {

                    //입력받은 문자열을 바이트 배열로 변환
                    byte[] plaintxt = Encoding.Unicode.GetBytes(strData);

                    //딕셔너리 공격을 대비해서 키를 더 풀기 어렵게 만들기 위해서 Salt를 사용합니다.
                    byte[] salt = Encoding.ASCII.GetBytes(pwd.Length.ToString());

                    //PasswordDeriveBytes 클래스를 사용해서 SecretKey를 얻습니다
                    using (PasswordDeriveBytes secretkey = new PasswordDeriveBytes(pwd, salt))
                    {

                        //Create a encryptor from the existing SecretKey bytes.
                        // encryptor 객체를 SecretKey로부터 만듭니다.
                        // Secret Key에는 32바이트
                        // (Rijndael의 디폴트인 256bit가 바로 32바이트입니다)를 사용하고, 
                        // Initialization Vector로 16바이트
                        // (역시 디폴트인 128비트가 바로 16바이트입니다)를 사용합니다
                        using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(secretkey.GetBytes(32), secretkey.GetBytes(16)))
                        {

                            //메모리스트림 객체를 선언,초기화 
                            using (MemoryStream memoryStream = new MemoryStream())
                            {

                                // CryptoStream객체를 암호화된 데이터를 쓰기 위한 용도로 선언
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {

                                    // 암호화 프로세스가 진행됩니다.
                                    cryptoStream.Write(plaintxt, 0, plaintxt.Length);

                                    // 암호화 종료
                                    cryptoStream.FlushFinalBlock();


                                    // 암호화된 데이터를 바이트 배열로 담습니다.
                                    byte[] CipherBytes = memoryStream.ToArray();

                                    // 스트림 해제
                                    memoryStream.Close();
                                    cryptoStream.Close();

                                    // 암호화된 데이터를 Base64 인코딩된 문자열로 변환합니다.
                                    string EncryptedData = Convert.ToBase64String(CipherBytes);

                                    // 최종 결과를 리턴
                                    return EncryptedData;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return strData;
            }
        }
        #endregion


    }
}
