    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Services.Emailing.DTO;

namespace Landyvest.Services.Emailing.Interface
{
    public   interface IEmailing
    {
        #region Email Template
        Task<List<EmailTemplateViewModel>> GetAllEmailTemplates(bool? status);
        Task<EmailTemplateViewModel> GetEmailTemplateById(long id, string ControllerName, string ActionName);
        Task<EmailTemplateViewModel> GetEmailTemplateByCode(string Code);
        Task<string> CreateEmailTemplate(EmailTemplateViewModel obj, string ControllerName, string ActionName);
        Task<string> UpdateEmailTemplate(EmailTemplateViewModel obj, string ControllerName, string ActionName);
        Task<bool> DeleteEmailTemplate(long id, string ControllerName, string ActionName, string ModifyBy);
        #endregion

        #region EmailLog

        Task<bool> PrepareEmailLog(string EmailTemplateCode, string emailTo, string emailCC
      , string emailBCC, List<EmailTokenViewModel> emailTokens,
            string CreadedBy, DateTime SendDateTime, bool EmailScheduler = false);

        Task<bool> PrepareEmailLog(string EmailTemplateCode, string emailTo, string emailCC
        , string emailBCC, List<EmailTokenViewModel> emailTokens,
       string CreadedBy, bool EmailScheduler = false, bool TranslateMessage = false, string contentToTranslate = "");

        bool SendEmail(EmailLogViewModel newEmailLog, bool EmailScheduler, string createdBy);
        bool SendEmail(EmailLogViewModel newEmailLog, string createdBy);
        void ProcessPendingEmails();

        #endregion
    }
}
