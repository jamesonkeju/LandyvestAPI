using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data.Models;
using Landyvest.Data.Payload;
using Landyvest.Services.Role.DTO;

namespace Landyvest.Services.Role.Interface
{
    public interface IRole
    {
        Task<List<ApplicationRole>> GetRoles(RoleFilter payload);

        Task<ApplicationRole> GetRoleById(string RoleId);

        List<string> GetUserRoleByUserId(string UserId);

        Task<string> CreateRole(ApplicationRoleViewModel obj, string ControllerName, string ActionName);
        Task<string> UpdateRole(ApplicationRoleViewModel obj, string ControllerName, string ActionName);
        Task<bool> DeleteRole(string id, string ModifiedBy, string ControllerName, string ActionName);

        Task<string> MapUsersToRoles(string UserId, List<ApplicationRole> userRoles, string ControllerName, string ActionName);

        List<string> FetchUserRoleByUserId(string UserId);



    }
}
