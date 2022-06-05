using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data.Models.Domains;
using Landyvest.Services.AuditLog.DTO;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.AuditLog.Concrete
{
    public interface IActivityLog
    {
        #region Interface for Activity Log Service CRUD
        Task<MessageOut> CreateActivityLog(ActivityLog payload);
        Task CreateActivityLogAsync(string description, string controllerName, string actionName, string userid, object record, object OldRecord);
        void CreateActivityLog(string description, string controllerName, string actionName, string userid, object record, object OldRecord);
       
        #endregion
    }
}
