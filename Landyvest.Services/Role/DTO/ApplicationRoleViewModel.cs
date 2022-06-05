using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Services.Role.DTO
{
    public class ApplicationRoleViewModel
    {
        public string ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsActive { get; set; }
        public string Name { get; set; }
        public string DataIntegrity { get; set; }
        public string ResponseMessage { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsSysAdmin { get; set; }


    }

    public class SidebarMenuViewModel
    {
        public string MenuText { get; set; }
        public string Alias { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string PID { get; set; }
        public string ParentId { get; set; }
        public List<Landyvest.Data.Models.Indentity.Permission> SubMenus { get; set; }

        public string action { get; set; }
        public string controller { get; set; }


    }
}
