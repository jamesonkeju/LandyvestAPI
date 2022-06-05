using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Payload
{
   public  class ProductCommisionFilter
    {
        public decimal ProductServiceCommission { get; set; }

        public int ServiceId { get; set; }
        public int Id { get; set; }
        public int pageNumber { get; set; } = 1;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool CheckDeleted { get; set; } = true;
        public int pageSize { get; set; } = 10;
    }
}
