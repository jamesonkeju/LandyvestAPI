using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
    public class Helper
    {
       
        public static string FindConstantName<T>(Type containingType, T value)
        {
            var comparer = EqualityComparer<T>.Default;

            foreach (FieldInfo field in containingType.GetFields
                (BindingFlags.Static | BindingFlags.Public))
            {
                if (field.FieldType == typeof(T) &&
                    comparer.Equals(value, (T)field.GetValue(null)))
                {
                    return field.Name;
                }
            }
            return null; // Or throw an exception
        }


        public static int? GetCount(object @object)
        {
            var collection = @object as ICollection;
            return collection?.Count;
        }

        public static string RemoveInvalidCharacters(string unique)
        {
            //" \ / : | < > * ?
            unique = unique
                .Replace(" ", "")
                .Replace(",", "")
                .Replace(@"\", "")
                .Replace(@"/", "")
                .Replace(":", "")
                .Replace("|", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("*", "")
                .Replace("?", "")
                .Replace("\"", "");

            return unique;
        }
        public class MaxFileSizeAttribute : ValidationAttribute
        {
            private readonly int _maxFileSize;
            public MaxFileSizeAttribute(int maxFileSize)
            {
                _maxFileSize = maxFileSize;
            }

          
            public string GetErrorMessage()
            {
                return $"Maximum allowed file size is { _maxFileSize} bytes.";
            }
        }

        public class AllowedExtensionsAttribute : ValidationAttribute
        {
            private readonly string[] _extensions;
            public AllowedExtensionsAttribute(string[] extensions)
            {
                _extensions = extensions;
            }



            public string GetErrorMessage()
            {
                return $"This photo extension is not allowed!";
            }
        }
        public static decimal GetDecimalVal(string s)
        {
            try
            {
                return decimal.Parse(s);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static string _GenerateRandomString()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }



        public static string DataTableToHTMLTable(DataTable dt)
        {
            if (dt.Rows.Count == 0) return "";

            var builder = new StringBuilder();
            builder.Append(
                "<table id='bulk-transfer-upload' class='table table-striped table-bordered dt-responsive nowrap' style='border-collapse: collapse; border-spacing: 0; width: 100%;'>");
            builder.Append("<thead><tr>");
            foreach (DataColumn c in dt.Columns)
            {
                builder.Append("<th>");
                builder.Append(c.ColumnName);
                builder.Append("</th>");
            }
            builder.Append("</tr></thead>");
            foreach (DataRow r in dt.Rows)
            {
                builder.Append("<tr style='font-size: 12px;'>");
                foreach (DataColumn c in dt.Columns)
                {
                    builder.Append("<td>");
                    builder.Append(r[c.ColumnName]);
                    builder.Append("</td>");
                }
                builder.Append("</tr>");
            }
            builder.Append("</table>");

            return builder.ToString();
        }


        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == '.')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

       
        public static string ReadFromFile(string fullPath)
        {
            try
            {
                string base64Str = "";
                var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8);
                base64Str = streamReader.ReadToEnd();
                return base64Str;
            }
            catch (Exception)
            {
                return "";
            }
        }


        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        public static string GetPeriodDifferece(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null)
            {
                return "";
            }
            var period = date2 - date1;
            var days = period.Value.TotalDays;
            string response = "";
            var remainingDays = days;
            if (remainingDays > 365)
            {
                response = ((int)Math.Round(remainingDays / 366)).ToString() + " year(s) ";
                remainingDays = remainingDays % 366;
            }
            if (remainingDays > 30)
            {
                response = response + ((int)Math.Round(remainingDays / 30)).ToString() + " month(s) ";
                remainingDays = remainingDays % 30;
            }
            if (remainingDays >= 1)
            {
                response = response + ((int)remainingDays).ToString() + " day(s) ";
            }
            else
            {
                if (response == "")
                {
                    var hours = period.Value.TotalHours;
                    if (hours >= 1)
                    {
                        response = response + ((int)hours).ToString() + " hour(s)";
                    }
                    else
                    {
                        var minutes = period.Value.TotalMinutes;
                        response = response + ((int)minutes).ToString() + " minute(s)";
                    }
                }
            }

            return response;
        }


        public static DateTime? GetDateValue(string strDate)
        {
            try
            {
                return DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                try
                {
                    return DateTime.ParseExact(strDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    try
                    {
                        return DateTime.ParseExact(strDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            return DateTime.ParseExact(strDate, "dd MMM yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public static string GetDateString(DateTime? dDate)
        {
            try
            {
                return ((DateTime)dDate).ToString("dd/MM/yyyy");
            }
            catch (Exception)
            {
                try
                {
                    return ((DateTime)dDate).ToString("dd-MM-yyyy");
                }
                catch (Exception)
                {
                    try
                    {
                        return ((DateTime)dDate).ToString("dd-MMM-yyyy");
                    }
                    catch (Exception)
                    {
                        try
                        {
                            return ((DateTime)dDate).ToString("dd MMM yyyy");
                        }
                        catch (Exception)
                        {
                            return "";
                        }
                    }
                }

            }
        }
    }
}