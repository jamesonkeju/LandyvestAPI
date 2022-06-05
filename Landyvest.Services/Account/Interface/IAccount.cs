using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.Account.Interface
{
    public interface IAccount
    {
        Task<MessageOut> Login(Data.Payload.UserLoginPayload payload);
        Task<MessageOut> Register(Data.Payload.AdminUserSettingViewModel payload);
    }
}
