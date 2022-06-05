using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data;
using Landyvest.Data.Models;
using Landyvest.Data.Payload;
using Landyvest.Services.AuditLog.Concrete;
using Landyvest.Services.CommonRoute;
using Landyvest.Services.DataAccess;
using Landyvest.Services.Role.DTO;
using Landyvest.Services.Role.Interface;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.Role.Concrete
{
    public class RoleService : IRole
    {
        private LandyvestAppContext _context;
        private IActivityLog _activityRepo;
        private ILogger<RoleService> _logger;
        private ICommonRoute _commonServices;

        public RoleService(LandyvestAppContext context, ILogger<RoleService> logger, IActivityLog activityLogService, ICommonRoute commonServices)
        {
            _context = context;
            _activityRepo = activityLogService;
            _logger = logger;
            _commonServices = commonServices;
        }

        public async Task<string> CreateRole(ApplicationRoleViewModel obj, string ControllerName, string ActionName)
        {
            string status = "";
            try
            {

                var checkexist = await _context.ApplicationRoles
                    .AnyAsync(x => x.RoleName.ToUpper() == obj.RoleName.ToUpper()
                   );

                if (checkexist == false)
                {
                    ApplicationRole newrecord = new ApplicationRole
                    {
                        RoleName = obj.RoleName,
                        RoleDescription = obj.RoleDescription,
                        Name = obj.RoleName,
                        IsSysAdmin = obj.IsSysAdmin,
                        CreatedDate = DateTime.Now,
                        CreatedBy = obj.CreatedBy,
                        IsActive = true,
                        Id = GenericUtil.generatePrimaryId(),
                    };

                    _context.ApplicationRoles.Add(newrecord);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        _activityRepo.CreateActivityLog(string.Format("Created Portal permission with Name : {0}", newrecord.RoleName),
                            ControllerName, ActionName, obj.CreatedBy, newrecord, null);

                        status = CommonResponseMessage.RecordSaved;
                        return status;
                    }
                    else
                    {
                        status = CommonResponseMessage.RecordNotSaved;
                        return status;
                    }
                }
                status = string.Format(CommonResponseMessage.RecordExistBefore, obj.RoleName.ToUpper());
                return status;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                status = CommonResponseMessage.RecordNotSaved;
                return status;
            }
        }

        public Task<bool> DeleteRole(string id, string ModifiedBy, string ControllerName, string ActionName)
        {
            throw new NotImplementedException();
        }

        public List<string> FetchUserRoleByUserId(string UserId)
        {
            try
            {
                List<string> roleList = new List<string>();
               
                var roles = _context.ApplicationUsers.Where(x => x.Id == UserId).ToList();

                foreach(var role in roles)
                {
                    roleList.Add(role.RoleId);
                }

                return roleList;


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ApplicationRole> GetRoleById(string RoleId)
        {
            var bind = new ApplicationRole();

            try
            {
               return  _context.ApplicationRoles.Where(a => a.Id == RoleId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return bind;
            }
        }

        public async Task<List<ApplicationRole>> GetRoles(RoleFilter payload)
        {
            var bind = new List<ApplicationRole>();

            try
            {
                
                
                var qry = _context.ApplicationRoles.AsQueryable();
                var data = new List<ApplicationRole>();

                if (payload != null && !string.IsNullOrEmpty(payload.Id))
                {
                    qry = qry.Where(p => p.Id == payload.Id).AsQueryable();
                     data = qry.OrderBy(p => p.RoleName).Take(payload.pageSize).Skip((payload.pageNumber - 1) * payload.pageSize).ToList();
                }

                if (payload != null && !string.IsNullOrEmpty(payload.RoleName))
                {
                    qry = qry.Where(p => p.Name.ToUpper() == payload.RoleName.ToUpper()).AsQueryable();
                    data = qry.OrderBy(p => p.RoleName).Take(payload.pageSize).Skip((payload.pageNumber - 1) * payload.pageSize).ToList();
                }
                else
                {
                    data = qry.OrderBy(p => p.Id).ToList();
                }

               
                
                return data;
            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return bind;
            }
        }


        public List<string> GetUserRoleByUserId(string UserId)
        {
            List<string> roleList = new List<string>();

            string status = "";
            try
            {

                // call store procedure to query for the registerd roles 

                DBManager dBManager = new DBManager(_context);
                var parameters = new List<IDbDataParameter>();
                parameters.Add(dBManager.CreateParameter("@UserId", UserId, DbType.AnsiString));

                DataTable getRecords = dBManager.GetDataTable("GetUserRoleByUserId", CommandType.StoredProcedure, parameters.ToArray());


                foreach (DataRow row in getRecords.Rows)
                {
                    roleList.Add(row["RoleId"].ToString());
                }
                return roleList;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }
        }

        

        public async Task<string> MapUsersToRoles(string UserId, List<ApplicationRole> userRoles, string ControllerName, string ActionName)
        {
            string status = "";
            try
            {
                List<ApplicationUserRole> roles = new List<ApplicationUserRole>();

                foreach (var item in userRoles)
                {
                    ApplicationUserRole applicationUserRole = new ApplicationUserRole
                    {
                        RoleId = item.Id,
                        UserId = UserId
                    };
                    roles.Add(applicationUserRole);
                }


                _context.ApplicationUserRoles.AddRange(roles);

                if (await _context.SaveChangesAsync() > 0)
                {
                    status = CommonResponseMessage.RecordSaved;
                    
                  await  _activityRepo.CreateActivityLogAsync(string.Format("Mapped User To Role  Name : {0}", roles.FirstOrDefault().Role),
                         ControllerName, ActionName, UserId, roles,null);

                    return status;
                }
                else
                {
                    status = CommonResponseMessage.RecordNotSaved;
                    return status;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                status = CommonResponseMessage.RecordNotSaved;
                return status;
            }
        }

        public async Task<string> UpdateRole(ApplicationRoleViewModel obj, string ControllerName, string ActionName)
        {
            string status = "";
            try
            {


                var checkexist = await _context.ApplicationRoles
                   .AnyAsync(x => x.Id != obj.ID.ToString() &&
                   (x.RoleName.ToUpper() == obj.RoleName.ToUpper()));

                if (checkexist == false)
                {
                    var model = await _context.ApplicationRoles
                    .FirstOrDefaultAsync(x => x.Id == obj.ID);
                    model.RoleName = obj.RoleName;
                    model.RoleDescription = obj.RoleDescription;
                    model.ModifiedBy = obj.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = obj.IsActive;
                    model.Name = obj.RoleName;
                    model.IsSysAdmin = obj.IsSysAdmin;
                    model.Name = obj.RoleName;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        status = CommonResponseMessage.SuccessfulAndUpdated;
                        _activityRepo.CreateActivityLog(string.Format("Updated Portal Role with Name:{0}", model.Name), ControllerName, ActionName, obj.CreatedBy, checkexist, null);

                        return status;
                    }
                    else
                    {
                        status = CommonResponseMessage.RecordNotSaved;
                        return status;
                    }
                }
                status = string.Format(CommonResponseMessage.RecordExistBefore, obj.RoleName.ToUpper());
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                status = CommonResponseMessage.RecordNotSaved;
                return status;
            }
        }
    }
}
