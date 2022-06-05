using System;
using System.Collections.Generic;
using System.Text;

namespace Landyvest.Utilities.Common
{
    public class GenericUtil
    {


        public static string generatePrimaryId()
        {
            try
            {
                Guid guid = Guid.NewGuid();
                return guid.ToString("N");
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }

        public static string GenerateOTP()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            //if (rbType.SelectedItem.Value == "1")
            //{
            //    characters += alphabets + small_alphabets + numbers;
            //}

            //change i < 4 instead of 5, to generate 4 digit tokens
            string otp = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } 
                while (otp.IndexOf(character) != -1);
                otp += character;
            }

            return otp;
        }
    }
}