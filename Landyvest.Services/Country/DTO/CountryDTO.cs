using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data.Models.Domains;

namespace Landyvest.Services.Country.DTO
{
    public class CountryDTO : BaseObjectResult
    {
        public long ID { get; set; }
      
        [Required(ErrorMessage = "Country Name is required")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Country code is required")]
        public string CountryCode { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }



    }

    public class CountryData: BaseObjectResult
    {
        public string CountryCode { get; set; }
        public string Name { get; set; }
    }
}
