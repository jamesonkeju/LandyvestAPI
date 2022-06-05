
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Models
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser<string>
    {
        public ApplicationUser()
        {
            CreatedDate = DateTime.Now;
            this.ForcePwdChange = false;
            this.IsDeleted = false;
            this.IsActive = true;
        }

        public ApplicationUser(string username)
        {
            this.UserName = username;
        }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public List<ApplicationUserRole> Roles { get; set; }

        public DateTime? DOB { get; set; }

        public string MobileNumber { get; set; }


        public DateTime? ExpirationTime { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public DateTime PwdExpiryDate { get; set; }
        public DateTime? PwdChangedDate { get; set; }
        public bool ForcePwdChange { get; set; }
        public DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string RoleId { get; set; }
        public DateTime ConfirmationLinkExpireDate { get; set; }

        /// Added OTP Code   
        public string RegistrationOTP { get; set; }
        public DateTime RegistrationExpireOTP { get; set; }

        public string  ForgetPasswordOTP { get; set; }
        public DateTime ForgetPasswordExpireOTP { get; set; }
        
        public string Token { get; set; }
        public DateTime? TokenExpiredDate { get; set; }

    }
}
