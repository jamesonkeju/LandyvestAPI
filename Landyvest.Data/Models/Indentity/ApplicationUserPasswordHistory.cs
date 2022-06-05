using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data.Models.Domains;

namespace Landyvest.Data.Models
{
    public class ApplicationUserPasswordHistory : BaseObject
    {
        public string UserId { get; set; }
        public string HashPassword { get; set; }

        public string PasswordSalt { get; set; }
    }
}
