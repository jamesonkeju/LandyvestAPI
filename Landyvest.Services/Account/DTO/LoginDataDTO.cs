using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Services.Role.DTO;

namespace Landyvest.Services.Account.DTO
{
   public class LoginDataDTO
    {
        public List<SidebarMenuViewModel> sideVarMenu { get; set; }
        public string UserEmail { get; set; }
        public string RoleName { get; set; }
        public string Menus { get; set; }
    }

    
}
