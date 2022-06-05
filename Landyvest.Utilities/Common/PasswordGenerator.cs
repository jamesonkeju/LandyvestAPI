using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
    public class PasswordGenerator
    {
        public static string CreateNumericRandomPassword(int length)
        {
            const string valid = "0123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string HashPassword(string password)
        {

            byte[] salt;
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, 1000))
            {
                salt = rfc2898DeriveBytes.Salt;
                bytes = rfc2898DeriveBytes.GetBytes(32);
            }
            byte[] inArray = new byte[49];
            Buffer.BlockCopy((Array)salt, 0, (Array)inArray, 1, 16);
            Buffer.BlockCopy((Array)bytes, 0, (Array)inArray, 17, 32);
            return Convert.ToBase64String(inArray);
        }

        public static string CreateLowCaseRandomPassword(int length)
        {
            const string valid = "abcdefghijkmnopqrstuvwxyz";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string CreateUpperCaseRandomPassword(int length)
        {
            const string valid = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string CreateSpecRandomPassword(int length)
        {
            const string valid = "@";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string CreateRandom(int passwordLength, bool Uppercase, bool RequireDigit, bool Lowercase)
        {
            if (RequireDigit == true && Uppercase == true && Lowercase == true)
            {
                string passcode = DateTime.Now.Ticks.ToString();
                string UpperCaseTrue1 = CreateUpperCaseRandomPassword(1);
                string LowerCaseTrue1 = CreateLowCaseRandomPassword(1);
                string passc1 = CreateSpecRandomPassword(1);
                string pass = BitConverter.ToString(new SHA512CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(passcode))).Replace("-", LowerCaseTrue1);
                var password = pass.Substring(0, passwordLength) + passc1 + UpperCaseTrue1;
                StringBuilder res = new StringBuilder();
                res.Append(password);
                return res.ToString();
            }
            else if (RequireDigit == true && Uppercase == false && Lowercase == false)
            {
                string passcode = DateTime.Now.Ticks.ToString();
                string NumericTrue2 = CreateNumericRandomPassword(1);
                string passc2 = CreateSpecRandomPassword(1);
                string pass = BitConverter.ToString(new SHA512CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(passcode))).Replace("-", NumericTrue2);
                var password = pass.Substring(0, passwordLength) + passc2;
                StringBuilder res = new StringBuilder();
                res.Append(password);
                return res.ToString();
            }
            else if (RequireDigit == true && Uppercase == true && Lowercase == false)
            {
                string passcode = DateTime.Now.Ticks.ToString();
                string NumericTrue3 = CreateNumericRandomPassword(1);
                string UpperTrue3 = CreateUpperCaseRandomPassword(1);
                string passc3 = CreateSpecRandomPassword(1);
                string pass = BitConverter.ToString(new SHA512CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(passcode))).Replace("-", NumericTrue3);
                var password = pass.Substring(0, passwordLength) + passc3 + UpperTrue3;
                StringBuilder res = new StringBuilder();
                res.Append(password);
                return res.ToString();
            }
            else if (RequireDigit == false && Uppercase == true && Lowercase == false)
            {
                string UpperTrue4 = CreateUpperCaseRandomPassword(1);
                string pass = BitConverter.ToString(new SHA512CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(UpperTrue4)));
                var password = pass.Substring(0, passwordLength);
                StringBuilder res = new StringBuilder();
                res.Append(password);
                return res.ToString();
            }
            else if (RequireDigit == false && Uppercase == false && Lowercase == true)
            {
                string LowerTrue4 = CreateLowCaseRandomPassword(1);
                string pass = BitConverter.ToString(new SHA512CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(LowerTrue4)));
                var password = pass.Substring(0, passwordLength);
                StringBuilder res = new StringBuilder();
                res.Append(password);
                return res.ToString();
            }
            string passcode5 = DateTime.Now.Ticks.ToString();
            string UpperCaseTrue5 = CreateUpperCaseRandomPassword(1);
            string LowerCaseTrue5 = CreateLowCaseRandomPassword(1);
            string passc5 = CreateSpecRandomPassword(1);
            string pass5 = BitConverter.ToString(new SHA512CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(passcode5))).Replace("-", LowerCaseTrue5);
            var password5 = pass5.Substring(0, passwordLength) + passc5 + UpperCaseTrue5;
            StringBuilder res5 = new StringBuilder();
            res5.Append(password5);
            return res5.ToString();
        }


        public static string CreateRandom()
        {

            string passcode = DateTime.Now.Ticks.ToString();
            string UpperCaseTrue1 = CreateUpperCaseRandomPassword(1);
            string LowerCaseTrue1 = CreateLowCaseRandomPassword(1);
            string passc1 = CreateSpecRandomPassword(1);
            string pass = BitConverter.ToString(new SHA512CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(passcode))).Replace("-", LowerCaseTrue1);
            var password = pass.Substring(0, 6) + passc1 + UpperCaseTrue1;
            StringBuilder res = new StringBuilder();
            res.Append(password);
            return res.ToString();

        }


        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            string password = CreateRandom(6, false, true, true);

            return password;
        }



        public static string GenerateRandomPassword1(PasswordOptions opts = null)
        {
            //if (opts == null) opts = new PasswordOptions()
            //{
            //    RequiredLength = 8,
            //    RequiredUniqueChars = 4,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireNonAlphanumeric = false,
            //    RequireUppercase = true
            //};



            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 6,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequireUppercase = false
            };



            string[] randomChars = new[] {
                "ABCDEFGHJKLMNPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnpqrstuvwxyz",    // lowercase
                "123456789",                   // digits
               
             };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            //if (opts.RequireNonAlphanumeric)
            //    chars.Insert(rand.Next(0, chars.Count),
            //        randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}
