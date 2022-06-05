using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
   public class DateTimeUtil
    {
        public static string ConvertDate(string DataValue)
        {
            try
            {
                string[] BreakStartDate = DataValue.Split("/");
                string data = BreakStartDate[2] + "-" + BreakStartDate[0] + "-" + BreakStartDate[1];

                return data;
            }
            catch (Exception)
            {

                return DataValue;
            }
        }

        public static string FormatDate(string date)
        {
            string formatDate = date.Replace(" 12:00:00 AM", "");
            string[] breakDate = formatDate.Split('/');
            string dd = "";
            string mm = "";
            string newdate = "";
            if (breakDate[0].Length == 1)
            {
                dd = "0" + breakDate[0];
            }
            else
            {
                dd = breakDate[0];
            }
            if (breakDate[1].Length == 1)
            {
                mm = "0" + breakDate[1];
            }
            else
            {
                mm = breakDate[1];
            }
            newdate = breakDate[2] + "-" + dd + "-" + mm;
            return newdate.ToString().Replace(" 12:00:00 AM", "");
        }


        public static string FormatDateByBackSlash(string date)
        {
            string formatDate = date.Replace(" 12:00:00 AM", "");
            string[] breakDate = formatDate.Split('/');
            string dd = "";
            string mm = "";
            string newdate = "";
            if (breakDate[0].Length == 1)
            {
                dd = "0" + breakDate[0];
            }
            else
            {
                dd = breakDate[0];
            }
            if (breakDate[1].Length == 1)
            {
                mm = "0" + breakDate[1];
            }
            else
            {
                mm = breakDate[1];
            }
            newdate = mm + "/" + dd + "/" + breakDate[2];
            return newdate.ToString().Replace(" 12:00:00 AM", "");
        }

        public static string ConvertDate_Date(string DataValue)
        {
            try
            {
                string[] BreakStartDate = DataValue.Split("/");
                string data = BreakStartDate[2] + "-" + BreakStartDate[0] + "-" + BreakStartDate[1];

                return data;
            }
            catch (Exception)
            {

                return DataValue;
            }
        }


        public static int FetchMonthDigit(string Month)
        {
            int digit = 0;
            if (Month == "January")
            {

                digit = 1;
            }
            else if (Month == "February")

            {
                digit = 2;
            }

            else if (Month == "March")

            {
                digit = 3;
            }
            else if (Month == "April")

            {
                digit = 4;
            }
            else if (Month == "May")

            {
                digit = 5;
            }
            else if (Month == "June")

            {
                digit = 6;
            }
            else if (Month == "July")

            {
                digit = 7;
            }

            else if (Month == "August")

            {
                digit = 8;
            }
            else if (Month == "September")

            {
                digit = 9;
            }
            else if (Month == "October")

            {
                digit = 10;
            }
            else if (Month == "November")

            {
                digit = 11;
            }
            else if (Month == "December")

            {
                digit = 12;
            }
            return digit;
        }



    }
}
