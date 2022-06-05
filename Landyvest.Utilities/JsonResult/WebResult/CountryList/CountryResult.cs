using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.JsonResult.WebResult.CountryList
{
    public class CountryResult
    {
        public string countryCode { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime lastModified { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public string ipAddress { get; set; }
    }

    public class Root
    {
        public bool hasError { get; set; }
        public string message { get; set; }
        public string statusCode { get; set; }
        public List<CountryResult> result { get; set; }
    }
}
