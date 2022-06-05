using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Models.Indentity
{
    public class AuthModel
    {
        public string RoleId { get; set; }
        public string MenuString { get; set; }
        public string Menus { get; set; }
        public string token { get; set; }
       public DateTime ExpiryTime { get; set; }
        public string UserEmail { get; set; }
        public string RoleName { get; set; }
        public string Role { get; set; }
        public int RolesId { get; set; }
        public ApplicationUser UserInformation { get; set; }


    }
}
