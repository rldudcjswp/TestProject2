using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;

namespace KBOLib.Util
{
    public class SecureUtil
    {
        public static string AesKey = "gead34egp35bag2786gbbdcxkl2f8sdq";     // 웹 프로토콜 key
        public static string AesKeyDB = "db78erahew3ghjkj463g443fsdfgb21b";   // DB 프로토콜 key

        [DllImport(@"AESDll.dll", CharSet = CharSet.Unicode)]
        extern public static int test_add(int a, int b);

        [DllImport(@"AESDll.dll", CharSet = CharSet.Unicode)]
        extern public static int test(StringBuilder buffer);

        /// <summary>
        /// SQL injection 방지를 위한 문자 변경 함수 
        /// </summary>
        /// <param name="src">검사 대상 문자열</param>
        /// <returns>안전하게 가공된 문자열</returns>
        public static string CheckBadString(string src)
        {
            string result = "";
            string[] badToken = { "‘", "“", "/", "\\", "--", ";", "%", "Union", "waitfor", "order by", "#", "xp_", "char(", "delete from", "drop table", "null", "sysobjects", "@@VERSION" };

            if (src == null)
            {
                return result;
            }

            result = src.Replace("'", "''");

            for (int i = 0; i < badToken.Length; i++)
            {
                result = result.Replace(badToken[i], "");
            }

            return result;
        }

        /// <summary>
        /// SHA512 Hash를 사용한 암호화
        /// </summary>
        /// <param name="data">암호화 대상 문자열</param>
        /// <returns>암호화된 문자열</returns>
        public static string SHA512Hash(string data)
        {
            SHA512 sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// AES256 암호화
        /// </summary>
        /// <param name="input">암호화 대상 문자열</param>
        /// <param name="key">암호화 비밀키</param>
        /// <returns>암호화된 문자열</returns>
        public static string AESEncrypt(string input, string key)
        {
            string output = "";

            if (!string.IsNullOrEmpty(input))
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] xBuff = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                output = Convert.ToBase64String(xBuff);
            }

            return output;
        }

        /// <summary>
        /// AES256 복호화
        /// </summary>
        /// <param name="input">복호화 대상 문자열</param>
        /// <param name="key">복호화 비밀키</param>
        /// <returns>복호화된 문자열</returns>
        public static string AESDecrypt(string input, string key)
        {
            string output = "";

            if (!string.IsNullOrEmpty(input))
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var decrypt = aes.CreateDecryptor();
                byte[] xBuff = null;

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                output = Encoding.UTF8.GetString(xBuff);
            }

            return output;
        }
    }
}
