using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
   public class MobileNumberUtil
    {
        public static string ValidateMobile(string mobile)
        {
            string Val = mobile.Substring(0, 1);
            string mobileNo;
            if (Val == "0")
            {
                mobileNo = mobile.Substring(mobile.Length - 10);
                mobileNo = "234" + mobileNo;
            }
            else
            {
                mobileNo = "234" + mobile;
            }
            return mobileNo;
        }
    }
}
