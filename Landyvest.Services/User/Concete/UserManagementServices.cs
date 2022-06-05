using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
using Landyvest.Services.Emailing.DTO;
using Landyvest.Services.Emailing.Interface;
using Landyvest.Services.Role.Interface;
using Landyvest.Services.SystemSetting.Interface;
using Landyvest.Services.User.Interface;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.User.Concrete
{
    public class UserManagementServices : IUserManagement
    {
        private readonly LandyvestAppContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<UserManagementServices> _logger;
        private readonly IActivityLog _activityLog;

        private readonly IConfiguration _configuration;
        protected readonly UserManager<ApplicationUser> _userManager;
        private readonly IRole _roleServices;
        private readonly IEmailing _emailingService;

        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public UserManagementServices(LandyvestAppContext context, ILogger<UserManagementServices> logger, IActivityLog activityLog, IConfiguration config,
          IConfiguration iConfig, UserManager<ApplicationUser> userManager, IRole roleServices, IEmailing emailingService, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _context = context;
            _logger = logger;
            _activityLog = activityLog;
            _config = config;
            _configuration = iConfig;
            _userManager = userManager;
            _roleServices = roleServices;
            _emailingService = emailingService;
           
            _passwordHasher = passwordHasher;
        }





        public bool ValidatePhoneNumber(string parameter, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var response = _context.Users.FirstOrDefault(x => x.PhoneNumber.ToLower().Equals(parameter.ToLower()));
                var result = response == null ? false : true;
                return result;
            }
            else
            {
                var response = _context.Users.FirstOrDefault(x => x.PhoneNumber.ToLower().Equals(parameter.ToLower()) && !x.Id.Equals(id));
                var result = response == null ? false : true;
                return result;
            }

        }

        public bool ValidateEMail(string parameter, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var response = _context.Users.FirstOrDefault(x => x.Email.ToLower().Equals(parameter.ToLower()));
                var result = response == null ? false : true;
                return result;
            }
            else
            {
                var response = _context.Users.FirstOrDefault(x => x.Email.ToLower().Equals(parameter.ToLower()) && !x.Id.Equals(id));
                var result = response == null ? false : true;
                return result;
            }

        }

        public bool ValidateUserName(string parameter, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var response = _context.Users.FirstOrDefault(x => x.UserName.ToLower().Equals(parameter.ToLower()));
                var result = response == null ? false : true;
                return result;
            }
            else
            {
                var response = _context.Users.FirstOrDefault(x => x.UserName.ToLower().Equals(parameter.ToLower()) && !x.Id.Equals(id));
                var result = response == null ? false : true;
                return result;
            }

        }


        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {


            return await (from x in _context.Users
                          join y in _context.Roles on x.RoleId equals y.Id
                          select new ApplicationUser()
                          {
                              RoleId = y.Id,
                              FirstName = x.FirstName,
                              LastName = x.LastName,
                              UserName = x.UserName,
                              IsActive = x.IsActive,
                              IsDeleted = x.IsDeleted,
                              CreatedDate = x.CreatedDate,
                              Email = x.Email,
                              PhoneNumber = x.PhoneNumber,
                              EmailConfirmed = x.EmailConfirmed,
                              PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                              Id = x.Id,
                              LastModified = x.LastModified,
                              ModifiedBy = _context.ApplicationRoles.Where(a => a.Id == x.RoleId).FirstOrDefault().RoleName,

                          }).ToListAsync();
        }





        public async Task<IEnumerable<ApplicationUser>> GetAllDealersByType(string AccountType)
        {
            if (AccountType == Landyvest.Utilities.Common.DefineRoles.SubDealer)
            {
                return await (from x in _context.Users
                              join y in _context.Roles on x.RoleId equals y.Id
                              where x.RoleId == DefineRoles.SubDealerRole 
                              select new ApplicationUser()
                              {
                                  RoleId = y.Id,
                                  FirstName = x.FirstName,
                                  LastName = x.LastName,
                                  UserName = x.UserName,
                                  IsActive = x.IsActive,
                                  IsDeleted = x.IsDeleted,
                                  CreatedDate = x.CreatedDate,
                                  Email = "Sub Dealer",
                                  PhoneNumber = x.PhoneNumber,
                                  EmailConfirmed = x.EmailConfirmed,
                                  PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                                  Id = x.Id,
                                  LastModified = x.LastModified
                              }).ToListAsync();
            }

            else if (AccountType == Landyvest.Utilities.Common.DefineRoles.Customer)
            {
                return await (from x in _context.Users
                              join y in _context.Roles on x.RoleId equals y.Id
                              join r in _context.ApplicationRoles on x.RoleId equals r.Id
                              where x.RoleId == DefineRoles.CustomerRole && x.IsActive == true && x.EmailConfirmed == true
                              select new ApplicationUser()
                              {
                                  RoleId = y.Id,
                                  FirstName = x.FirstName,
                                  LastName = x.LastName,
                                  UserName = x.UserName,
                                  IsActive = x.IsActive,
                                  IsDeleted = x.IsDeleted,
                                  CreatedDate = x.CreatedDate,
                                  Email = r.RoleName,
                                  PhoneNumber = x.PhoneNumber,
                                  EmailConfirmed = x.EmailConfirmed,
                                  PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                                  Id = x.Id,
                                  LastModified = x.LastModified
                              }).ToListAsync();
            }
            else if (AccountType == Landyvest.Utilities.Common.DefineRoles.Dealer)
            {
                return await (from x in _context.Users
                              join y in _context.Roles on x.RoleId equals y.Id
                              join r in _context.ApplicationRoles on x.RoleId equals r.Id
                              where x.RoleId == DefineRoles.DealerRole && x.IsActive==true && x.EmailConfirmed==true
                              select new ApplicationUser()
                              {
                                  RoleId = y.Id,
                                  FirstName = x.FirstName,
                                  LastName = x.LastName,
                                  UserName = x.UserName,
                                  IsActive = x.IsActive,
                                  IsDeleted = x.IsDeleted,
                                  CreatedDate = x.CreatedDate,
                                  Email = r.RoleName,
                                  PhoneNumber = x.PhoneNumber,
                                  EmailConfirmed = x.EmailConfirmed,
                                  PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                                  Id = x.Id,
                                  LastModified = x.LastModified
                              }).ToListAsync();
            }
            else
            {
                return await (from x in _context.Users
                              join y in _context.Roles on x.RoleId equals y.Id
                              join r  in _context.ApplicationRoles on x.RoleId equals r.Id
                              where x.IsActive ==true && x.IsActive == true && x.EmailConfirmed == true
                              select new ApplicationUser()
                              {
                                  RoleId = y.Id,
                                  FirstName = x.FirstName,
                                  LastName = x.LastName,
                                  UserName = x.UserName,
                                  IsActive = x.IsActive,
                                  IsDeleted = x.IsDeleted,
                                  CreatedDate = x.CreatedDate,
                                   PhoneNumber = x.PhoneNumber,
                                  Email = r.RoleName,
                                  EmailConfirmed = x.EmailConfirmed,
                                  PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                                  Id = x.Id,
                                  LastModified = x.LastModified
                              }).ToListAsync();
            }
        }


        public async Task<IEnumerable<ApplicationUser>> GetDealerSubAgents(string UserId)
        {


            return await (from x in _context.Users
                          join y in _context.Roles on x.RoleId equals y.Id
                          where x.RoleId == DefineRoles.SubDealerRole 
                          select new ApplicationUser()
                          {
                              RoleId = y.Id,
                              FirstName = x.FirstName,
                              LastName = x.LastName,
                              UserName = x.UserName,
                              IsActive = x.IsActive,
                              IsDeleted = x.IsDeleted,
                              CreatedDate = x.CreatedDate,
                              Email = x.Email,
                              PhoneNumber = x.PhoneNumber,
                              EmailConfirmed = x.EmailConfirmed,
                              PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                              Id = x.Id,
                              LastModified = x.LastModified
                          }).ToListAsync();
        }

        public List<DetailsOptionDTO> GetRoles()
        {
            string[] usersFilter = { DefineRoles.CustomerRole, DefineRoles.SubDealerRole, DefineRoles.DealerRole };

            return (from y in _context.Roles where !usersFilter.Contains(y.Id) select y).Select(x => new DetailsOptionDTO()
            {
                Name = x.RoleName,
                ID = x.Id,
            }).ToList();
        }

        public async Task<UserViewModel> GetUser(string id)
        {
            var response = await (from x in _context.Users
                                  select new UserViewModel()
                                  {
                                      Id = x.Id,
                                      RoleId = x.RoleId,
                                      FirstName = x.FirstName,
                                      LastName = x.LastName,
                                      Email = x.Email,
                                      PhoneNumber = x.PhoneNumber,
                                      UserName = x.UserName,
                                      Password = x.PasswordHash,
                                      IsActive = x.IsActive
                                  }).FirstOrDefaultAsync(y => y.Id == id);

            return response;
        }

        public async Task<CustomResponse> AddUpdatePortalUser(UserViewModel obj)
        {
            var response = new CustomResponse();

            var userRoles = await _roleServices.GetRoles(new RoleFilter());


            try
            {

                if (obj.Id == string.Empty)
                {

                    string[] breakEmail = obj.Email.Split("@");

                    var newRecord = new ApplicationUser()
                    {

                        FirstName = obj.FirstName.Trim(),
                        LastName = obj.LastName.Trim(),
                        PhoneNumber = obj.PhoneNumber,
                        Email = obj.Email.Trim(),
                        MobileNumber = obj.PhoneNumber,
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = true,
                        TwoFactorEnabled = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0,
                        CreatedBy = "1000", /// system created 
                        CreatedDate = DateTime.Now,
                        UserName = breakEmail[0],
                        RoleId = obj.RoleId,

                    };


                    newRecord.PasswordHash = SHAUtil.Encrypt(obj.Password);

                    //save the record
                    var result = await _userManager.CreateAsync(newRecord);

                    if (result.Succeeded)
                    {
                        // audit trail ----- string description, string moduleName, string moduleAction, Int64 userid, object record
                        await _activityLog.CreateActivityLogAsync("New User created", "", "", "", "", obj);

                        var resut = userRoles.ToList().Where(r => r.Id == obj.RoleId).ToList();

                        var userInfo = await _context.ApplicationUsers.Where(a => a.Email.ToLower() == obj.Email).FirstOrDefaultAsync();
                        string roleResult = await _roleServices.MapUsersToRoles(userInfo.Id, resut, "UserManagement", "CreateUser");



                        // SEND EMAIL 

                        var emailTokens = new List<EmailTokenViewModel>
                        {
                            new EmailTokenViewModel {  Token = EmailTokenConstants.FULLNAME,  TokenValue = obj.FirstName +  ' ' + obj.LastName },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.USERNAME,  TokenValue = obj.Email },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PASSWORD,  TokenValue = obj.Password },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.LogoURL,  TokenValue = _configuration["LogoURL"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["PortalUrl"] }
                        };

                        bool mailresponse = await _emailingService.PrepareEmailLog(EmailTemplateCode.ACCOUNT_CREATION, obj.Email,
                                                            _configuration["cc"], "", emailTokens, userInfo.Id, false);

                        //  response. ApplicationResponseCode.LoadErrorMessageByCode("200").Code;
                        return response;
                    }
                    else
                    {
                        response.code = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                        response.message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name;
                        return response;
                    }
                }
                else
                {
                    var existingRecord = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == obj.Id);


                    if (existingRecord != null)
                    {

                        existingRecord.Id = obj.Id;
                        existingRecord.FirstName = obj.FirstName.Trim();
                        existingRecord.LastName = obj.LastName.Trim();
                        existingRecord.PhoneNumber = obj.PhoneNumber;
                        existingRecord.Email = obj.Email.Trim();
                        existingRecord.MobileNumber = obj.PhoneNumber;
                        existingRecord.ModifiedBy = existingRecord.Id;
                        existingRecord.LastModified = DateTime.Now;
                        existingRecord.RoleId = obj.RoleId;


                        var result = await _userManager.UpdateAsync(existingRecord);

                        if (result.Succeeded)
                        {
                            await _activityLog.CreateActivityLogAsync("Updated User Record", "", "", obj.Id, existingRecord, obj);

                            response.code = ApplicationResponseCode.LoadErrorMessageByCode("201").Code;
                            response.message = ApplicationResponseCode.LoadErrorMessageByCode("201").Name;
                            return response;
                        }
                        else
                        {
                            response.code = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                            response.message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name;
                            return response;

                        }
                    }
                    else
                    {
                        response.code = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                        response.message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name;
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.code = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code;
                response.message = ex.Message;
                return response;
            }
        }


        public async Task<CustomResponse> AddUpdateCustomer(UserViewModel obj, bool isPrimary)
        {
            var response = new CustomResponse();
            string RoleId = string.Empty;
            string token = "";
            var userRoles = await _roleServices.GetRoles(new RoleFilter());


            try
            {

                if (obj.Id == string.Empty || obj.Id == "")
                {

                    string[] breakEmail = obj.Email.Split("@");

                    var newRecord = new ApplicationUser()
                    {

                        FirstName = obj.FirstName.Trim(),
                        LastName = obj.LastName.Trim(),
                        PhoneNumber = obj.PhoneNumber,
                        Email = obj.Email.Trim(),
                        MobileNumber = obj.PhoneNumber,
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = true,
                        TwoFactorEnabled = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0,
                        CreatedBy = "1000", /// system created 
                        CreatedDate = DateTime.Now,
                        UserName = breakEmail[0],
                        ForcePwdChange = false,
                        PwdChangedDate = DateTime.Now,
                        ExpirationTime = DateTime.Now.AddDays(1),
                        ConfirmationLinkExpireDate = DateTime.Now,
                       
                       
                    };

                    // newRecord.PasswordHash = ExtentionUtility.Encrypt(obj.Password);
                    newRecord.PasswordHash = _passwordHasher.HashPassword(newRecord, obj.Password);
                    //newRecord.PasswordHash = ExtentionUtility.Encrypt(obj.Password);

                    //check if it is sub-dealer
                    if (isPrimary == true)
                    {
                        // newRecord.PasswordHash = SHAUtil.Encrypt(obj.Password);
                        newRecord.RoleId = Landyvest.Utilities.Common.DefineRole.Customer;
                        RoleId = Landyvest.Utilities.Common.DefineRole.Customer;
                    }
                    else
                    {
                        
                        newRecord.RoleId = Landyvest.Utilities.Common.DefineRole.Sub_Dealer;
                        RoleId = Landyvest.Utilities.Common.DefineRole.Sub_Dealer;
                    }
                    //save the record
                    var result = await _userManager.CreateAsync(newRecord);

                    if (result.Succeeded)
                    {


                        var userInfo = await _context.ApplicationUsers.Where(a => a.Email.ToLower() == obj.Email.ToLower()).FirstOrDefaultAsync();

                        string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(userInfo);

                        await _userManager.ConfirmEmailAsync(userInfo, confirmationToken);

                        await _userManager.AddPasswordAsync(userInfo, obj.Password);

                        

                        if (obj.IsMobileRegistration == true)
                        {
                            token = GenericUtil.GenerateOTP();

                            userInfo.RegistrationOTP = token;
                            userInfo.RegistrationExpireOTP = DateTime.Now.AddMinutes(30);
                            userInfo.IsActive = true;
                          
                            await _context.SaveChangesAsync();


                            var emailTokens_t = new List<EmailTokenViewModel>
                        {
                            new EmailTokenViewModel {  Token = EmailTokenConstants.FULLNAME,  TokenValue = obj.FirstName +  ' ' + obj.LastName },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                            new EmailTokenViewModel { Token = EmailTokenConstants.Token,  TokenValue = token },

                            new EmailTokenViewModel {  Token = EmailTokenConstants.LogoURL,  TokenValue = _configuration["LogoURL"] },

                            new EmailTokenViewModel {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["AdminUrl"] }
                        };
                            await _emailingService.PrepareEmailLog(EmailTemplateCode.Landyvest_TOKEN, obj.Email, _configuration["cc"], "", emailTokens_t, userInfo.Id, false);
                        }

                        // audit trail ----- string description, string moduleName, string moduleAction, Int64 userid, object record
                        await _activityLog.CreateActivityLogAsync("New User created", "", "", "", "", obj);

                        var resut = userRoles.ToList().Where(r => r.Id == RoleId).ToList();


                        string roleResult = await _roleServices.MapUsersToRoles(userInfo.Id, resut, "UserManagement", "CreateUser");



                        // SEND EMAIL 
                        var emailtemplate = await _emailingService.GetEmailTemplateByCode(EmailTemplateCode.ACCOUNT_CREATION);


                        string ExpirationTime = DateTime.Now.AddDays(1).ToString();


                        var emailTokens = new List<EmailTokenViewModel>
                        {
                            new EmailTokenViewModel {  Token = EmailTokenConstants.FULLNAME,  TokenValue = obj.FirstName +  ' ' + obj.LastName },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                            new EmailTokenViewModel { Token = EmailTokenConstants.USERNAME,  TokenValue = obj.Email },
                            new EmailTokenViewModel { Token = EmailTokenConstants.PASSWORD,  TokenValue = obj.Password },

                            new EmailTokenViewModel {  Token = EmailTokenConstants.LogoURL,  TokenValue = _configuration["LogoURL"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["PortalUrl"] }
                        };

                        bool mailresponse = await _emailingService.PrepareEmailLog(EmailTemplateCode.ACCOUNT_CREATION, obj.Email,
                                                            _configuration["cc"], "", emailTokens, userInfo.Id, false);


                        response.code = ApplicationResponseCode.LoadErrorMessageByCode("100").Code;
                        response.message = ApplicationResponseCode.LoadErrorMessageByCode("100").Name;
                        response.data = userInfo.Email;
                        response.userId = userInfo.Id;
                        return response;
                    }
                    else
                    {
                        response.code = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                        response.message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name + token;
                        return response;
                    }
                }
                else
                {
                    var existingRecord = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == obj.Id);


                    if (existingRecord != null)
                    {

                        existingRecord.Id = obj.Id;
                        existingRecord.FirstName = obj.FirstName.Trim();
                        existingRecord.LastName = obj.LastName.Trim();
                        existingRecord.PhoneNumber = obj.PhoneNumber;
                        existingRecord.Email = obj.Email.Trim();
                        existingRecord.MobileNumber = obj.PhoneNumber;
                        existingRecord.ModifiedBy = existingRecord.Id;
                        existingRecord.LastModified = DateTime.Now;



                        var result = await _userManager.UpdateAsync(existingRecord);

                        if (result.Succeeded)
                        {
                            await _activityLog.CreateActivityLogAsync("Updated User Record", "", "", obj.Id, existingRecord, obj);

                            response.code = ApplicationResponseCode.LoadErrorMessageByCode("100").Code;
                            response.message = ApplicationResponseCode.LoadErrorMessageByCode("100").Name;
                            response.data = existingRecord.Email;
                            return response;
                        }
                        else
                        {
                            response.code = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                            response.message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name;
                            return response;

                        }
                    }
                    else
                    {
                        response.code = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                        response.message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name;
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.code = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<string> CreateUser(UserViewModel obj, bool isPrimary)
        {
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

                   

                    //save the record
                    var result = await _userManager.CreateAsync(newRecord);

                    if (result.Succeeded)
                    {
                        // audit trail ----- string description, string moduleName, string moduleAction, Int64 userid, object record
                        await _activityLog.CreateActivityLogAsync("New User created", "", "", "", "", obj);
                        response = ApplicationResponseCode.LoadErrorMessageByCode("200").Code;
                        return response;
                    }
                    else
                    {
                        response = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                        return response;
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
                            return response;
                        }
                        else
                        {
                            response = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                            return response;
                        }
                    }
                    else
                    {
                        response = ApplicationResponseCode.LoadErrorMessageByCode("110").Code;
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code;
                return response;
            }
        }

        public async Task<CustomResponse> ValidateOTPToken(OTPPayload payload)
        {
            var response = new CustomResponse();
            try
            {

                var userInfo = _context.Users.Where(a => a.Email.ToLower() == payload.EmailAddress.ToLower()).FirstOrDefault();
                if (userInfo == null)
                {
                    response.code = ApplicationResponseCode.LoadErrorMessageByCode("3000").Code;
                    response.message = ApplicationResponseCode.LoadErrorMessageByCode("3000").Name;
                    return response;
                }

                if (payload.isRegistration == true)
                {
                    if (userInfo.RegistrationOTP != payload.Token)
                    {
                        response.code = ApplicationResponseCode.LoadErrorMessageByCode("3001").Code;
                        response.message = ApplicationResponseCode.LoadErrorMessageByCode("3001").Name;
                        return response;
                    }

                    if (DateTime.Now > userInfo.RegistrationExpireOTP)
                    {
                        response.code = ApplicationResponseCode.LoadErrorMessageByCode("3001").Code;
                        response.message = ApplicationResponseCode.LoadErrorMessageByCode("3001").Name;
                        return response;
                    }

                  

                }
                else
                {
                    if (userInfo.ForgetPasswordOTP != payload.Token)
                    {
                        response.code = ApplicationResponseCode.LoadErrorMessageByCode("3001").Code;
                        response.message = ApplicationResponseCode.LoadErrorMessageByCode("3001").Name;
                        return response;
                    }

                    if (DateTime.Now > userInfo.ForgetPasswordExpireOTP)
                    {
                        response.code = ApplicationResponseCode.LoadErrorMessageByCode("3001").Code;
                        response.message = ApplicationResponseCode.LoadErrorMessageByCode("3001").Name;
                        return response;
                    }

                }

             
                await _context.SaveChangesAsync();

                await _activityLog.CreateActivityLogAsync("Validate OTP Successful", "", "", userInfo.Id, userInfo, userInfo);
                response.code = ApplicationResponseCode.LoadErrorMessageByCode("100").Code;
                response.message = "OTP verficiation was successful";
                response.data = userInfo.Email;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.code = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code;
                response.message = ex.Message;
                return response;
            }
        }


        public async Task<CustomResponse> RegenerateOTP(OTPPayload payload)
        {
            var response = new CustomResponse();
            try
            {
                string token = GenericUtil.GenerateOTP();

                var userInfo = _context.Users.Where(a => a.Email.ToLower() == payload.EmailAddress.ToLower()).FirstOrDefault();
                if (userInfo == null)
                {
                    response.code = ApplicationResponseCode.LoadErrorMessageByCode("3000").Code;
                    response.message = ApplicationResponseCode.LoadErrorMessageByCode("3000").Name;
                    return response;
                }

              


              
                await _context.SaveChangesAsync();

                var emailTokens_t = new List<EmailTokenViewModel>
                        {
                            new EmailTokenViewModel {  Token = EmailTokenConstants.FULLNAME,  TokenValue = userInfo.FirstName +  ' ' + userInfo.LastName },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                            new EmailTokenViewModel { Token = EmailTokenConstants.Token,  TokenValue = token },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["PortalUrl"] },

                            new EmailTokenViewModel {  Token = EmailTokenConstants.LogoURL,  TokenValue = _configuration["LogoURL"] }
                        };
                await _emailingService.PrepareEmailLog(EmailTemplateCode.Landyvest_TOKEN, userInfo.Email,
                                                                          _configuration["cc"], "", emailTokens_t, userInfo.Id, false);


                await _activityLog.CreateActivityLogAsync("OTP Token Sent", "", "", userInfo.Id, userInfo, userInfo);
                response.code = ApplicationResponseCode.LoadErrorMessageByCode("100").Code;
                response.message = "OTP Token sent";
                response.data = userInfo.Email;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.code = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code;
                response.message = ex.Message;
                return response;
            }
        }

        public List<ApplicationUser> GetUsersByRole(string role)
        {
            var Rolen = _context.Roles.FirstOrDefault(x => x.RoleName == role);
            return (from x in _context.Users
                    join y in _context.Roles on x.RoleId equals Rolen.Id
                    where x.RoleId == y.Id
                         select new ApplicationUser()
                         {
                             RoleId = y.Id,
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             UserName = x.UserName,
                             IsActive = x.IsActive,
                             IsDeleted = x.IsDeleted,
                             CreatedDate = x.CreatedDate,
                             Email = x.Email,
                             PhoneNumber = x.PhoneNumber,
                             EmailConfirmed = x.EmailConfirmed,
                             PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                             Id = x.Id,
                             LastModified = x.LastModified,
                             ModifiedBy = _context.ApplicationRoles.Where(a => a.Id == x.RoleId).FirstOrDefault().RoleName,

                         }).ToList();
        }


        public async Task<UpdateUserViewModel> UpdateUserMobile(Landyvest.Data.Models.ViewModel.UpdateUserViewModel model)
        {
            var existingRecord = await GetUserMobile(model.Id);  /*_context.ApplicationUsers.FirstOrDefault(x => x.Id == model.Id);*/


            if (existingRecord != null)
            {
                existingRecord.Id = model.Id;
                existingRecord.FirstName = model.FirstName.Trim();
                existingRecord.LastName = model.LastName.Trim();
                existingRecord.PhoneNumber = model.PhoneNumber;
                existingRecord.Email = model.Email.Trim();
                existingRecord.MobileNumber = model.PhoneNumber;
                existingRecord.ModifiedBy = existingRecord.Id;
                existingRecord.LastModified = DateTime.Now;
                //existingRecord.RoleId = model.RoleId;
                existingRecord.IsActive = model.IsActive;



                var result = await _userManager.UpdateAsync(existingRecord);

                if (result.Succeeded)
                {
                    var log = new ActivityLog
                    {
                        CreatedDate = DateTime.Now,
                        Description = string.Format("Updated User Record"),
                        ModuleAction = "User",
                        IsActive = true,
                        IsDeleted = false,
                        UserId = model.Id,
                    };

                    var updatedRecord = new UpdateUserViewModel
                    {
                        Id = existingRecord.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        MiddleName = model.MiddleName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        IsActive = model.IsActive,
                       
                    };

                     
                    return updatedRecord;
                    
                }
                else
                {
                    return null;

                }

               
            }
            return null;

        }


        public async Task<UserViewModel> CreateUserMobile(Landyvest.Data.Models.ViewModel.UserViewModel model)
        {
            
            var dealer = GetUser(model.DealerId);
            var username = model.Email.Split("@")[0];


            try
            {

                var newRecord = new ApplicationUser()
                {
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email.Trim(),
                    MobileNumber = model.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    CreatedBy = model.DealerId,
                    CreatedDate = DateTime.Now,
                    UserName = username,
                    RoleId = DefineRoles.SubDealerRole,
                   
                    PwdChangedDate = DateTime.Now,
                    ConfirmationLinkExpireDate = DateTime.Now,
                    IsActive = true,
                    ForcePwdChange = true,
                };


                model.Password = Landyvest.Utilities.Common.PasswordGenerator.GenerateRandomPassword();
                newRecord.PasswordHash = ExtentionUtility.Encrypt(model.Password);

                //save the record
                var result = await _userManager.CreateAsync(newRecord, model.Password);

                if (result.Succeeded)
                {

                    var freshRecord = new UserViewModel
                    {
                        
                        //Id = result.Succeeded.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        MiddleName = model.MiddleName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        IsActive = model.IsActive
                    };

                   

                    var resut = await _context.Roles.Where(r => r.Id == DefineRoles.SubDealerRole).ToListAsync();

                    var userInfo = await _context.ApplicationUsers.Where(a => a.Email.ToLower() == model.Email).FirstOrDefaultAsync();

                    // create the wallet
                   
                    string roleResult = await _roleServices.MapUsersToRoles(userInfo.Id, resut, "Agent", "CreateWallet");

                    // SEND EMAIL 
                    var emailTokens = new List<EmailTokenViewModel>
                        {
                            new EmailTokenViewModel {  Token = EmailTokenConstants.FULLNAME,  TokenValue = model.FirstName +  ' ' + model.LastName },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.USERNAME,  TokenValue = model.Email },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PASSWORD,  TokenValue = model.Password },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.LogoURL,  TokenValue = _configuration["LogoURL"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["portalURL"] }
                        };

                    bool mailresponse = await _emailingService.PrepareEmailLog(EmailTemplateCode.ACCOUNT_CREATION, model.Email,
                                                        _configuration["cc"], "", emailTokens, userInfo.Id, false);
                    var log = new ActivityLog
                    {
                        CreatedDate = DateTime.Now,
                        Description = string.Format("New sub dealer created"),
                        ModuleAction = "Created",
                        IsActive = true,
                        IsDeleted = false,
                        UserId = string.Empty,
                    };

                    //await _activityLog.CreateActivityLogAsync(log.Description, "Agent", log.ModuleAction, model.Id, model, newRecord);

                    await _activityLog.CreateActivityLogAsync("User Created", "Agent", "Create", model.Id, model, newRecord);
                    return freshRecord;

                }
                else
                {
                    return null;
                    
                }



            }
            catch (Exception ex)
            {
                return null;
            }
           
        }


        public async Task<ApplicationUser> GetUserMobile(string userId)
        {
            return await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();


            //return await (from x in _context.Users
            //              where x.Id == userId
            //              join y in _context.Roles on x.RoleId equals y.Id
            //              select
            //              {
            //                  RoleId = y.Id,
            //                  FirstName = x.FirstName,
            //                  LastName = x.LastName,
            //                  UserName = x.UserName,
            //                  IsActive = x.IsActive,
            //                  IsDeleted = x.IsDeleted,
            //                  CreatedDate = x.CreatedDate,
            //                  Email = x.Email,
            //                  PhoneNumber = x.PhoneNumber,
            //                  EmailConfirmed = x.EmailConfirmed,
            //                  PhoneNumberConfirmed = x.PhoneNumberConfirmed,
            //                  Id = x.Id,
            //                  LastModified = x.LastModified,
            //                  ModifiedBy = _context.ApplicationRoles.Where(a => a.Id == x.RoleId).FirstOrDefault().RoleName,

            //              }).FirstOrDefault();
        }



    }
}
