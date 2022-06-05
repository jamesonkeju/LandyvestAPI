using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
    public static class URLEncrytion
    {
        public static String Decrypt(this string Text)
        {
            Text = AddSpecialXter(Text);
            try
            {
                //string newText = Text.Replace("_", "+").Replace("_", "+").Replace("_", "+").Replace("_", "+").Replace("_", "+");
                String Decrypted = CryptoClass.DecryptCipherTextToPlainText(Text);
                return Decrypted;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static String EncryptByid(this string Text)
        {
            try
            {
                String Encrypted = CryptoClass.EncryptPlainTextToCipherText(Text);
                Encrypted = RemoveSpecialXter(Encrypted);
                //return Ecrypted.Replace("+", "_").Replace("+", "_").Replace("+", "_").Replace("+", "_").Replace("+", "_");
                return Encrypted;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private static string RemoveSpecialXter(string encryptString)
        {
            int count = encryptString.Count(f => (f == '+') || (f == '/'));

            if (count != 0)
            {
                foreach (char ch in encryptString)
                {
                    if (ch == '+')
                    {
                        encryptString = encryptString.Replace("+", "_");
                    }
                    if (ch == '/')
                    {
                        encryptString = encryptString.Replace("/", "~");
                    }
                }
            }

            return encryptString;
        }

        private static string AddSpecialXter(string cipherText)
        {
            int count = cipherText.Count(f => (f == '_') || (f == '~'));

            if (count != 0)
            {
                foreach (char ch in cipherText)
                {
                    if (ch == '_')
                    {
                        cipherText = cipherText.Replace("_", "+");
                    }
                    if (ch == '~')
                    {
                        cipherText = cipherText.Replace("~", "/");
                    }
                }
            }

            return cipherText;
        }
    }
}
