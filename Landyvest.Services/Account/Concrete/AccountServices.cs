using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data;
using Landyvest.Data.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Landyvest.Data.Payload;
using Landyvest.Services.Account.DTO;
using Landyvest.Services.Account.Interface;
using Landyvest.Services.AuditLog.Concrete;
using Landyvest.Services.DataAccess;
using Landyvest.Services.Permission.Interface;
using Landyvest.Services.Role.DTO;
using Landyvest.Services.Role.Interface;
using Landyvest.Utilities.Common;

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Landyvest.Services.Account.Concrete
{
    public class AccountServices :IAccount
    {
        private readonly LandyvestAppContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        protected readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRole _roleSignManager;
        private readonly IRolePermission _rolePermissionManager;
        private readonly IActivityLog _activityRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountServices(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, IConfiguration configuration,
            IRole roleSignManager, IRolePermission rolePermissionManager,
            LandyvestAppContext context, IActivityLog activityRepo, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
           _roleSignManager = roleSignManager;
             _rolePermissionManager = rolePermissionManager;
            _context = context;
            _roleManager = roleManager;
            _activityRepo = activityRepo;
            _httpContextAccessor = httpContextAccessor;
        }

          public Task<MessageOut> Register(AdminUserSettingViewModel payload)
        {
            throw new NotImplementedException();
        }

        #region Seceret
        public LoginDataDTO PrepareUserMenu(ApplicationUser user, List<ApplicationRoleViewModel> roles)
        {
            var model = new LoginDataDTO();

            var role = roles.ToList().Where(r => r.ID.Equals(user.RoleId ?? "")).FirstOrDefault();

            if (role != null)
            {
                var roleId = role.ID;
                // Get User's Role Permissions:
                AccessDataLayer accessDataLayer = new AccessDataLayer(_context);
                DBManager dBManager = new DBManager(_context);

                var parameters = new List<IDbDataParameter>();


                parameters.Add(dBManager.CreateParameter("@RoleId", user.RoleId, DbType.String));
                DataTable menuList = accessDataLayer.
                    FetchRolePermissionsByRoleId(parameters.ToArray(), Landyvest.Utilities.Common.StoredProcedureName.FetchUserPermissionAndRole);

                List<Landyvest.Data.Models.Indentity.Permission> menus = new List<Landyvest.Data.Models.Indentity.Permission>();

                foreach (DataRow dataRow in menuList.Rows)
                {
                    var permission = new Landyvest.Data.Models.Indentity.Permission
                    {
                        CreatedBy = dataRow["CreatedBy"].ToString(),
                        Icon = dataRow["IconName"].ToString(),
                        ParentId = Convert.ToInt32(dataRow["ParentId"]),
                        PermissionCode = dataRow["PermissionCode"].ToString(),
                        PermissionName = dataRow["PermissionName"].ToString(),
                        Url = dataRow["Url"].ToString(),
                        ID = Convert.ToInt64(dataRow["ID"])

                    };

                    menus.Add(permission);

                }


                var parentMenus = menuList.Select("ParentId = 0").ToList(); ;
                var SelectedParentMenus = new List<Landyvest.Data.Models.Indentity.Permission>();
                List<Landyvest.Data.Models.Indentity.Permission> SelectedParentMenusOrder = new List<Landyvest.Data.Models.Indentity.Permission>();

                foreach (DataRow dataRow in parentMenus)
                {
                    var permission = new Landyvest.Data.Models.Indentity.Permission
                    {
                        //  CreatedBy = dataRow["CreatedBy"].ToString();
                        Icon = dataRow["IconName"].ToString(),
                        ParentId = Convert.ToInt32(dataRow["ParentId"]),
                        PermissionCode = dataRow["PermissionCode"].ToString(),
                        PermissionName = dataRow["PermissionName"].ToString(),
                        Url = dataRow["Url"].ToString(),
                        ID = Convert.ToInt64(dataRow["PermissionId"])
                    };

                    SelectedParentMenus.Add(permission);

                }


                SelectedParentMenusOrder = SelectedParentMenus.OrderBy(a => a.PermissionName).ToList();

                var sb = new StringBuilder();
               var sidebarMenus = DynamicMenu.GenerateUrl(SelectedParentMenusOrder, menus, sb);


           
                model.Menus = JsonConvert.SerializeObject(menus);
                model.RoleName = role.RoleName.ToLower();
                model.UserEmail = user.Email;
                model.sideVarMenu = sidebarMenus;
               
                return model;
            }

            return model;
        }
        private async Task PrepareSignInClaims(ApplicationUser user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);

            var _claims = userClaims.ToList();
            string roles = string.Empty;
            IList<string> role = await _userManager.GetRolesAsync(user);

           
            string USERPERMISSION = SetUserPermissions(user.Id);

            var RoleName = await _roleManager.FindByIdAsync(user.RoleId);


            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.FirstName + ' '  + user.LastName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, RoleName.Name),
                            new Claim(ClaimTypes.Surname, user.LastName),
                            new Claim(ClaimTypes.MobilePhone, user.MobileNumber),
                            new Claim(ClaimTypes.GivenName, user.FirstName + ' '  + user.LastName),
                            new Claim(ClaimTypes.Anonymous, RoleName.RoleName),
                            new Claim(ClaimTypes.PostalCode, RoleName.Id)

                        }.Union(userClaims);

            foreach (var r in role)
            {
                claims = claims.Append(new Claim(ClaimTypes.Role, r));
            }

            _claims = claims.ToList();

        
            await _signInManager.SignInWithClaimsAsync(user, false, _claims);
        }

        private string SetUserPermissions(string UserId)
        {
            try
            {

                AccessDataLayer accessDataLayer = new AccessDataLayer(_context);
                DBManager dBManager = new DBManager(_context);

                var parameters = new List<IDbDataParameter>();

                parameters.Add(dBManager.CreateParameter("@UserId", UserId, DbType.String));
                DataTable mUserPremissionRolemodel = accessDataLayer.FetchUserPermissionAndRole(parameters.ToArray(), "FetchUserPermissionAndRole");



                string userPermissions = "";
                if (mUserPremissionRolemodel != null)
                {
                    int i = 0;
                    foreach (DataRow item in mUserPremissionRolemodel.Rows)
                    {
                        i = i + 1;
                        if (i == 0)
                        {
                            userPermissions = item["PermissionCode"] + ",";
                        }
                        else
                        {
                            userPermissions = userPermissions + item["PermissionCode"].ToString() + ",";
                        }
                    }
                }
                return userPermissions;
            }
            catch (Exception ex)
            {
                //  _log.Error(ex);
                return string.Empty;
            }
        }

        public Task<MessageOut> Login(UserLoginPayload payload)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
