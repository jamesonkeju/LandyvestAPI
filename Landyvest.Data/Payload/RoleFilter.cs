using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Payload
{
   public class RoleFilter
   {
        
        public string RoleName { get;set;}
      

        public string Id { get; set; }

        public bool CheckDeleted { get; set; } = true;

        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;


    }
}
