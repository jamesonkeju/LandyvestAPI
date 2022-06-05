using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Models
{
    public class UserLoginHistory
    {
        public string LoginhistId { get; set; }
        public string UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTime SessionDate { get; set; }
        public string Operation { get; set; }
        public string Browser { get; set; }
    }
}
