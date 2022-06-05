using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Models.Domains
{
    public class BaseObject
    {
        public long ID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public string IPAddress { get; set; } = Landyvest.Utilities.Common.IPAddressUtil.GetLocalIPAddress();
    }

    public class BaseObjectResult
    {
        public long ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

       
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public string ResponseMessage  { get; set; }


    }
}
