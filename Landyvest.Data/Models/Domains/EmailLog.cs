using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Models.Domains
{
    public class EmailLog : BaseObject
    {

        public EmailLog()
        {
            EmailAttachments = new HashSet<EmailAttachment>();
        }



        //[StringLength(50)]
        //[Required(ErrorMessage = "Sender Email Address is required")]
        //[DataType(DataType.EmailAddress)]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Sender { get; set; }


        //[StringLength(50)]
        //[Required(ErrorMessage = "Receiver Email Address is required")]
        //[DataType(DataType.EmailAddress)]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Receiver { get; set; }


        public string CC { get; set; }


        public string BCC { get; set; }


        //[StringLength(50)]
        [Required(ErrorMessage = "Email Subject is required")]
        public string Subject { get; set; }


        [Required(ErrorMessage = "Message Body is required")]
        public string MessageBody { get; set; }
        public bool HasAttachment { get; set; }
        public bool IsSent { get; set; }
        public int Retires { get; set; }
        public DateTime? DateSent { get; set; }
        public DateTime? DateToSend { get; set; }


        public virtual ICollection<EmailAttachment> EmailAttachments { get; set; }

    }
}
