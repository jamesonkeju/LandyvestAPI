using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Payload
{
    public class FetchCustomerPayload
    {
        public string MeterNo { get; set; }
        public string hashstring { get; set; }
        public string api_key { get; set; }
      
        public bool isprepaid { get; set; }

    }
}
