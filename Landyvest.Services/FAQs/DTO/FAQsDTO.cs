using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data.Models.Domains;

namespace Landyvest.Services.FAQsDTO.DTO
{
    public class NotificationLogDTO : BaseObjectResult
    {

        [Required(ErrorMessage = "Question is required")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Answer code is required")]
        public string Answer { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }



    }

    public class CountryData : BaseObjectResult
    {
        public string CountryCode { get; set; }
        public string Name { get; set; }
    }
}
