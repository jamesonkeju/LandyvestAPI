using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Payload
{
    public class FetchBalancePayload
    {
        public string dealer_code { get; set; }
        public string hashstring { get; set; }
        public string api_key { get; set; }

    }
}
