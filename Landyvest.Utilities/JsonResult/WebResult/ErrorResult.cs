using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.JsonResult.WebResult.ErrorResult
{

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class ErrorResult
        {
            public string message { get; set; }
            public bool isSuccessful { get; set; }
            public int retId { get; set; }
            public int bulkUploadId { get; set; }
        }

        

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Root
    {
        public bool hasError { get; set; }
        public string message { get; set; }
        public string statusCode { get; set; }
       
    }

    public class ApiResponse<T>
    {
        public bool hasError { get; set; }
        public string message { get; set; }
        public string statusCode { get; set; }
        public T Result { get; set; }
    }

    public class DataVendingOut
    {
        public string amount { get; set; }

        public string package { get; set; }

        public string  validity { get; set; }

        public string status { get; set; }

        public string transId { get; set; }

        public DateTime date { get; set; }

        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
        public long RetId { get; set; }
        public long BulkUploadId { get; set; }
        public string BulkUploadHtmlData { get; set; }
        public string RedirectUrl { get; set; }
        public string ReferenceNumber { get; set; }

        public List<string> Errors { get; set; }
    }



}
