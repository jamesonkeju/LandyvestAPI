using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Services.Report.DTO
{
  public  class loadSummaryDashboardViewModel
    {
        public string Description { get; set; }
        public decimal Total { get; set; }
        public string MappingItem { get; set; }

        public DateTime? CreatedDate { get; set; } 
        public string RequestId { get; set; } = "";
        public string Status { get; set; }

        public string TransDate { get; set; }

        public string Amount { get; set; }

        public string Transtype { get; set; }

        public string icon { get; set; }

        public string Cssclass { get; set; }



    }

    public class dashboardModels
    {
        public List<loadSummaryDashboardViewModel> loadDashboardSummary { get; set; }

    }

    public class TransactionView
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }

    public class InFlowOutFlowReportView
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
