using Microsoft.AspNetCore.Hosting;
using Landyvest.Data;
using Landyvest.Services.SystemSetting.Interface;
using Microsoft.Extensions.Logging;
using Landyvest.Services.AuditLog.Concrete;
using Microsoft.Extensions.Configuration;
using Landyvest.Utilities.Common;
using System.Threading.Tasks;
using Landyvest.Services.Permission.DTO;
using Landyvest.Data.Models.Domains;
using Newtonsoft.Json;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Landyvest.Data.Models.Indentity;
using Landyvest.Services.CommonRoute;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Landyvest.Services.DataAccess;
using System.Data;

namespace Landyvest.Services.Permission.Concrete
{
    public class PermissionServices : IPermission
    {
        private readonly LandyvestAppContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<PermissionServices> _logger;
        private readonly IActivityLog _activityLog;
        private readonly IHostingEnvironment _env;
        private readonly ISystemSetting _systemSettingManager;
        private readonly IConfiguration _configuration;

        public static bool _mailSent;
        private ICommonRoute _commonServices;


        public PermissionServices(LandyvestAppContext context, ILogger<PermissionServices> logger, IActivityLog activityLog, IConfiguration config,
          IConfiguration iConfig, IHostingEnvironment env, ISystemSetting systemSettingManager, ICommonRoute commonServices)
        {
            _context = context;
            _logger = logger;
            _activityLog = activityLog;
            _config = config;
            _configuration = iConfig;
            _commonServices = commonServices;
        }



        public async Task<PermissionViewModel> GetPermissionByID(long RoleId)
        {

            var model = new PermissionViewModel();
            var parentList = await GetAllParent(true);


            try
            {

                return await _context.Permissions
                    .Where(x => x.ID == RoleId)

                    .Select(modelResponse => new PermissionViewModel
                    {
                        PermissionName = modelResponse.PermissionName,
                        ID = modelResponse.ID,
                        PermissionCode = modelResponse.PermissionCode,

                        Url = modelResponse.Url,
                        ParentId = modelResponse.ParentId,

                        CreatedDate = DateTime.Now,
                        IsActive = (bool)modelResponse.IsActive,
                        CreatedBy = modelResponse.CreatedBy,
                        Icon = modelResponse.Icon,
                        ParentMenus = parentList.Select(u => new SelectListItem { Text = u.PermissionName, Value = u.ID.ToString() }).ToList()
                    })
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
               
                return model;
            }
        }

        public async Task<MessageOut> AddUpdatePermission(PermissionViewModel payload)
        {
            var op = new OperationReqObj(payload.ID);
            try
            {
                var inComing = payload.PermissionName.Trim();
                var parentId = payload.ParentId.GetValueOrDefault();
                var url = payload.Url.Trim();


                string msg = string.Empty;
                var roleExists = new List<Landyvest.Data.Models.Indentity.Permission>();

                #region Initialization:

                var alog = new ActivityLog
                {
                    ModuleName = payload.ControllerName,
                    ModuleAction = payload.ActionName,
                    UserId = payload.CreatedBy,
                    Record = payload != null ? JsonConvert.SerializeObject(payload) : "N/A",
                    CreatedDate = DateTime.Now,
                    CreatedBy = payload.CreatedBy,
                    IPAddress = IPAddressUtil.GetLocalIPAddress()
                };

                #endregion

                #region Duplicate:

                var check = payload.ID > 0
                    ? await _context.Permissions.FirstOrDefaultAsync(x =>
                        (x.PermissionName == payload.PermissionName.Trim()) && x.PermissionCode == payload.PermissionCode  && x.ID != payload.ID)
                    : await _context.Permissions.FirstOrDefaultAsync(x =>
                        (x.PermissionName == payload.PermissionName.Trim()));

                if (check != null && check.ID > 0)
                    return _commonServices.OutputMessage(false,
                        CommonResponseMessage.RecordExisting.Replace("{0}", " role  name " + payload.PermissionName));

                #endregion

                #region Process Request:

                var data = await _context.Permissions.FirstOrDefaultAsync(x => x.ID == payload.ID);

                if (data != null && data.ID > 0)
                {
                    // log the old data 
                    alog.OldRecord = data != null ? JsonConvert.SerializeObject(data) : "N/A";


                    data.PermissionName = payload.PermissionName.ToString().Trim();
                    data.PermissionCode = payload.PermissionCode.ToString().Trim();
                    data.ModifiedBy = payload.ModifiedBy;
                    data.LastModified = DateTime.Now;

                }
                else
                {
                    var addData = new Landyvest.Data.Models.Indentity.Permission
                    {
                        PermissionName = inComing,
                        Url = url,
                        ParentId = parentId,
                        Icon = (string.IsNullOrEmpty(payload.Icon) ? "" : payload.Icon.Trim()),
                        CreatedBy = payload.CreatedBy,
                        PermissionCode = payload.PermissionCode,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                    };
                    await _context.Permissions.AddAsync(addData);
                }

                if (!await _context.TrySaveChangesAsync(_logger))
                {
                    #region Activity Logs Failed

                    alog.Description = op.fail_request_type + payload.PermissionName;
                    alog.ActionType = op.operation_type;
                    await _activityLog.CreateActivityLog(alog);
                    #endregion

                    return _commonServices.OutputMessage(false, op.fail_record.Replace("{0}", payload.PermissionName));
                }

                #region Activity Logs Successful
                alog.Description = op.created_request_type + payload.PermissionName;
                alog.ActionType = op.operation_type;
                await _activityLog.CreateActivityLog(alog);
                #endregion

                return _commonServices.OutputMessage(true, op.save_record.Replace("{0}", payload.PermissionName));

                #endregion
            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return _commonServices.OutputMessage(false,
                    string.Format(CommonResponseMessage.InternalError, op.operation_type,
                        payload.PermissionName, ex.Message));
            }
        }

