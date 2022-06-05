using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Landyvest.Data.Payload.PasswordUtility;
using Microsoft.AspNetCore;
namespace Landyvest.Data.Payload
{
   public class UserLoginPayload
   {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    [Serializable]
    public class AdminUserSettingViewModel
    {

        public string Id { get; set; }
        [Required(ErrorMessage = "* First name required")]
        [DisplayName("First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "* Surname  required")]
        [DisplayName("Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [DisplayName("Middle Name")]
        [StringLength(50)]
        public string MiddleName { get; set; }


        //[Required(ErrorMessage = "* Username required")]
        [DisplayName("User Name")]
        [StringLength(50)]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "* Mobile number required")]
        [DisplayName("Mobile Number")]
        [StringLength(50)]
        public string MobileNumber { get; set; }

        [DisplayName("Phone Number")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }


        public string Status
        {
            get
            {
                return (IsActive ? "Active" : "In Active");
            }
        }



        [Required(ErrorMessage = "* Email required")]
        [DisplayName("Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[Required(ErrorMessage = "* Password required")]
        //[ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "* Confirm password")]
        [DataType(DataType.Password)]
        //[ValidatePasswordLength]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please select role")]
        public string RoleId { get; set; }

        //[Required(ErrorMessage = "* Select at least one role")]
        public List<long> SelectedRoleIDs { get; set; }

        //[Required(ErrorMessage = "* Select at least one role")]
        public List<string> SelectedUserRoleIDs { get; set; }

        public IEnumerable<Landyvest.Data.Models.ApplicationUserRole> UserRoles { get; set; }
        public List<Landyvest.Data.Models.ApplicationRole> Roles { get; set; }

        public List<Landyvest.Data.Payload.CountryFilter> RoleList { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }

        public DateTime PwdExpiryDate { get; set; }
        public DateTime? PwdChangedDate { get; set; }
        public bool ForcePwdChange { get; set; }
        public DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public bool IsDeleted { get; set; }
        public string Action { get; set; }
        public string ActionTitle { get; set; }
        public string RoleName { get; set; }

        
        public string ResponderNameAndAgency { get; set; }

    }
}
