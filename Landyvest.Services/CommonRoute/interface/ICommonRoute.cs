using Abp.Application.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.CommonRoute
{
   
    public interface ICommonRoute : IApplicationService
    {
        MessageOut OutputMessage(bool isSuccessful, string message, long retId = 0, List<string> errors = null);

        Task<bool> LogError(Exception ex);
        Task<bool> LogError(Exception ex,  string UserId);

        Task<bool> LogMessage(IRestResponse message);



        Task<string> GenerateBatchKey();


    }

}
