using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Services.Permission.DTO
{
    public class PermissionViewModel 
    {

        public  long ID { get; set; }

        [Required(ErrorMessage = "Permission Name is required")]
        public string PermissionName { get; set; }

        //[Required(ErrorMessage = "Code is required")]
        public string PermissionCode { get; set; }
        public string Icon { get; set; }
        [Required(ErrorMessage = "Url is required")]
        public string Url { get; set; }
        public int? ParentId { get; set; }

        public string Parent { get; set; }

        public string Action { get; set; }
        public string ActionTitle { get; set; }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ParentMenus { get; set; }

        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class DeletePermissionViewModel
    {
        public long ID { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class UpdateRolePermissionViewModel
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }
        public List<string> roles { get; set; }
    }
}
