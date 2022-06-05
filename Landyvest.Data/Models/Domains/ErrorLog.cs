using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Models.Domains
{
    public class ErrorLog : BaseObject
    {
        public string Name { get; set; }
        public string InnerException { get; set; }
        public string Source { get; set; }
        public string CreatedById { get; set; }
        public DateTime ErrorDate { get; set; }
    }
}
