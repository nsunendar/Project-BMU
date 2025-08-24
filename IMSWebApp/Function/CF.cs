using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IMSWebApp.Function
{
    public static class CF
    {   
        public static string CheckValue(string val) => !(val == "empty") ? val : " ";        

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            string resultStr = Convert.ToBase64String(array);

            return Base64UrlEncoder.Encode(resultStr);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(Base64UrlEncoder.Decode(cipherText));

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string GFDecript(string sWord)
        {
            //string functionReturnValue = null;
            string cDEncrypt = null;
            byte nAscValue = 0;
            byte nCnt = 0;
            int iI = 0;
            int lentext = sWord.Length;
            string isi = null;
            byte cekChar = 0;
            char cekChar2;

            //lentext = sWord.Length;

            cDEncrypt = "";
            if (lentext == 0)
            {
                //functionReturnValue = "";
                return "";
            }
            nCnt = 2;
            for (iI = 0; iI <= lentext - 1; iI++)
            {
                isi = sWord.Substring(iI, 1);
                char c = Convert.ToChar(isi);
                nAscValue = (byte)(Asc(c) - nCnt);
                if (nCnt > 4)
                {
                    nCnt = 1;
                }
                nCnt += 1;
                if (nAscValue > 255)
                {
                    cekChar = 255;
                }
                else
                {
                    cekChar = nAscValue;
                }
                cekChar2 = Chr(cekChar);
                cDEncrypt = cDEncrypt + cekChar2; //sWord.Char((nAscValue > 255 ? 255 : nAscValue));
            }
            //functionReturnValue = cDEncrypt;
            return cDEncrypt;
        }

        public static byte Asc(char src)
        {
            return (System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(src + "")[0]);
        }

        public static char Chr(byte src)
        {
            Encoding utf8 = Encoding.UTF8;
            //char[] characters = utf8.GetChars(new byte[] { src });  System.Text.Encoding.ASCII.GetChars(new byte[] { src });
            char[] characters = System.Text.Encoding.GetEncoding("us-ascii").GetChars(new byte[] { src });
            char c = characters[0];
            return c;
        }

        public static string GFEncript(string sWord)
        {
            string cEncrypt = null;
            byte nAscValue = 0;
            byte nCnt = 0;
            int iI = 0;
            int lentext = sWord.Length;
            string isi;
            byte cekChar = 0;
            char cekChar2;

            cEncrypt = "";

            if (lentext == 0)
            {
                //functionReturnValue = "";
                return "";
            }
            nCnt = 2;
            for (iI = 0; iI <= lentext - 1; iI++)
            {
                isi = sWord.Substring(iI, 1);
                char c = Convert.ToChar(isi);
                nAscValue = (byte)(Asc(c) + nCnt);
                if (nCnt > 4)
                {
                    nCnt = 1;
                }
                nCnt += 1;
                if (nAscValue > 255)
                {
                    cekChar = 255;
                }
                else
                {
                    cekChar = nAscValue;
                }
                cekChar2 = Chr(cekChar);
                if (cekChar2.ToString() == "'")
                {
                    cEncrypt = cEncrypt + "'";
                }
                cEncrypt = cEncrypt + cekChar2;
            }
            return cEncrypt;
        }

        /*public static async Task<string> removeTime(string dtstr)
        {
            return string.IsNullOrEmpty(dtstr) ? "" : DateTime.Parse(dtstr).Date.ToString("dd/MM/yyyy");
        }*/

        public static async Task<string> removeTime(string dtstr) => string.IsNullOrEmpty(dtstr) ? "" : DateTime.Parse(dtstr).Date.ToString("dd-MM-yyyy");

        /*public static async Task<string> formatTime(string dtstr)
        {
            return string.IsNullOrEmpty(dtstr) ? "" : DateTime.Parse(dtstr).ToString("hh:mm tt");
        }*/

        public static async Task<string> formatTime(string dtstr) => string.IsNullOrEmpty(dtstr) ? "" : DateTime.Parse(dtstr).ToString("hh:mm tt");

        public static async Task<string> formatTableDate(string dtstr, string tstr)
        {
            string[] strArray1 = dtstr.Split(" ", StringSplitOptions.None);
            string[] strArray2 = strArray1[0].Split("/", StringSplitOptions.None);
            return strArray2[1] + "-" + strArray2[0] + "-" + strArray2[2] + " " + tstr;
        }

        public static async Task<string> GetFileExtension(string filename)
        {
            string path = "wwwroot\\upload\\";
            string existingFile = Directory.EnumerateFiles(path, filename + ".*").FirstOrDefault();

            return Path.GetExtension(existingFile);
        }

        public static async Task<string> formatPrice(string priceStr)
        {
            string priceFormatted = String.Format("{0:n0}", Int32.Parse(priceStr)).Replace(",", ".");
            return priceFormatted;
        }

        public static async Task<string> checkemptystring(string val)
        {
            string str = val;
            if (val == " ")
            {
                str = "";
            }

            return str;
        }

        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public static string checkValue(string val) => !(val == "empty") ? val : " ";

        public static string checkDecValue(string val) => !(val == "empty") ? val : "0";

        public static string checkValueDB(string cVal) => !(cVal == " ") ? cVal : (string)null;
    }
}
