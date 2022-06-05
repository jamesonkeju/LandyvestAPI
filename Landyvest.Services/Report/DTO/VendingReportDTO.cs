using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Services.Report.DTO
{
    public class VendingReportDTO
    {
        

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
       public string ProductId { get; set; }
       
    }
    public class VendingReportDTOInner
    {


        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }

    }
}
