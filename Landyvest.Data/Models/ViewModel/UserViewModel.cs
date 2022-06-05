using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Landyvest.Data.Models.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "* First name required")]
        // [DisplayName("First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "* Surname  required")]
        //   [DisplayName("Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }
        //    [DisplayName("Phone Number")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
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
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please select role")]

        public int UserType { get; set; }

        public string CreatedBy { get; set; }
        public string RoleId { get; set; }
        public string MiddleName { get; set; }

        public bool IsMobileRegistration { get; set; } = false;

        public string DealerId { get; set; }
    }


    public class UpdateUserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "* First name required")]
        // [DisplayName("First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "* Surname  required")]
        //   [DisplayName("Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }
        //    [DisplayName("Phone Number")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string Email{ get; set; }
        public string MiddleName { get; set; }

        public bool IsActive { get; set; }

        public string DealerId { get; set; }

        public decimal Balance { get; set; }

    }



    public class UserViewModelMobile
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        [Required(ErrorMessage = "* Surname  required")]
        //   [DisplayName("Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }
        //    [DisplayName("Phone Number")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string MiddleName { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Dealer id is Required")]
        public string DealerId { get; set; }

        //public bool IsMobileRegistration { get; set; } = false;
    }

    public class OTPPayload
    {
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public bool isRegistration { get; set; } = false;
    }
}
