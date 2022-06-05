using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Models.Domains
{
    public class State :BaseObject
    {
        public string StateCode { get; set; }
        public string Name { get; set; }
        public long CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}
