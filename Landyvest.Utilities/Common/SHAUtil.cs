using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Landyvest.Utilities.Common
{
    public class SHAUtil
    {
        public static string GTBSHA512(string hash_string)
        {
            System.Security.Cryptography.SHA512Managed sha512 = new System.Security.Cryptography.SHA512Managed();
            Byte[] EncryptedSHA512 = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(hash_string));
            sha512.Clear();
            string hashed = BitConverter.ToString(EncryptedSHA512).Replace("-", "").ToLower();
            return hashed;
        }

        public static string Encrypt(string id)
        {
            return HttpUtility.UrlEncode(PasswordSecurity.Encrypt(id.ToString(), "siswiusj34938322*"));
        }
        public static string GetSHA512(string text)
        {
            ASCIIEncoding UE = new ASCIIEncoding();
            byte[] hashValue;
            byte[] data = UE.GetBytes(text);
            SHA512 hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(data);
            hex = ByteToString(hashValue);
            return hex;
        }
        public static string EncryptID(int id)
        {
            return HttpUtility.UrlEncode(PasswordSecurity.Encrypt(id.ToString(), "siswiusj34938322*"));
        }




       

        public static int DecryptID(string encryptedID)
        {
            try
            {
                return Convert.ToInt32(PasswordSecurity.Decrypt(HttpUtility.UrlDecode(encryptedID), "siswiusj34938322*"));
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(PasswordSecurity.Decrypt(encryptedID, "siswiusj34938322*"));
            }

        }

        public class PasswordSecurity
        {
            private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

            // This constant is used to determine the keysize of the encryption algorithm.
            private const int keysize = 256;


            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
            public static string Encrypt(string plainText, string passPhrase)
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
                {
                    byte[] keyBytes = password.GetBytes(keysize / 8);
                    using (RijndaelManaged symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    byte[] cipherTextBytes = memoryStream.ToArray();
                                    return Convert.ToBase64String(cipherTextBytes);
                                }
                            }
                        }
                    }
                }
            }


            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
            public static string Decrypt(string cipherText, string passPhrase)
            {
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
                {
                    byte[] keyBytes = password.GetBytes(keysize / 8);
                    using (RijndaelManaged symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                        {
                            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }
        }
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }


        public static string Tokenize(string amount)
        {
            return (amount.Split('.')[0].Replace(",", ""));
        }

        public static string xTokenize(string amount)
        {
            return (amount.Replace(",", ""));
        }

    }
}
