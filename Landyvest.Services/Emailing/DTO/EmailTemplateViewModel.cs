using System;
using System.Collections.Generic;
using System.Text;
using Landyvest.Data.Models.Domains;

namespace Landyvest.Services.Emailing.DTO
{
    public class EmailTemplateViewModel : BaseObjectResult
    {
        public long EmailTemplateId { get; set; }
        public string Subject { get; set; }
        public String MailBody { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        //public long SMSTemplateId { get; set; }
        //[Required(ErrorMessage = "Subject Code is required")]
        //[StringLength(5)]
        //public string Code { get; set; }

        //[Required(ErrorMessage = "Subject field is required")]
        //public string Name { get; set; }

        //[Required(ErrorMessage = "Message is required")]
        //[StringLength(160)]
        //public string Message { get; set; }

    }
}
