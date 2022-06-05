using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Payload
{
     public class ProductFilter
     {
      
        public string Name { get; set; }

     
        public string ProductDescription { get; set; }
        public string ProductCode { get; set; }
        public int Id { get; set; }
        public string ActionName { get; set; }
        public int pageNumber { get; set; } = 1;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool CheckDeleted { get; set; } = true;
        public int pageSize { get; set; } = 10;
        public string ControllerName { get; set; }
       
    }
}