        public async Task<MessageOut> DeletePermission(DeletePermissionViewModel payload)
        {
            var op = new OperationReqObj(payload.ID);
            try
            {
                

                string msg = string.Empty;
                var roleExists = new List<Landyvest.Data.Models.Indentity.Permission>();

                #region Initialization:

                var alog = new ActivityLog
                {
                    ModuleName = payload.ControllerName,
                    ModuleAction = payload.ActionName,
                    UserId = payload.ModifiedBy,
                    Record = payload != null ? JsonConvert.SerializeObject(payload) : "N/A",
                    CreatedDate = DateTime.Now,
                    CreatedBy = payload.ModifiedBy,
                    IPAddress = IPAddressUtil.GetLocalIPAddress()
                };

                #endregion

               

                #region Process Request:

                var data = await _context.Permissions.FirstOrDefaultAsync(x => x.ID == payload.ID);

                if (data != null)
                {

                   
                    // log the old data 
                    alog.OldRecord = data != null ? JsonConvert.SerializeObject(data) : "N/A";


                    data.PermissionName = data.PermissionName.ToString().Trim();
                    data.PermissionCode = data.PermissionCode.ToString().Trim();
                    data.ModifiedBy = payload.ModifiedBy;
                    data.LastModified = DateTime.Now;
                    data.IsActive = false;
                    data.LastModified = DateTime.Now;
                    data.IsDeleted = true;
                }
                else
                {

                }

                if (!await _context.TrySaveChangesAsync(_logger))
                {
                    #region Activity Logs Failed

                    alog.Description = op.fail_request_type + data.PermissionName;
                    alog.ActionType = op.operation_type;
                    await _activityLog.CreateActivityLog(alog);
                    #endregion

                    return _commonServices.OutputMessage(false, op.fail_record.Replace("{0}", data.PermissionName));
                }

                #region Activity Logs Successful
                alog.Description = op.created_request_type + data.PermissionName;
                alog.ActionType = op.operation_type;
                await _activityLog.CreateActivityLog(alog);
                #endregion

                return _commonServices.OutputMessage(true, op.delete_record.Replace("{0}", data.PermissionName));

                #endregion
            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return _commonServices.OutputMessage(false,
                    string.Format(CommonResponseMessage.InternalError, op.operation_type,
                        "", ex.Message));
            }
        }

