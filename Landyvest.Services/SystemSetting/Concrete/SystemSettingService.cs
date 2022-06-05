


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
using Landyvest.Services.Role.DTO;

namespace Landyvest.Services.SystemSetting.Concrete
{
    public class SystemSettingService : ISystemSetting
    {
        private readonly LandyvestAppContext _context;
        private readonly ILogger<SystemSettingService> _logger;
        private readonly IActivityLog _activityLog;


        public static bool _mailSent;
        private ICommonRoute _commonServices;


        public SystemSettingService(LandyvestAppContext context, ILogger<SystemSettingService> logger, IActivityLog activityLog, ICommonRoute commonServices)
        {
            _context = context;
            _logger = logger;
            _activityLog = activityLog;
            _commonServices = commonServices;
        }



        public async Task<SystemSettingViewModel> GetSystemSettingById(long Id)
        {

            var model = new SystemSettingViewModel();
            try
            {

                return await _context.SystemSettings
                    .Where(x => x.ID == Id)

                    .Select(modelResponse => new SystemSettingViewModel
                    {
                        ItemName = modelResponse.ItemName,
                        ID = modelResponse.ID,
                        ItemValue = modelResponse.ItemValue,
                        CreatedDate = DateTime.Now,
                        IsActive = (bool)modelResponse.IsActive,
                        CreatedBy = modelResponse.CreatedBy,
                        LookUpCode = modelResponse.LookUpCode
                    }).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return model;
            }
        }

        public async Task<MessageOut> CreateUpdateSystemSetting(SystemSettingViewModel payload)
        {
            var op = new OperationReqObj(payload.ID);
            try
            {
               

                string msg = string.Empty;
                var extendingRecord = new List<Landyvest.Data.Models.Domains.SystemSetting>();

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
                    ? await _context.SystemSettings.FirstOrDefaultAsync(x =>
                        (x.LookUpCode == payload.LookUpCode.Trim())  && x.ItemValue == payload.ItemValue && x.ID != payload.ID)
                    : await _context.SystemSettings.FirstOrDefaultAsync(x =>
                        (x.LookUpCode == payload.LookUpCode.Trim()));

                if (check != null && check.ID > 0)
                    return _commonServices.OutputMessage(false,
                        CommonResponseMessage.RecordExisting.Replace("{0}", " action  name or code exist " + payload.ActionName+ "[" + payload.ItemValue+"]"));

                #endregion

                #region Process Request:

                var data = await _context.SystemSettings.FirstOrDefaultAsync(x => x.ID == payload.ID);

                if (data != null && data.ID > 0)
                {
                    // log the old data 
                    alog.OldRecord = data != null ? JsonConvert.SerializeObject(data) : "N/A";
                    data.ItemValue = payload.ItemValue.ToString().Trim();
                    data.ItemName = payload.ItemName.ToString().Trim();
                    data.LookUpCode = payload.LookUpCode.ToString().Trim();
                    data.ModifiedBy = payload.ModifiedBy;
                    data.LastModified = DateTime.Now;
                }
                else
                {
                    var addData = new Landyvest.Data.Models.Domains.SystemSetting
                    {
                        LookUpCode = payload.LookUpCode,
                        ItemName = payload.ItemName,
                        ItemValue = payload.ItemValue,
                        CreatedDate = DateTime.Now,
                        CreatedBy = payload.CreatedBy,
                        IsActive = payload.IsActive,
                        IsDeleted = false,
                     
                    };
                    await _context.SystemSettings.AddAsync(addData);
                }

                if (!await _context.TrySaveChangesAsync(_logger))
                {
                    #region Activity Logs Failed

                    alog.Description = op.fail_request_type + payload.ItemName;
                    alog.ActionType = op.operation_type;
                    await _activityLog.CreateActivityLog(alog);
                    #endregion

                    return _commonServices.OutputMessage(false, op.fail_record.Replace("{0}", payload.ItemValue));
                }

                #region Activity Logs Successful
                alog.Description = op.created_request_type + payload.ItemValue;
                alog.ActionType = op.operation_type;
                await _activityLog.CreateActivityLog(alog);
                #endregion

                return _commonServices.OutputMessage(true, op.save_record.Replace("{0}", payload.ItemValue));

                #endregion
            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return _commonServices.OutputMessage(false,
                    string.Format(CommonResponseMessage.InternalError, op.operation_type,
                        payload.ItemValue, ex.Message));
            }
        }

