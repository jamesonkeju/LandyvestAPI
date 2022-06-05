using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Models.Domains
{
    public class EmailAttachment : BaseObject
    {
        public long EmailLogId { get; set; }
        public string FolderOnServer { get; set; }
        public string FileNameOnServer { get; set; }
        public string EmailFileName { get; set; }

        public virtual EmailLog EmailLog { get; set; }
    }
}
