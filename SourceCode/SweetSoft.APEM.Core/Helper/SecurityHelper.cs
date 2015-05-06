using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace SweetSoft.APEM.Core
{
    public class SecurityHelper
    {
        public const string APPLICATION_NAME = "APEM";
        public const string ACCESS_DENIED = "ACCESS DENIED!!!";
        public const string ROLES_OF_FUNCTION_CACHE_KEY = "ROLES_OF_FUNCTION_CACHE_KEY";
        public static string GPDN_EncryptionPrivateKey = "j+zqNUpaAm/Psqz0o77Gyg==";
        //ducnm fix 25.11
        public static string CONTAINER_PLACE = "container";
        //end fix
        /// <summary>
        /// Decrypts text
        /// </summary>
        /// <param name="CipherText">Cipher text</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, GPDN_EncryptionPrivateKey);
        }

        public static string Decrypt(string cipherText, string key)
        {
            if (String.IsNullOrEmpty(cipherText))
                return cipherText;

            TripleDESCryptoServiceProvider tDESalg = new TripleDESCryptoServiceProvider();
            tDESalg.Key = new ASCIIEncoding().GetBytes(key.Substring(0, 16));
            tDESalg.IV = new ASCIIEncoding().GetBytes(key.Substring(8, 8));
            cipherText = cipherText.Replace(' ', '+');
            byte[] buffer = Convert.FromBase64String(cipherText);
            string result = DecryptTextFromMemory(buffer, tDESalg.Key, tDESalg.IV);
            return result;
        }

        /// <summary>
        /// Encrypts text
        /// </summary>
        /// <param name="PlainText">Plaint text</param>        
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string plainText)
        {
            return Encrypt(plainText, GPDN_EncryptionPrivateKey);
        }

        public static string Encrypt(string plainText, string key)
        {
            if (String.IsNullOrEmpty(plainText))
                return plainText;

            TripleDESCryptoServiceProvider tDESalg = new TripleDESCryptoServiceProvider();
            tDESalg.Key = new ASCIIEncoding().GetBytes(key.Substring(0, 16));
            tDESalg.IV = new ASCIIEncoding().GetBytes(key.Substring(8, 8));

            byte[] encryptedBinary = EncryptTextToMemory(plainText, tDESalg.Key, tDESalg.IV);
            string result = Convert.ToBase64String(encryptedBinary);
            return result;
        }


        private static byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
        {
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV), CryptoStreamMode.Write);
            byte[] toEncrypt = new UnicodeEncoding().GetBytes(Data);
            cStream.Write(toEncrypt, 0, toEncrypt.Length);
            cStream.FlushFinalBlock();
            byte[] ret = mStream.ToArray();
            cStream.Close();
            mStream.Close();
            return ret;
        }

        private static string DecryptTextFromMemory(byte[] Data, byte[] Key, byte[] IV)
        {
            MemoryStream msDecrypt = new MemoryStream(Data);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV), CryptoStreamMode.Read);
            StreamReader sReader = new StreamReader(csDecrypt, new UnicodeEncoding());
            return sReader.ReadLine();
        }
    }
}
