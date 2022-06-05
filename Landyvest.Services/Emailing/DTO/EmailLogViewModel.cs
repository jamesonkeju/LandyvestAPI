using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Landyvest.Services.Emailing.DTO
{
    public class EmailLogViewModel
    {
        public EmailLogViewModel()
        {
            AttachmentModelCol = Enumerable.Empty<EmailLogAttachementViewModel>().ToList();
        }

        public long Id { get; set; }
        public string Receiver { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public bool HasAttachment { get; set; }
        public List<EmailLogAttachementViewModel> AttachmentModelCol { get; set; }
    }


    public class EmailTokenViewModel
    {
        public string TokenName { get; set; }
        public string Token { get; set; }
        public string TokenValue { get; set; }

    }

    public class EmailTemplateSetup
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public int DefaultEmailTemplateID { get; set; }
    }


    public class EmailLogAttachementViewModel
    {
        public string FolderOnServer { get; set; }
        public string FileNameOnServer { get; set; }
        public string EmailFileName { get; set; }
    }

    public class SendEmailLogViewModel
    {
        public string EmailTemplateCode { get; set; }
        [Required]
        
      
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Attachment { get; set; }
        public string Tokens { get; set; }
        public string serverPath { get; set; }
        public string SendDate { get; set; }
        public string SendNow { get; set; }
    }
}


