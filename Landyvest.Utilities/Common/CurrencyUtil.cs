using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
   public class CurrencyUtil
    {
        public static decimal AmountToKobo(decimal amount)
        {
            return (decimal)(amount * 100);
        }


        public static string formatAmount(string amount)
        {
            try
            {
                double dblValue = Convert.ToDouble(amount);
                return dblValue.ToString("N", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {

                return amount;
            }
        }

        public static string formatAmountToNumber(string amount)
        {
            try
            {
                
                return String.Format("{0:0.#}", amount);
            }
            catch (Exception)
            {

                return amount;
            }
        }


       

        public static decimal KoboToAmount(decimal amount)
        {
            return (decimal)(amount / 100);
        }

        public static string GenerateGTBReferenceNumber()
        {
            return "GTB" + DateTime.Now.Ticks;
        }
    }
}

