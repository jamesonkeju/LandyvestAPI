using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
    public class Util
    {

        public static void SetDateCulture()
        {
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }
        public static DateTime ConvertDateValue(Object obj)
        {
            SetDateCulture();
            DateTime result;
            var succeeded = DateTime.TryParse(obj.ToString(), out result);
            if (succeeded == true)
            {
                return result;
            }
            else
            {
                return DateTime.Now;
            }
        }

        public static string GeneratePassword(int passwordLength)
        {
            return RandomPassword.Generate(passwordLength);
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


        #region Common





        public class NameAndValueObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }





        public enum AppRole
        {
            Super_Admin = 1,
            //Portal_Admin = 2,
            Member = 2,
            Non_Member = 3,

        }

        public enum RequestStatus
        {
            Pending = 1,   // First level approval
            Processing = 2, // Second Level approval
            BusinessManager = 3,  // third level approval
            Approved = 4,  // create user
            Reject = 5,   // reject 
            Failed = 6,
        }
        public static List<NameAndValueObject> SystemRoles()
        {
            return Enum.GetValues(typeof(AppRole)).Cast<AppRole>().Select(x => new NameAndValueObject
            {
                Name = x.ToString().Replace('_', ' '),
                Id = ((int)x)
            }).ToList();
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        //public static ApplicationUser GetUserById(long Id)
        //{
        //    IRepositoryQuery<ApplicationUser> _applicationUserQuery = new RepositoryQuery<ApplicationUser>();
        //    var list = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@Id", Id)
        //    };

        //    #region Execute StoreProcedure
        //    var items = _applicationUserQuery.StoreprocedureQueryFor<ApplicationUser>("GetUserById @Id", list.ToArray()).ToList();
        //    if (items.Count == 0)
        //    {
        //        return new ApplicationUser();
        //    }
        //    #endregion

        //    return items.FirstOrDefault();
        //}



        #endregion

        public class RandomPassword
        {
            private static int DEFAULT_MIN_PASSWORD_LENGTH = 6;
            private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;

            // Define supported password characters divided into groups.
            // You can add (or remove) characters to (from) these groups.
            private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnpqrstwxyz";
            private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            private static string PASSWORD_CHARS_NUMERIC = "23456789";
            //  private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

            /// <summary>
            /// Generates a random password.
            /// </summary>
            /// <returns>
            /// Randomly generated password.
            /// </returns>
            /// <remarks>
            /// The length of the generated password will be determined at
            /// random. It will be no shorter than the minimum default and
            /// no longer than maximum default.
            /// </remarks>
            public static string Generate()
            {
                return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                                DEFAULT_MAX_PASSWORD_LENGTH);
            }

            /// <summary>
            /// Generates a random password of the exact length.
            /// </summary>
            /// <param name="length">
            /// Exact password length.
            /// </param>
            /// <returns>
            /// Randomly generated password.
            /// </returns>
            public static string Generate(int length)
            {
                return Generate(length, length);
            }

            public static string GenerateRandomLetter(int length)
            {
                return GenerateLetter(length, length);
            }

            /// <summary>
            /// Generates a random password.
            /// </summary>
            /// <param name="minLength">
            /// Minimum password length.
            /// </param>
            /// <param name="maxLength">
            /// Maximum password length.
            /// </param>
            /// <returns>
            /// Randomly generated password.
            /// </returns>
            /// <remarks>
            /// The length of the generated password will be determined at
            /// random and it will fall with the range determined by the
            /// function parameters.
            /// </remarks>
            public static string Generate(int minLength,
                                          int maxLength)
            {
                // Make sure that input parameters are valid.
                if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                    return null;

                // Create a local array containing supported password characters
                // grouped by types. You can remove character groups from this
                // array, but doing so will weaken the password strength.
                char[][] charGroups = new char[][]
            {
            PASSWORD_CHARS_LCASE.ToCharArray(),
                //   PASSWORD_CHARS_UCASE.ToCharArray(),
                //   PASSWORD_CHARS_NUMERIC.ToCharArray(),
                //  PASSWORD_CHARS_SPECIAL.ToCharArray()
            };

                // Use this array to track the number of unused characters in each
                // character group.
                int[] charsLeftInGroup = new int[charGroups.Length];

                // Initially, all characters in each group are not used.
                for (int i = 0; i < charsLeftInGroup.Length; i++)
                    charsLeftInGroup[i] = charGroups[i].Length;

                // Use this array to track (iterate through) unused character groups.
                int[] leftGroupsOrder = new int[charGroups.Length];

                // Initially, all character groups are not used.
                for (int i = 0; i < leftGroupsOrder.Length; i++)
                    leftGroupsOrder[i] = i;

                // Because we cannot use the default randomizer, which is based on the
                // current time (it will produce the same "random" number within a
                // second), we will use a random number generator to seed the
                // randomizer.

                // Use a 4-byte array to fill it with random bytes and convert it then
                // to an integer value.
                byte[] randomBytes = new byte[4];

                // Generate 4 random bytes.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(randomBytes);

                // Convert 4 bytes into a 32-bit integer value.
                int seed = (randomBytes[0] & 0x7f) << 24 |
                            randomBytes[1] << 16 |
                            randomBytes[2] << 8 |
                            randomBytes[3];

                // Now, this is real randomization.
                Random random = new Random(seed);

                // This array will hold password characters.
                char[] password = null;

                // Allocate appropriate memory for the password.
                if (minLength < maxLength)
                    password = new char[random.Next(minLength, maxLength + 1)];
                else
                    password = new char[minLength];

                // Index of the next character to be added to password.
                int nextCharIdx;

                // Index of the next character group to be processed.
                int nextGroupIdx;

                // Index which will be used to track not processed character groups.
                int nextLeftGroupsOrderIdx;

                // Index of the last non-processed character in a group.
                int lastCharIdx;

                // Index of the last non-processed group.
                int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

                // Generate password characters one at a time.
                for (int i = 0; i < password.Length; i++)
                {
                    // If only one character group remained unprocessed, process it;
                    // otherwise, pick a random character group from the unprocessed
                    // group list. To allow a special character to appear in the
                    // first position, increment the second parameter of the Next
                    // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                    if (lastLeftGroupsOrderIdx == 0)
                        nextLeftGroupsOrderIdx = 0;
                    else
                        nextLeftGroupsOrderIdx = random.Next(0,
                                                             lastLeftGroupsOrderIdx);

                    // Get the actual index of the character group, from which we will
                    // pick the next character.
                    nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                    // Get the index of the last unprocessed characters in this group.
                    lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                    // If only one unprocessed character is left, pick it; otherwise,
                    // get a random character from the unused character list.
                    if (lastCharIdx == 0)
                        nextCharIdx = 0;
                    else
                        nextCharIdx = random.Next(0, lastCharIdx + 1);

                    // Add this character to the password.
                    password[i] = charGroups[nextGroupIdx][nextCharIdx];

                    // If we processed the last character in this group, start over.
                    if (lastCharIdx == 0)
                        charsLeftInGroup[nextGroupIdx] =
                                                  charGroups[nextGroupIdx].Length;
                    // There are more unprocessed characters left.
                    else
                    {
                        // Swap processed character with the last unprocessed character
                        // so that we don't pick it until we process all characters in
                        // this group.
                        if (lastCharIdx != nextCharIdx)
                        {
                            char temp = charGroups[nextGroupIdx][lastCharIdx];
                            charGroups[nextGroupIdx][lastCharIdx] =
                                        charGroups[nextGroupIdx][nextCharIdx];
                            charGroups[nextGroupIdx][nextCharIdx] = temp;
                        }
                        // Decrement the number of unprocessed characters in
                        // this group.
                        charsLeftInGroup[nextGroupIdx]--;
                    }

                    // If we processed the last group, start all over.
                    if (lastLeftGroupsOrderIdx == 0)
                        lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                    // There are more unprocessed groups left.
                    else
                    {
                        // Swap processed group with the last unprocessed group
                        // so that we don't pick it until we process all groups.
                        if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                        {
                            int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                            leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                        leftGroupsOrder[nextLeftGroupsOrderIdx];
                            leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                        }
                        // Decrement the number of unprocessed groups.
                        lastLeftGroupsOrderIdx--;
                    }
                }

                // Convert password characters into a string and return the result.
                return new string(password);
            }


            public static string GenerateLetter(int minLength,
                                          int maxLength)
            {
                // Make sure that input parameters are valid.
                if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                    return null;

                // Create a local array containing supported password characters
                // grouped by types. You can remove character groups from this
                // array, but doing so will weaken the password strength.
                char[][] charGroups = new char[][]
            {
            PASSWORD_CHARS_UCASE.ToCharArray(),
                //  PASSWORD_CHARS_SPECIAL.ToCharArray()
            };

                // Use this array to track the number of unused characters in each
                // character group.
                int[] charsLeftInGroup = new int[charGroups.Length];

                // Initially, all characters in each group are not used.
                for (int i = 0; i < charsLeftInGroup.Length; i++)
                    charsLeftInGroup[i] = charGroups[i].Length;

                // Use this array to track (iterate through) unused character groups.
                int[] leftGroupsOrder = new int[charGroups.Length];

                // Initially, all character groups are not used.
                for (int i = 0; i < leftGroupsOrder.Length; i++)
                    leftGroupsOrder[i] = i;

                // Because we cannot use the default randomizer, which is based on the
                // current time (it will produce the same "random" number within a
                // second), we will use a random number generator to seed the
                // randomizer.

                // Use a 4-byte array to fill it with random bytes and convert it then
                // to an integer value.
                byte[] randomBytes = new byte[4];

                // Generate 4 random bytes.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(randomBytes);

                // Convert 4 bytes into a 32-bit integer value.
                int seed = (randomBytes[0] & 0x7f) << 24 |
                            randomBytes[1] << 16 |
                            randomBytes[2] << 8 |
                            randomBytes[3];

                // Now, this is real randomization.
                Random random = new Random(seed);

                // This array will hold password characters.
                char[] password = null;

                // Allocate appropriate memory for the password.
                if (minLength < maxLength)
                    password = new char[random.Next(minLength, maxLength + 1)];
                else
                    password = new char[minLength];

                // Index of the next character to be added to password.
                int nextCharIdx;

                // Index of the next character group to be processed.
                int nextGroupIdx;

                // Index which will be used to track not processed character groups.
                int nextLeftGroupsOrderIdx;

                // Index of the last non-processed character in a group.
                int lastCharIdx;

                // Index of the last non-processed group.
                int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

                // Generate password characters one at a time.
                for (int i = 0; i < password.Length; i++)
                {
                    // If only one character group remained unprocessed, process it;
                    // otherwise, pick a random character group from the unprocessed
                    // group list. To allow a special character to appear in the
                    // first position, increment the second parameter of the Next
                    // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                    if (lastLeftGroupsOrderIdx == 0)
                        nextLeftGroupsOrderIdx = 0;
                    else
                        nextLeftGroupsOrderIdx = random.Next(0,
                                                             lastLeftGroupsOrderIdx);

                    // Get the actual index of the character group, from which we will
                    // pick the next character.
                    nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                    // Get the index of the last unprocessed characters in this group.
                    lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                    // If only one unprocessed character is left, pick it; otherwise,
                    // get a random character from the unused character list.
                    if (lastCharIdx == 0)
                        nextCharIdx = 0;
                    else
                        nextCharIdx = random.Next(0, lastCharIdx + 1);

                    // Add this character to the password.
                    password[i] = charGroups[nextGroupIdx][nextCharIdx];

                    // If we processed the last character in this group, start over.
                    if (lastCharIdx == 0)
                        charsLeftInGroup[nextGroupIdx] =
                                                  charGroups[nextGroupIdx].Length;
                    // There are more unprocessed characters left.
                    else
                    {
                        // Swap processed character with the last unprocessed character
                        // so that we don't pick it until we process all characters in
                        // this group.
                        if (lastCharIdx != nextCharIdx)
                        {
                            char temp = charGroups[nextGroupIdx][lastCharIdx];
                            charGroups[nextGroupIdx][lastCharIdx] =
                                        charGroups[nextGroupIdx][nextCharIdx];
                            charGroups[nextGroupIdx][nextCharIdx] = temp;
                        }
                        // Decrement the number of unprocessed characters in
                        // this group.
                        charsLeftInGroup[nextGroupIdx]--;
                    }

                    // If we processed the last group, start all over.
                    if (lastLeftGroupsOrderIdx == 0)
                        lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                    // There are more unprocessed groups left.
                    else
                    {
                        // Swap processed group with the last unprocessed group
                        // so that we don't pick it until we process all groups.
                        if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                        {
                            int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                            leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                        leftGroupsOrder[nextLeftGroupsOrderIdx];
                            leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                        }
                        // Decrement the number of unprocessed groups.
                        lastLeftGroupsOrderIdx--;
                    }
                }

                // Convert password characters into a string and return the result.
                return new string(password);
            }

        }



    }
    public static class CryptoClass
    {
        /// <summary>
        /// This security key should be very complex and Random for encrypting the text. This playing vital role in encrypting the text.
        /// </summary>
        private const string _securityKey = "FleetKeyForCryptographyZInk";


        /// <summary>
        /// This method is used to convert the plain text to Encrypted/Un-Readable Text format.
        /// </summary>
        /// <param name="PlainText">Plain Text to Encrypt before transferring over the network.</param>
        /// <returns>Cipher Text</returns>
        public static string EncryptPlainTextToCipherText(string PlainText)
        {
            //Getting the bytes of Input String.
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));

            //De-allocatinng the memory after doing the Job.
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;

            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;

            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();

            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);

            //Releasing the Memory Occupied by TripleDES Service Provider for Encryption.
            objTripleDESCryptoService.Clear();

            //Convert and return the encrypted data/byte into string format.
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


        /// <summary>
        /// This method is used to convert the Cipher/Encypted text to Plain Text.
        /// </summary>
        /// <param name="CipherText">Encrypted Text</param>
        /// <returns>Plain/Decrypted Text</returns>
        public static string DecryptCipherTextToPlainText(string CipherText)
        {
            byte[] toEncryptArray = Convert.FromBase64String(CipherText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));

            //De-allocatinng the memory after doing the Job.
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;

            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;

            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();

            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //Releasing the Memory Occupied by TripleDES Service Provider for Decryption.          
            objTripleDESCryptoService.Clear();

            //Convert and return the decrypted data/byte into string format.
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }

    public static class ExtentionUtility
    {

        

       



        public static String EncryptID(this Int64 ID)
        {
            try
            {
                string EcryptedApplicantID = CryptoClass.EncryptPlainTextToCipherText(ID.ToString());
                EcryptedApplicantID = EcryptedApplicantID.Replace("/", "~");
                EcryptedApplicantID = EcryptedApplicantID.Replace("\\", "`");
                return EcryptedApplicantID;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static String Encrypt(this string Text)
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


        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }



        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a == null || b == null || a.Length != b.Length)
                return false;

            bool flag = true;
            for (int index = 0; index < a.Length; ++index)
                flag = flag & (a[index] == b[index]);

            return flag;
        }


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


        public static Int64 DecryptID(this string ID)
        {
            try
            {
                ID = ID.Replace("~", "/");
                ID = ID.Replace("`", "\\");
                Int64 DecryptedApplicantID = Convert.ToInt64(CryptoClass.DecryptCipherTextToPlainText(ID));
                return DecryptedApplicantID;
            }
            catch (Exception ex)
            {
                return 0;
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
        public static string GenerateReferenceNo()
        {

           return string.Format("{0}", DateTime.Now.ToString("yyyyMMddHHmmssfffff"));
        }


      
    }
}
