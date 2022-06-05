using Castle.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Services.Role.DTO
{
    public class DynamicMenu
    {
        private readonly IConfiguration _configuration;
        public DynamicMenu(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static List<SidebarMenuViewModel> GenerateUrl(List<Landyvest.Data.Models.Indentity.Permission> parentMenus, List<Landyvest.Data.Models.Indentity.Permission> menus, StringBuilder sb)
        {
            List<SidebarMenuViewModel> sidebarMenus = null;
            if (parentMenus.Count > 0)
            {
                string alias = "";

                // Looping through Parent Menu
                sidebarMenus = new List<SidebarMenuViewModel>();

                foreach (var menu in parentMenus)
                {
                    // Get all Menu Components into variables:
                    string url = menu.Url;
                    string menuText = menu.PermissionName;
                    string icon = menu.Icon;


                    // Get out the current Parent menu Id & ParentId inside the loop
                    var pid = menu.ID;
                    var parentId = menu.ParentId;
                    //var subMenus = menus.FindAll(x => x.ParentId == pid);

                    sidebarMenus.Add(new SidebarMenuViewModel
                    {

                        Icon = menu.Icon,
                        MenuText = menu.PermissionName,
                        Alias = alias,
                        Url = menu.Url,
                        PID = menu.ID.ToString(),
                        ParentId = menu.ParentId.ToString(),
                        SubMenus = menus.FindAll(x => x.ParentId == pid).ToList(),


                    }); ;


                }
            }
            return sidebarMenus;
        }


    }
}
