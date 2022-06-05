using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.JsonResult.WebResult
{
    public class AccountLoginResult
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class UserInformation
        {
            public string firstName { get; set; }
            public string middleName { get; set; }
            public string lastName { get; set; }
            public DateTime dob { get; set; }
            public string mobileNumber { get; set; }
            public DateTime expirationTime { get; set; }
            public DateTime lastLoginDate { get; set; }
            public DateTime pwdExpiryDate { get; set; }
            public DateTime pwdChangedDate { get; set; }
            public bool forcePwdChange { get; set; }
            public DateTime lastModified { get; set; }
            public string modifiedBy { get; set; }
            public DateTime createdDate { get; set; }
            public string createdBy { get; set; }
            public bool isDeleted { get; set; }
            public bool isActive { get; set; }
            public string roleId { get; set; }
            public DateTime confirmationLinkExpireDate { get; set; }
            public string id { get; set; }
            public string userName { get; set; }
            public string normalizedUserName { get; set; }
            public string email { get; set; }
            public string normalizedEmail { get; set; }
            public bool emailConfirmed { get; set; }
            public string passwordHash { get; set; }
            public string securityStamp { get; set; }
            public string concurrencyStamp { get; set; }
            public string phoneNumber { get; set; }
            public bool phoneNumberConfirmed { get; set; }
            public bool twoFactorEnabled { get; set; }
            public DateTime lockoutEnd { get; set; }
            public bool lockoutEnabled { get; set; }
            public int accessFailedCount { get; set; }
        }

        public class Result
        {
            public string roleId { get; set; }
            public string menuString { get; set; }
            public string menus { get; set; }
            public string token { get; set; }
            public DateTime expiryTime { get; set; }
            public string role { get; set; }
            public int rolesId { get; set; }
            public UserInformation userInformation { get; set; }
        }

        public class Root
        {
            public bool hasError { get; set; }
            public string message { get; set; }

            public Result result { get; set; }

        }


    }

    public class ProductResult
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Result
        {
            public string productName { get; set; }
            public string productDescription { get; set; }
            public string productCode { get; set; }
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
            public List<Result> result { get; set; }
        }


    }
}
