using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Landyvest.Data;
using Landyvest.Services.Emailing.Interface;
using Landyvest.Utilities.Common;
using Landyvest.Utilities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Landyvest.Services.Jobs
{
    public class BackgroundJobs
    {
        private readonly IConfiguration _configuration;
       
        private readonly LandyvestAppContext _context;
        public IEmailing _emailManager;
        public BackgroundJobs(IEmailing emailManager, IConfiguration configuration,LandyvestAppContext context)
        {
            _emailManager = emailManager;
            _configuration = configuration;
           
            _context = context;
           
        }

        public void ProcessEmail()
        {
            if (_configuration["EnableHangFire_Email"].ToLower() == "yes")
            {
                _emailManager.ProcessPendingEmails();
            }
        }

       
    }
}
