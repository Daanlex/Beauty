using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;

namespace Beauty.Tool
{

    public class Encrypt
    {
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        //默认密钥字符串
        private static string encryptKey = "uiej9&*4", decryptKey = "uiej9&*4";

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }


        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }


        /// <summary>
        /// 加密对象
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="th"></param>
        /// <returns></returns>
        public static T TEncryptDES<T>(T th) where T : class
        {
            try
            {
                Type type = th.GetType();
                PropertyInfo[] propertyInfo = type.GetProperties();
                if (type.IsGenericType)
                {
                    System.Collections.ICollection Ilist = th as System.Collections.ICollection;
                    foreach (object obj in Ilist)
                    {
                        type = obj.GetType();
                        propertyInfo = type.GetProperties();
                        foreach (var item in propertyInfo)
                        {
                            var val = item.GetValue(obj, null);
                            if (val != null)
                            {
                                if (val.GetType() == typeof(string))
                                    item.SetValue(obj, EncryptDES(val.ToString()), null);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in propertyInfo)
                    {
                        var val = item.GetValue(th, null);
                        if (val != null)
                        {
                            if (val.GetType() == typeof(string))
                                item.SetValue(th, EncryptDES(val.ToString()), null);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return th;
        }



        /// <summary>
        /// 解密对象
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="th"></param>
        /// <returns></returns>
        public static T TDecryptDES<T>(T th) where T : class
        {
            try
            {
                Type type = th.GetType();
                PropertyInfo[] propertyInfo = type.GetProperties();
                if (type.IsGenericType)
                {
                    System.Collections.ICollection Ilist = th as System.Collections.ICollection;
                    foreach (object obj in Ilist)
                    {
                        type = obj.GetType();
                        propertyInfo = type.GetProperties();
                        foreach (var item in propertyInfo)
                        {
                            var val = item.GetValue(obj, null);
                            if (val != null)
                            {
                                if ( val.GetType() == typeof(string))
                                    item.SetValue(obj, DecryptDES(val.ToString()), null);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in propertyInfo)
                    {
                        var val = item.GetValue(th, null);
                        if (val != null)
                        {
                            if (val.GetType() == typeof(string))
                                item.SetValue(th, DecryptDES(val.ToString()), null);
                        }
                    }
                }

                return th;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 验证注册码
        /// </summary>
        /// <param name="registCode"></param>
        /// <returns></returns>
        public static bool VerificationRegistCode(string registCode)
        {
            string publicKey = @"<RSAKeyValue><Modulus>oqJQ/jhgv0//WDGdqfUZgiecO4Qorb6CPX9DP1+WsRCf7fM
                            CYWwv7AIBLW1XpV9/y++rIEm6U+NKnJqt+B90ZHHddWYy6hCFOWvmzTS/MZqvg1HrzNZYSXjqp63ENfWJhJGLkVszInSaaADy13selr
                            cd/zs4AhT7+H7RZsV6z1U=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            bool flag = false;
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);
                    RSAPKCS1SignatureDeformatter f = new RSAPKCS1SignatureDeformatter(rsa);
                    f.SetHashAlgorithm("SHA1");
                    byte[] key = Convert.FromBase64String(registCode);
                    SHA1Managed sha = new SHA1Managed();
                    byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Common.GetCpu()));
                    string s = Convert.ToBase64String(name);
                    if (f.VerifySignature(name, key))
                        flag = true;
                }
            }
            catch (Exception)
            {
               
            }
            
            return flag;
        }
    }
}
