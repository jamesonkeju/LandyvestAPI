using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data;
using Landyvest.Data.Models.Domains;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.AuditLog.Concrete
{
    public class ActivityLogServices : IActivityLog
    {
        private LandyvestAppContext _context;
        private ILogger<ActivityLogServices> _logger;
        public ActivityLogServices(LandyvestAppContext context, ILogger<ActivityLogServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CreateActivityLog(string description, string moduleName, string moduleAction, string userid, object record, object OldRecord)
        {

            try
            {

                var alog = new ActivityLog
                {
                    ModuleName = moduleName,
                    ModuleAction = moduleAction,
                    UserId = userid,
                    Description = description,
                    Record = record != null ? JsonConvert.SerializeObject(record) : "N/A",
                    OldRecord = OldRecord != null ? JsonConvert.SerializeObject(OldRecord) : "N/A",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = userid,
                    LastModified = DateTime.Now,
                    IPAddress = Landyvest.Utilities.Common.IPAddressUtil.GetLocalIPAddress()


                };

                _context.ActivityLog.Add(alog);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
        }

        public async Task<MessageOut> CreateActivityLog(ActivityLog payload)
        {
            var message = new MessageOut();
            try
            {

                var alog = new ActivityLog
                {
                    ModuleName = payload.ModuleName,
                    ModuleAction = payload.ModuleName,
                    UserId = payload.UserId,
                    Description = payload.Description,
                    Record = payload.Record != null ? JsonConvert.SerializeObject(payload.Record) : "N/A",
                    OldRecord = payload.OldRecord != null ? JsonConvert.SerializeObject(payload.OldRecord) : "N/A",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,

                    CreatedBy = payload.UserId,
                    LastModified = DateTime.Now,
                    IPAddress = IPAddressUtil.GetLocalIPAddress()
                };

                _context.ActivityLog.Add(alog);
                _context.SaveChanges();

                message.IsSuccessful = true;
                message.Message = "Activity Logged";
                    
               

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                message.IsSuccessful = false;
                message.Message = "An internal error :" + ex.Message;

            }

            return message;
        }

        public async Task CreateActivityLogAsync(string description, string moduleName, string moduleAction, string userid, object record, object OldRecord)
        {

            try
            {

                var alog = new ActivityLog
                {

                    ModuleName = moduleName,
                    ModuleAction = moduleAction,
                    UserId = userid,
                    Description = description,
                    OldRecord = OldRecord != null ? JsonConvert.SerializeObject(record) : "N/A",
                    Record = record != null ? JsonConvert.SerializeObject(record) : "N/A",
                    CreatedDate = DateTime.Now,
                    IPAddress = IPAddressUtil.GetLocalIPAddress()
                };

                await _context.ActivityLog.AddAsync(alog);
                await _context.SaveChangesAsync();



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
        }


        
    }
}
