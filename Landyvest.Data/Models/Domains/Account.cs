using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Models.Domains
{
    public class Account : BaseObject
    {
        public string UserId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
    }
}
