using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data;
using Landyvest.Data.Models.Domains;
using Landyvest.Data.Payload;
using Landyvest.Services.AuditLog.Concrete;
using Landyvest.Services.AuditLog.DTO;
using Landyvest.Services.CommonRoute;
using Landyvest.Services.Country;
using Landyvest.Services.Country.DTO;
using Landyvest.Utilities.Common;


namespace Landyvest.Services.Country.Concrete
{
    public class CountryServices : ICountry
    {
        private LandyvestAppContext _context;
        private IActivityLog _activityLogService;
        private ILogger<CountryServices> _logger;
        private ICommonRoute _commonServices;

        public CountryServices(LandyvestAppContext context,ILogger<CountryServices> logger, IActivityLog activityLogService,ICommonRoute commonServices)
        {
            _context = context;
            _activityLogService = activityLogService;
            _logger = logger;
            _commonServices = commonServices;
        }

        public async Task<MessageOut> AddUpdateCountry(CountryDTO payload)
        {
            var op = new OperationReqObj(payload.ID);
            try
            {
                
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
                    ? await _context.Countries.FirstOrDefaultAsync(x =>
                        (x.Name == payload.Name.Trim()) && x.CountryCode == payload.CountryCode && x.ID != payload.ID)
                    : await _context.Countries.FirstOrDefaultAsync(x =>
                        (x.Name == payload.Name.Trim()) && x.CountryCode == payload.CountryCode);

                if (check != null && check.ID > 0)
                    return _commonServices.OutputMessage(false,
                        CommonResponseMessage.RecordExisting.Replace("{0}", " country name " +payload.Name));

                #endregion

                #region Process Request:

                var data = await _context.Countries.FirstOrDefaultAsync(x =>x.ID == payload.ID);
                
                if (data != null && data.ID > 0)
                {
                    // log the old data 
                    alog.OldRecord = data != null ? JsonConvert.SerializeObject(data) : "N/A";


                    data.Name = payload.Name.ToString().Trim();
                    data.CountryCode = payload.CountryCode.ToString().Trim();
                    data.ModifiedBy = payload.ModifiedBy;
                    data.LastModified = DateTime.Now;
                 
                }
                else
                {
                    var addData = new Data.Models.Domains.Country
                    {
                        Name = payload.Name.Trim(),
                        CountryCode = payload.CountryCode,
                        IsActive = true,
                        IsDeleted=false, 
                    };
                    await _context.Countries.AddAsync(addData);
                }

                if (!await _context.TrySaveChangesAsync(_logger))
                {
                    #region Activity Logs Failed

                    alog.Description = op.fail_request_type + payload.Name;
                    alog.ActionType = op.operation_type;
                    await _activityLogService.CreateActivityLog(alog);
                    #endregion

                    return _commonServices.OutputMessage(false,op.fail_record.Replace("{0}", payload.Name));
                }

                #region Activity Logs Successful
                alog.Description = op.created_request_type + payload.Name;
                alog.ActionType = op.operation_type;
                await _activityLogService.CreateActivityLog(alog);
                #endregion
                
                return _commonServices.OutputMessage(true, op.save_record.Replace("{0}", payload.Name));

                #endregion
            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return _commonServices.OutputMessage(false,
                    string.Format(CommonResponseMessage.InternalError, op.operation_type,
                        payload.Name, ex.Message));
            }
        }
        public async Task<MessageOut> DeleteCountry(long ID)
        {


            var responseObj = new MessageOut()
            {
                IsSuccessful = true,
                Message = CommonResponseMessage.RecordDelete.Replace("{0}", "Country")
            };

            var data = await _context.Countries.FirstOrDefaultAsync(x => x.ID == ID);
          
            if (data != null)
            {
                if (data.IsActive == true)
                {
                    data.IsActive = false;
                    responseObj.Message = CommonResponseMessage.RecordDelete.Replace("{0}", "Country");
                }
                
                _context.Entry(data).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return responseObj;
        }
        public async Task<List<Data.Models.Domains.Country>> GetAllCountries(CountryFilter payload)
        {
          

            try
            {
                var qry = _context.Countries.AsQueryable();

                if (payload.Id > 0)
                {
                    qry = qry.Where(p => p.ID == payload.Id).AsQueryable();
                }

                if (!string.IsNullOrEmpty(payload.Name))
                {
                    qry = qry.Where(p => p.Name.ToUpper() == payload.Name.ToUpper()).AsQueryable();
                }

                if (!string.IsNullOrEmpty(payload.CountryCode))
                {
                    qry = qry.Where(p => p.CountryCode.ToUpper() == payload.CountryCode.ToUpper()).AsQueryable();
                }

                var data = qry.OrderBy(p => p.Name).Take(payload.pageSize).Skip((payload.pageNumber - 1) * payload.pageSize).ToList();

                return data;
            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return new List<Landyvest.Data.Models.Domains.Country>();
            }
        }
        public async Task<Data.Models.Domains.Country> GetCountryById(long id, bool CheckDeleted)
        {
            var bind = new Landyvest.Data.Models.Domains.Country();

            try
            {
                if (CheckDeleted== true)
                {
                    return _context.Countries.Where(a => a.ID == id && a.IsDeleted == false).FirstOrDefault();
                }
                else
                {
                    return _context.Countries.Where(a => a.ID == id).FirstOrDefault();
                }
               
            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return bind;
            }
        }

        public Task<MessageOut> ToggleCountryStatus(long id)
        {
            throw new NotImplementedException();
        }

        //public async Task<MessageOut> ToggleCountryStatus(long id)
        //{
        //    try
        //    {
        //        #region Initialization:



        //        #endregion

        //        var item = await _context.Countries.FindAsync(id);


        //        if (item == null || item.ID < 1)
        //        {
        //            return
        //                new MessageOut { Message = CommonResponseMessage.RecordNotFound, IsSuccessful = false };
        //        }


        //        item.IsActive = item.IsActive == false;
        //     //   item.ModifiedBy = usr.user_id;
        //        item.LastModified = DateTime.Now;

        //        if (await _context.TrySaveChangesAsync(_logger))
        //        {
        //            #region Activity Logs

        //            var activityBody = new ActivityLog
        //            {
        //                ModuleName = "",
        //                ModuleAction = "",
        //                UserId = "",
        //                Record = item != null ? JsonConvert.SerializeObject(item) : "N/A",
        //                CreatedDate = DateTime.Now,
        //                CreatedBy = "",
        //                IPAddress = IPAddressUtil.GetLocalIPAddress(),
        //                Description = RequestTypes.UpdateRequest_Fail + item.Name


        //        };

        //            await _activityLogService.CreateActivityLog(activityBody);

        //            #endregion
        //            //await _auditLogService.CreateAudit(AuditLog(item, AuditLogStatus.Unsuccessful, OperationType.Update, usr));
        //            //return _commonServices.OutputMessage(true,
        //            //    CommonResponseMessage.RecordUpdate.Replace("{0}", CommonModelNames.LOAN_TYPE));
        //        }

        //        #region Activity Logs

        //        activityBody.Description = RequestTypes.UpdateRequest + item.Name;
        //        activityBody.OperationType = OperationType.Update;
        //        await _activityLogService.CreateActivityLog(activityBody);

        //        #endregion
        //        await _auditLogService.CreateAudit(AuditLog(item, AuditLogStatus.Successful, OperationType.Update, usr));
        //        return _commonServices.OutputMessage(true,
        //            CommonResponseMessage.RecordUpdate.Replace("{0}", CommonModelNames.LOAN_TYPE));

        //    }
        //    catch (Exception ex)
        //    {
        //        await _commonServices.LogError(ex, usr.company_id, usr.user_id);
        //        return _commonServices.OutputMessage(false,
        //            string.Format(CommonResponseMessage.InternalError, OperationType.Update, CommonModelNames.LOAN_TYPE,
        //                ex.Message));
        //    }
        //}


    }
}