        public async Task<List<PermissionViewModel>> GetAllPermissions(bool? status)
        {



            List<PermissionViewModel> modelResponse = new List<PermissionViewModel>();

            var  model = new PermissionViewModel();

            try
            {
                var parentList = await GetAllParent(true);

                if (status == null)
                {

                    return await _context.Permissions.Where(a => a.IsDeleted != true)
                    .Select(modelResponse => new PermissionViewModel
                    {
                        PermissionName = modelResponse.PermissionName,
                        ID = modelResponse.ID,
                        PermissionCode = modelResponse.PermissionCode,
                        ActionTitle = "",
                        Url = modelResponse.Url,
                        ParentId = modelResponse.ParentId,
                        CreatedDate = DateTime.Now,
                        
                        ParentMenus = parentList.ToList().Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = u.PermissionName, Value = u.ID.ToString() }).ToList(),

                        IsActive = (bool)modelResponse.IsActive,
                        CreatedBy = modelResponse.CreatedBy
                       
                    })


                    .ToListAsync();
                }
                else
                {
                    return await _context.Permissions
                    .Where(x => x.IsActive == status)
                    .Select(modelResponse => new PermissionViewModel
                    {
                        PermissionName = modelResponse.PermissionName,
                        ID = modelResponse.ID,
                        PermissionCode = modelResponse.PermissionCode,
                        ActionTitle = "",
                        Url = modelResponse.Url,
                        ParentId = modelResponse.ParentId,
                        CreatedDate = DateTime.Now,
                        ParentMenus = parentList.ToList().Select(u => new SelectListItem { Text = u.PermissionName, Value = u.ID.ToString() }).ToList(),

                        IsActive = (bool)modelResponse.IsActive,
                        CreatedBy = modelResponse.CreatedBy,
                       

                    })
                    .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
              
                modelResponse.Add(model);
                return modelResponse;
            }
        }

        public async Task<List<PermissionViewModel>> GetAllParent(bool? status)
        {



            var   modelResponse = new List<PermissionViewModel>();

            var model = new PermissionViewModel();

            try
            {

                if (status == null)
                {

                    return await _context.Permissions.Where(a => a.ParentId == CommonResponseMessage.PermissionParentId && a.IsDeleted != true).OrderBy(a => a.PermissionName)
                    .Select(modelResponse => new  PermissionViewModel
                    {
                        PermissionName = modelResponse.PermissionName,
                        ID = modelResponse.ID,
                       
                    })


                    .ToListAsync();
                }
                else
                {
                    return await _context.Permissions.Where(a => a.ParentId == CommonResponseMessage.PermissionParentId && a.IsActive == status)
                     .Select(modelResponse => new  PermissionViewModel
                     {
                         PermissionName = modelResponse.PermissionName,
                         ID = modelResponse.ID
                        
                     })


                     .ToListAsync();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
               
                modelResponse.Add(model);
                return modelResponse;
            }
        }

        public async Task<bool> UpdateRolePermission(UpdateRolePermissionViewModel payload)
        {
            bool  status = false;
            var rolePermissions = new List<RolePermission>();

            #region Initialization:

            var alog = new ActivityLog
            {
                ModuleName = payload.Controller,
                ModuleAction = payload.ActionName,
                UserId = payload.UserId,
                Record = payload != null ? JsonConvert.SerializeObject(payload) : "N/A",
                CreatedDate = DateTime.Now,
                CreatedBy = payload.UserId,
                IPAddress = IPAddressUtil.GetLocalIPAddress()
            };

            #endregion


            try
            {


                foreach (var role in payload.roles)
                {
                    var rolePermission = new RolePermission
                    {
                        PermissionId = Convert.ToInt64(role),
                        RoleId = payload.RoleId
                    };
                    rolePermissions.Add(rolePermission);
                }


                // call a store procedure
                DBManager dBManager = new DBManager(_context);
                var parameters = new List<IDbDataParameter>();
                parameters.Add(dBManager.CreateParameter("@RoleId", payload.RoleId, DbType.AnsiString));
                AccessDataLayer dbManager = new AccessDataLayer(_context);
                dbManager.DeletePermissionByRoleID(parameters.ToArray(), "DeletePermissionByRoleID");



                _context.RolePermissions.AddRange(rolePermissions);

                if (await _context.SaveChangesAsync() > 0)
                {
                    string RoleName = _context.Roles.Where(a => a.Id == payload.RoleId).FirstOrDefault().RoleName;

                    string permissionName = "";

                    foreach (var item in rolePermissions)
                    {
                        var parametersList = new List<IDbDataParameter>();
                        parametersList.Add(dBManager.CreateParameter("@permissionId", item.ID, DbType.Int64));

                        DataTable dt = dBManager.GetDataTable("getPermission", CommandType.StoredProcedure, parametersList.ToArray());

                        if (dt.Rows.Count > 0)
                        {
                            permissionName = permissionName + dt.Rows[0]["PermissionName"].ToString() + "|";
                        }

                    }

                    _activityLog.CreateActivityLog(string.Format("Mapped Permission to Role  Portal : {0}", RoleName), payload.Controller, payload.ActionName, payload.UserId, permissionName,null);

                    status = true;
                }
                else
                {
                    status = false;
                }
                return status;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return status;
            }
        }
    }
}