        public async Task<MessageOut> DeleteSystemSetting(DeleteSystemSettingViewModel payload)
        {
            var op = new OperationReqObj(payload.ID);
            try
            {

                string msg = string.Empty;
                var roleExists = new List<Landyvest.Data.Models.Domains.SystemSetting>();

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

                var data = await _context.SystemSettings.FirstOrDefaultAsync(x => x.ID == payload.ID);

                if (data != null)
                {


                    // log the old data 
                    alog.OldRecord = data != null ? JsonConvert.SerializeObject(data) : "N/A";

                    data.LookUpCode = data.LookUpCode.ToString().Trim();
                    data.ItemValue = data.ItemValue.ToString().Trim();
                    data.ItemName = data.ItemName.ToString().Trim();
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

                    alog.Description = op.fail_request_type + data.ItemName;
                    alog.ActionType = op.operation_type;
                    await _activityLog.CreateActivityLog(alog);
                    #endregion

                    return _commonServices.OutputMessage(false, op.fail_record.Replace("{0}", data.ItemName));
                }

                #region Activity Logs Successful
                alog.Description = op.created_request_type + data.ItemName;
                alog.ActionType = op.operation_type;
                await _activityLog.CreateActivityLog(alog);
                #endregion

                return _commonServices.OutputMessage(true, op.delete_record.Replace("{0}", data.ItemName));

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

        public async Task<List<SystemSettingViewModel>> GetSystemSettings()
        {



            var  modelResponse = new List<SystemSettingViewModel>();

            var model = new SystemSettingViewModel();

            try
            {
                    return await _context.SystemSettings.Where(a => a.IsDeleted != true)
                    .Select(modelResponse => new SystemSettingViewModel
                    {
                        LookUpCode = modelResponse.LookUpCode,
                        ID = modelResponse.ID,
                        ItemName = modelResponse.ItemName,
                        CreatedDate = DateTime.Now,
                        ItemValue = modelResponse.ItemValue,
                        IsActive = (bool)modelResponse.IsActive,
                        CreatedBy = modelResponse.CreatedBy,
                        ModifiedBy = modelResponse.ModifiedBy,
                        LastModified = modelResponse.LastModified                       
                    })


                    .ToListAsync();
                
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                modelResponse.Add(model);
                return modelResponse;
            }
        }

        public async Task<List<SystemSettingViewModel>> GetSystemSettingByLookUpCodeAsync(string LookUpCode)
        {



            var modelResponse = new List<SystemSettingViewModel>();

            var model = new SystemSettingViewModel();

            try
            {
                return await _context.SystemSettings.Where(a => a.IsDeleted != true && a.LookUpCode ==LookUpCode)
                .Select(modelResponse => new SystemSettingViewModel
                {
                    LookUpCode = modelResponse.LookUpCode,
                    ID = modelResponse.ID,
                    ItemName = modelResponse.ItemName,
                    CreatedDate = DateTime.Now,
                    ItemValue = modelResponse.ItemValue,
                    IsActive = (bool)modelResponse.IsActive,
                    CreatedBy = modelResponse.CreatedBy,
                    ModifiedBy = modelResponse.ModifiedBy,
                    LastModified = modelResponse.LastModified
                }).ToListAsync();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                modelResponse.Add(model);
                return modelResponse;
            }
        }


        public  List<SystemSettingViewModel> GetSystemSettingByLookUpCode(string LookUpCode)
        {



            var modelResponse = new List<SystemSettingViewModel>();

            var model = new SystemSettingViewModel();

            try
            {
                var result = _context.SystemSettings.Where(x => x.LookUpCode == LookUpCode).ToList();



                return _context.SystemSettings.Where(a => a.IsDeleted != true && a.LookUpCode == LookUpCode)
                .Select(modelResponse => new SystemSettingViewModel
                {
                    LookUpCode = modelResponse.LookUpCode,
                    ID = modelResponse.ID,
                    ItemName = modelResponse.ItemName,
                    CreatedDate = DateTime.Now,
                    ItemValue = modelResponse.ItemValue,
                    IsActive = (bool)modelResponse.IsActive,
                    CreatedBy = modelResponse.CreatedBy,
                    ModifiedBy = modelResponse.ModifiedBy,
                    LastModified = modelResponse.LastModified
                }).ToList();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                modelResponse.Add(model);
                return modelResponse;
            }
        }



    }
}
