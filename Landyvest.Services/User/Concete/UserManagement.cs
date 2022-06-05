using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data;
using Landyvest.Data.Models;
using Landyvest.Data.Models.Domains;
using Landyvest.Data.Models.ViewModel;
using Landyvest.Data.Payload;
using Landyvest.Services.AuditLog.Concrete;
using Landyvest.Services.Emailing.Concrete;
using Landyvest.Services.SystemSetting.Interface;
using Landyvest.Services.User.Interface;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.User.Concrete
{
    public class UserManagement : IUserManagement
    {
        private readonly LandyvestAppContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<EmailingServices> _logger;
        private readonly IActivityLog _activityLog;
        private readonly IHostingEnvironment _env;
        private readonly ISystemSetting _systemSettingManager;
        private readonly IConfiguration _configuration;
        protected readonly UserManager<ApplicationUser> _userManager;

        public UserManagement(LandyvestAppContext context, ILogger<EmailingServices> logger, IActivityLog activityLog, IConfiguration config,
          IConfiguration iConfig, IHostingEnvironment env, ISystemSetting systemSettingManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _activityLog = activityLog;
            _config = config;
            _configuration = iConfig;
            _userManager = userManager;
        }

        public async Task<CustomResponse> AddUpdateCustomer(UserViewModel obj, bool isPrimary)
        {
            var rs = new CustomResponse();
            string response = string.Empty;
            try
            {
                if (obj.Id == string.Empty)
                {
                    var newRecord = new ApplicationUser()
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        PhoneNumber = obj.PhoneNumber,
                        Email = obj.Email,
                        CreatedDate = DateTime.Now,
                        IsActive = obj.IsActive,
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = false,
                       
                    };

                    //check if it is sub-dealer
                    if (isPrimary == true)
                        newRecord.PasswordHash = SHAUtil.Encrypt(obj.Password);
                    else
                        newRecord.DependentDealerId = obj.CreatedBy;

                    //save the record
                    var result = await _userManager.CreateAsync(newRecord);

                    if (result.Succeeded)
                    {
                        // audit trail ----- string description, string moduleName, string moduleAction, Int64 userid, object record
                        await _activityLog.CreateActivityLogAsync("New User created", "", "", "", "", obj);
                        response = ApplicationResponseCode.LoadErrorMessageByCode("200").Code;
                        return rs;
                    }
                    else
                    {
                        response = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                        return rs;
                    }
                }
                else
                {
                    var checkexist_ = _context.ApplicationUsers.FirstOrDefault(x => x.Id == obj.Id);
                    if (checkexist_ != null)
                    {
                        checkexist_ = new ApplicationUser()
                        {
                            Id = obj.Id,
                            FirstName = obj.FirstName,
                            LastName = obj.LastName,
                            PhoneNumber = obj.PhoneNumber,
                            Email = obj.Email,
                            CreatedDate = DateTime.Now,
                            IsActive = obj.IsActive,
                           
                        };

                        var result = await _userManager.UpdateAsync(checkexist_);

                        if (result.Succeeded)
                        {
                            await _activityLog.CreateActivityLogAsync("Updated User Record", "", "", obj.Id, checkexist_, obj);

                            response = ApplicationResponseCode.LoadErrorMessageByCode("201").Code;
                            return rs;
                        }
                        else
                        {
                            response = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                            return rs;
                        }
                    }
                    else
                    {
                        response = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                        return rs;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code;
                return rs;
            }
        }
    }
}
