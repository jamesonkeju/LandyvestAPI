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
using Landyvest.Utilities.Common;
using Landyvest.Services.FAQsDTO.DTO;
using Landyvest.Services.FAQs.Interface;

namespace Landyvest.Services.FAQs.Concrete
{
    public class FAQsServices : IFAQs
    {
        private LandyvestAppContext _context;
        private IActivityLog _activityLogService;
        private ILogger<FAQsServices> _logger;
        private ICommonRoute _commonServices;

        public FAQsServices(LandyvestAppContext context, ILogger<FAQsServices> logger, IActivityLog activityLogService, ICommonRoute commonServices)
        {
            _context = context;
            _activityLogService = activityLogService;
            _logger = logger;
            _commonServices = commonServices;
        }


        public async Task<MessageOut> AddUpdateQuestionAndAnswer(FAQsDTO.DTO.NotificationLogDTO payload)
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
                    ? await _context.Faqs.FirstOrDefaultAsync(x =>
                        (x.Question == payload.Question.Trim()) && x.ID != payload.ID)
                    : await _context.Faqs.FirstOrDefaultAsync(x =>
                        (x.Question == payload.Question.Trim()) && x.Answer == payload.Answer);

                if (check != null && check.ID > 0)
                    return _commonServices.OutputMessage(false,
                        CommonResponseMessage.RecordExisting.Replace("{0}", " question " + payload.Question));

                #endregion

                #region Process Request:

                var data = await _context.Faqs.FirstOrDefaultAsync(x => x.ID == payload.ID);

                if (data != null && data.ID > 0)
                {
                    // log the old data 
                    alog.OldRecord = data != null ? JsonConvert.SerializeObject(data) : "N/A";


                    data.Question = payload.Question.ToString().Trim();
                    data.Answer = payload.Answer.ToString().Trim();
                    data.ModifiedBy = payload.ModifiedBy;
                    data.LastModified = DateTime.Now;

                }
                else
                {
                    var addData = new Data.Models.Domains.Faq
                    {
                        Answer = payload.Answer.Trim(),
                        Question = payload.Question,
                        IsActive = true,
                        IsDeleted = false,
                    };
                    await _context.Faqs.AddAsync(addData);
                }

                if (!await _context.TrySaveChangesAsync(_logger))
                {
                    #region Activity Logs Failed

                    alog.Description = op.fail_request_type + payload.Answer;
                    alog.ActionType = op.operation_type;
                    await _activityLogService.CreateActivityLog(alog);
                    #endregion

                    return _commonServices.OutputMessage(false, op.fail_record.Replace("{0}", payload.Question));
                }

                #region Activity Logs Successful
                alog.Description = op.created_request_type + payload.Answer;
                alog.ActionType = op.operation_type;
                await _activityLogService.CreateActivityLog(alog);
                #endregion

                return _commonServices.OutputMessage(true, op.save_record.Replace("{0}", payload.Question));

                #endregion
            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return _commonServices.OutputMessage(false,
                    string.Format(CommonResponseMessage.InternalError, op.operation_type,
                        payload.Question, ex.Message));
            }
        }
        public async Task<MessageOut> DeleteQuestionAndAnswer(long ID)
        {


            var responseObj = new MessageOut()
            {
                IsSuccessful = true,
                Message = CommonResponseMessage.RecordDelete.Replace("{0}", "Question And Answer")
            };

            var data = await _context.Faqs.FirstOrDefaultAsync(x => x.ID == ID);

            if (data != null)
            {
                if (data.IsActive == true)
                {
                    data.IsActive = false;
                    responseObj.Message = CommonResponseMessage.RecordDelete.Replace("{0}", "Question And Answer");
                }

                _context.Entry(data).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return responseObj;
        }
        public async Task<List<Data.Models.Domains.Faq>> GetAllQuestionAndAnswers(FAQsFilter payload)
        {


            try
            {
                var qry = _context.Faqs.AsQueryable();

                if (payload.Id > 0)
                {
                    qry = qry.Where(p => p.ID == payload.Id).AsQueryable();
                }

                if (!string.IsNullOrEmpty(payload.Question))
                {
                    qry = qry.Where(p => p.Question.ToUpper() == payload.Question.ToUpper()).AsQueryable();
                }

                if (!string.IsNullOrEmpty(payload.Answer))
                {
                    qry = qry.Where(p => p.Answer.ToUpper() == payload.Answer.ToUpper()).AsQueryable();
                }

                var data = qry.OrderBy(p => p.Question).Take(payload.pageSize).Skip((payload.pageNumber - 1) * payload.pageSize).ToList();

                return data;
            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return new List<Landyvest.Data.Models.Domains.Faq>();
            }
        }
        public async Task<Data.Models.Domains.Faq> GetFAQsById(long id, bool CheckDeleted)
        {
            var bind = new Landyvest.Data.Models.Domains.Faq();

            try
            {
                if (CheckDeleted == true)
                {
                    return _context.Faqs.Where(a => a.ID == id && a.IsDeleted == false).FirstOrDefault();
                }
                else
                {
                    return _context.Faqs.Where(a => a.ID == id).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                await _commonServices.LogError(ex);
                return bind;
            }
        }




    }
}
