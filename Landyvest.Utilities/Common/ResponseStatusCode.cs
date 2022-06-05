using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
    public class ResponseStatusCode
    {

        public  ResponseCodeItem GetResponseCode(string Code)
        {
            var dataResult = new ResponseCodeItem();

            try
            {
                dataResult= StatusCodeDataBank().Where(a => a.Code == Code).FirstOrDefault();

                return dataResult;
            }
            catch (Exception ex)
            {
                return dataResult;
            }
        }


        private List<ResponseCodeItem> StatusCodeDataBank()
        {

            var databank = new List<ResponseCodeItem>();

            var addSuccessful = new ResponseCodeItem
            {
                Code = "200",
                Message = "Successful"

            };
            databank.Add(addSuccessful);

            var addPending = new ResponseCodeItem
            {
                Code = "400",
                Message = "Pending (Do re-query)"

            };
            databank.Add(addPending);


            var addFailed = new ResponseCodeItem
            {
                Code = "300",
                Message = "Failed"

            };
            databank.Add(addFailed);


            var addEmpty = new ResponseCodeItem
            {
                Code = "301",
                Message = "Empty fields"

            };
            databank.Add(addFailed);



            var addDuplicateRequestId = new ResponseCodeItem
            {
                Code = "310",
                Message = "Duplicate Request ID"

            };
            databank.Add(addDuplicateRequestId);


            var ServiceNotAvailable = new ResponseCodeItem
            {
                Code = "500",
                Message = "Service currently not available"

            };
            databank.Add(ServiceNotAvailable);


            var addAuthenticationFailed = new ResponseCodeItem
            {
                Code = "1000",
                Message = "Authentication Failed"

            };
            databank.Add(addAuthenticationFailed);



            var NoRecord = new ResponseCodeItem
            {
                Code = "320",
                Message = "Empty Record"

            };
            databank.Add(NoRecord);

            var SystemError = new ResponseCodeItem
            {
                Code = "330",
                Message = "System Error"

            };
            databank.Add(SystemError);


            return databank.ToList();
        }

    }


    public class ResponseCodeItem
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }


   
}
