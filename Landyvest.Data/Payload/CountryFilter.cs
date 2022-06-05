using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Payload
{
   public class CountryFilter
   {
        
        public string Name { get;set;}
        public string CountryCode { get;set;}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public long Id { get; set; }

        public bool CheckDeleted { get; set; } = true;

        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;


    }


    public class FAQsFilter
    {

        public string Question { get; set; }
        public string Answer { get; set; }
       
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public long Id { get; set; }

        public bool CheckDeleted { get; set; } = true;

        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;


    }
}
