using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Services.Role.DTO
{
   public  class SystemSettingViewModel
    {
       [Key]
        public long ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsActive { get; set; }

        [Required]
        public string LookUpCode { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public string ItemValue { get; set; }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }

    public class DeleteSystemSettingViewModel
    {
        public long ID { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ModifiedBy { get; set; }
    }
}
