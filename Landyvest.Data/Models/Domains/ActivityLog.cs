using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Data.Models.Domains
{
    public class ActivityLog : BaseObject
    {
        public string? UserId { get; set; }

        public string ModuleName { get; set; }
        public string ModuleAction { get; set; }

        public string Description { get; set; }

        public string Record { get; set; }

        public string OldRecord { get; set; }

        public string ActionType { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class ActivityInfo
    {
        public Int64 Id { get; set; }
        public string FullName { get; set; }
        public Int64? UserID { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModuleName { get; set; }
        public string ModuleAction { get; set; }
        public string Record { get; set; }
    }
}
