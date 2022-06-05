using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data.Models;
using Landyvest.Data.Payload;

namespace Landyvest.Services.SessionTokenGenerator.Interface
{
    public interface ISessionTokenGenerator
    {
        string GenerateToken(VwUserInfornation session);
        Task<string> GenerateToken(ApplicationUser usr);
    }
}
