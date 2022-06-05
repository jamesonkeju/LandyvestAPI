using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data;
using Landyvest.Data.Models.Domains;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.CommonRoute.concrete
{
    public class CommonRouteServices : ICommonRoute
    {
        private readonly LandyvestAppContext _appcontext;
        private readonly ILogger<CommonRouteServices> _logger;
        private ILogger<CommonRouteServices> logger;

        public CommonRouteServices(LandyvestAppContext appcontext)
        {

            _appcontext = appcontext;
            _logger = logger;

        }


        //public MessageOut OutputMessage(bool isSuccessful, string message) => new MessageOut { IsSuccessful = isSuccessful, Message = message };
        public MessageOut OutputMessage(bool isSuccessful, string message, long retId = 0,    List<string> errors = null) => new MessageOut
            { IsSuccessful = isSuccessful, Message = message, RetId = retId, Errors = errors };


        //public MessageOut OutputMessage(bool isSuccessful, string message, long retId = 0, string redirectUrl = "",
        //    List<string> errors = null)
        //{
        //    throw new NotImplementedException();
        //}

       
        public async Task<bool> LogError(Exception ex)
        {


            // prepare the insert action
            var error = new ErrorLog()
            {
                Name = ex.Message,
                InnerException = ex.InnerException == null ? "" : ex.InnerException.ToString(),
                Source = ex.Source,
                CreatedById = null,
                ErrorDate = DateTime.Now,

            };

            await _appcontext.ErrorLogs.AddAsync(error);
            await _appcontext.SaveChangesAsync();

            return true;
        }

        public async Task<string> GenerateBatchKey()
        {
            Random r = new Random((int)DateTime.Now.Ticks);

            //Generate your random number
            int s = r.Next(0, 999999);
            var yourRandomValue = s.ToString("D6");
            return s.ToString("D6");
        }

        public async Task<bool> LogError(Exception ex, string UserId)
        {

            // prepare the insert action
            var error = new ErrorLog()
            {
                Name = ex.Message,
                InnerException = ex.InnerException == null ? "" : ex.InnerException.ToString(),
                Source = ex.Source,
                CreatedById = UserId,
                ErrorDate = DateTime.Now,

            };

            await _appcontext.ErrorLogs.AddAsync(error);
            await _appcontext.SaveChangesAsync();

            return true;
        }

        public async  Task<bool> LogMessage(IRestResponse message)
        {
            var error = new ErrorLog()
            {
                Name = message.Content,
                InnerException = message.ErrorException == null ? "" : message.ErrorException.ToString(),
                Source = message.StatusDescription,
                CreatedById = null,
                ErrorDate = DateTime.Now,

            };

            await _appcontext.ErrorLogs.AddAsync(error);
            await _appcontext.SaveChangesAsync();

            return true;
        }
    }
}
