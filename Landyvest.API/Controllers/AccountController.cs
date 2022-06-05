using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Landyvest.Data.Payload;
using Landyvest.Utilities.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Landyvest.Data;
using Landyvest.Data.Models;
using Landyvest.Services.AuditLog.Concrete;
using Landyvest.Services.Permission.Interface;
using Landyvest.Services.Role.Interface;
using System.Data;
using Landyvest.Services.DataAccess;
using Landyvest.Services.Role.DTO;
using Landyvest.Data.Models.Indentity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using Landyvest.Data.Models.ViewModel;
using Landyvest.Services.Emailing.DTO;
using Landyvest.Services.Emailing.Interface;
using System.Globalization;

namespace Landyvest.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : BaseController
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        protected readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRole _roleSignManager;
        private readonly IRolePermission _rolePermissionManager;
        private readonly LandyvestAppContext _context;
        private readonly IActivityLog _activityRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailing _emailManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, IConfiguration configuration,
            IRole roleSignManager, IRolePermission rolePermissionManager,
            LandyvestAppContext context, IActivityLog activityRepo, IHttpContextAccessor httpContextAccessor, IEmailing emailManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _roleSignManager = roleSignManager;
            _rolePermissionManager = rolePermissionManager;
            _context = context;
            _roleManager = roleManager;
            _activityRepo = activityRepo;
            _httpContextAccessor = httpContextAccessor;
            _emailManager = emailManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] UserLoginPayload payload)
        {
            var s = ApplicationResponseCode.LoadErrorMessageByCode("101");
            try
            {



                // var userObj = null;
                if (!ModelState.IsValid)
                    return Ok(
                        new ApiResult<MessageOut>
                        {
                            HasError = true,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("101").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("101").Code
                        }); ;


                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == payload.Email);

                if (user == null)
                {
                    return Ok(
                       new ApiResult<MessageOut>
                       {
                           HasError = true,
                           Message = ApplicationResponseCode.LoadErrorMessageByCode("102").Name,
                           StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("102").Code
                       });

                }

                if (user.IsDeleted == true)
                {
                    return Ok(
                       new ApiResult<MessageOut>
                       {
                           HasError = true,
                           Message = ApplicationResponseCode.LoadErrorMessageByCode("103").Name,
                           StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("103").Code
                       });
                }

                if (user.IsActive == false)
                {
                    return Ok(
                        new ApiResult<MessageOut>
                        {
                            HasError = true,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("104").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("104").Code
                        });
                }

                if (user.EmailConfirmed == false)
                {
                    return Ok(
                         new ApiResult<MessageOut>
                         {
                             HasError = true,
                             Message = ApplicationResponseCode.LoadErrorMessageByCode("105").Name,
                             StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("105").Code
                         });
                }
              

                var signedUser = await _context.ApplicationUsers.Where(a => a.Email == payload.Email).FirstOrDefaultAsync();

                if (!(await _userManager.IsEmailConfirmedAsync(signedUser)))
                {
                    return Ok(
                        new ApiResult<MessageOut>
                        {
                            HasError = true,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("102").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("102").Code
                        });
                }

              
                bool checkPassword = await _userManager.CheckPasswordAsync(signedUser, payload.Password);

                if (checkPassword == false)
                {
                    return Ok(
                      new ApiResult<MessageOut>
                      {
                          HasError = true,
                          Message = ApplicationResponseCode.LoadErrorMessageByCode("102").Name,
                          StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("102").Code
                      });
                }

                if (signedUser.PwdChangedDate == null)
                {

                    return Ok(
                     new ApiResult<MessageOut>
                     {
                         HasError = true,
                         Message = ApplicationResponseCode.LoadErrorMessageByCode("106").Name,
                         StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("106").Code
                     });
                }


                var signInResult = await _signInManager.PasswordSignInAsync(user, payload.Password, false, false);

                if (signInResult.Succeeded)
                {
                    var roleseach = new RoleFilter();
                    var roles = await _roleSignManager.GetRoles(roleseach);
                    var returnClaims = await PrepareSignInClaims(user);
                    AuthModel userProfile = PrepareUserMenu(user, roles, returnClaims);


                    return Ok(
                          new ApiResult<AuthModel>
                          {
                              HasError = false,
                              Result = userProfile,
                              Message = ApplicationResponseCode.LoadErrorMessageByCode("107").Name,
                              StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("107").Code
                          });
                }
                else
                {
                    return Ok(
                                     new ApiResult<MessageOut>
                                     {
                                         HasError = true,
                                         Message = ApplicationResponseCode.LoadErrorMessageByCode("102").Name,
                                         StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("102").Code
                                     });
                }
            }
            catch (Exception ex)
            {

                return null;
            }

        }




        [AllowAnonymous]
        [HttpPost]
        [Route("ForgetPassword")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordViewModel payload)
        {
            string token = "";

            // var userObj = null;
            if (!ModelState.IsValid)
                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = true,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("101").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("101").Code
                    }); 


            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == payload.Email || x.PhoneNumber == payload.Email);

            if (user == null)
            {
                return Ok(
                   new ApiResult<MessageOut>
                   {
                       HasError = true,
                       Message = ApplicationResponseCode.LoadErrorMessageByCode("108").Name,
                       StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("108").Code
                   });

            }

            if (user.IsDeleted == true)
            {
                return Ok(
                   new ApiResult<MessageOut>
                   {
                       HasError = true,
                       Message = ApplicationResponseCode.LoadErrorMessageByCode("103").Name,
                       StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("103").Code
                   });
            }

            if (user.IsActive==false)
            {
                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = true,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("104").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("104").Code
                    });
            }

            if (user.EmailConfirmed == false)
            {
                return Ok(
                     new ApiResult<MessageOut>
                     {
                         HasError = true,
                         Message = ApplicationResponseCode.LoadErrorMessageByCode("105").Name,
                         StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("105").Code
                     });
            }


      
            string confirmationLink = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (confirmationLink != null)
            {
                string resetConfirmationLink = "";

                // SEND EMAIL 
                var emailtemplate = await _emailManager.GetEmailTemplateByCode(EmailTemplateCode.FORGOT_PASSWORD);
                var expiredDate = Convert.ToInt32(_configuration["PwdExpireDate"]);
                string ExpirationTime = DateTime.Now.AddHours(expiredDate).ToString(@"dd/MM/yyyy hh:mm:ss tt", new CultureInfo(_configuration["UkCulture"]));

               
                ExpirationTime = ExtentionUtility.Encrypt(ExpirationTime);

               
                resetConfirmationLink = _configuration["portalURL"] + "/Account/ResetPassword?userid=" + user.Id + "&token= " + HttpUtility.UrlEncode(confirmationLink) + "&data=" + ExpirationTime;




                bool mailresponse = false;

                if (payload.IsMobile == true)
                {
                    token = GenericUtil.GenerateOTP();

                    user.ForgetPasswordOTP = token;
                    user.ForgetPasswordExpireOTP = DateTime.Now.AddMinutes(30);
                   
                    await _context.SaveChangesAsync();

                    var emailTokens_t = new List<EmailTokenViewModel>
                        {
                            new EmailTokenViewModel {  Token = EmailTokenConstants.FULLNAME,  TokenValue = user.FirstName +  ' ' + user.LastName },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                            new EmailTokenViewModel { Token = EmailTokenConstants.Token,  TokenValue = token },

                            new EmailTokenViewModel {  Token = EmailTokenConstants.LogoURL,  TokenValue = _configuration["LogoURL"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["portalURL"] }
                        };
                    await _emailManager.PrepareEmailLog(EmailTemplateCode.Landyvest_TOKEN, user.Email,
                                                                               _configuration["cc"], "", emailTokens_t, user.Id, false);
                    mailresponse = true;

                }

                else
                {
                    var emailTokens = new List<EmailTokenViewModel>
                        {
                            new EmailTokenViewModel {  Token = EmailTokenConstants.FULLNAME,  TokenValue = user.FirstName +  ' ' + user.LastName },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.USERNAME,  TokenValue = user.Email },

                            new EmailTokenViewModel {  Token = EmailTokenConstants.Token,  TokenValue = token },

                            new EmailTokenViewModel {  Token = EmailTokenConstants.ACCOUNT_VERIFICATION_LINK,  TokenValue = resetConfirmationLink },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.LogoURL,  TokenValue = _configuration["LogoURL"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["portalURL"] }
                        };

                     mailresponse = await _emailManager.PrepareEmailLog(EmailTemplateCode.FORGOT_PASSWORD, user.Email,
                                                        _configuration["cc"], "", emailTokens, user.Id, false);
                }

                if (mailresponse == true)
                {

                    _activityRepo.CreateActivityLog(string.Format("Password Reset link successfully  for  : {0} was successfully generated",
                      user.Email), getMainController(), getMainAction(), user.Id, user, null);

                    return Ok(
                   new ApiResult<MessageOut>
                   {
                       HasError = false,
                       Message = ApplicationResponseCode.LoadErrorMessageByCode("109").Name,
                       StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("109").Code
                   });

                }
                else
                {
                    return Ok(
                   new ApiResult<MessageOut>
                   {
                       HasError = true,
                       Message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name,
                       StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("110").Code
                   });
                }

            }
            else
            {
                return Ok(
                  new ApiResult<MessageOut>
                  {
                      HasError = true,
                      Message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name,
                      StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("110").Code
                  });



            }


        }


        [AllowAnonymous]
        [HttpPost]
        [Route("ChangePassword")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {

            if (!ModelState.IsValid)
                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = true,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("101").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("101").Code
                    });

            // confirm password check

            if (model.ConfirmPassword != model.NewPassword)
            {

                return Ok(
                     new ApiResult<MessageOut>
                     {
                         HasError = true,
                         Message = ApplicationResponseCode.LoadErrorMessageByCode("108").Name,
                         StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("108").Code
                     }); ;

            }

            //check old password with email address
            var signedUser = await _userManager.FindByEmailAsync(model.username);


            if (signedUser == null)
            {

                return Ok(
                     new ApiResult<MessageOut>
                     {
                         HasError = true,
                         Message = ApplicationResponseCode.LoadErrorMessageByCode("108").Name,
                         StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("108").Code
                     }); ;
            }



            var result = await _userManager.ChangePasswordAsync(signedUser, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {

                // update the pwd change date to enable the user login
                signedUser.PwdChangedDate = DateTime.Now;
                signedUser.LastModified = DateTime.Now;
                signedUser.ForcePwdChange = false;
                await _userManager.UpdateAsync(signedUser);


                ApplicationUserPasswordHistory passwordModel = new ApplicationUserPasswordHistory();
                passwordModel.UserId = signedUser.Id;
                passwordModel.CreatedDate = DateTime.Now;
                passwordModel.HashPassword = ExtentionUtility.Encrypt(model.NewPassword);
                passwordModel.CreatedBy = signedUser.Id;

                _context.ApplicationUserPasswordHistorys.Add(passwordModel);
                _context.SaveChanges();

                // update the password and send the login credentails 
                // SEND EMAIL 
                var emailtemplate = await _emailManager.GetEmailTemplateByCode(EmailTemplateCode.PASSWORD_UPDATE);



                var emailTokens = new List<EmailTokenViewModel>
                        {
                            new EmailTokenViewModel {  Token = EmailTokenConstants.FULLNAME,  TokenValue = signedUser.FirstName +  ' ' + signedUser.LastName },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.USERNAME,  TokenValue = signedUser.Email },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PASSWORD,  TokenValue = model.NewPassword },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.LogoURL,  TokenValue = _configuration["LogoURL"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["portalURL"] }
                        };

                bool mailresponse = await _emailManager.PrepareEmailLog(EmailTemplateCode.PASSWORD_UPDATE, signedUser.Email,
                                                    _configuration["cc"], "", emailTokens, signedUser.Id, false);

                if (mailresponse == true)
                {



                    _activityRepo.CreateActivityLog(string.Format("Password change was successful for  : {0}",
                        signedUser.Email), getMainController(), getMainAction(), signedUser.Id, signedUser,null);

                    return Ok(
                  new ApiResult<MessageOut>
                  {
                      HasError = false,
                      Message = ApplicationResponseCode.LoadErrorMessageByCode("112").Name,
                      StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("112").Code
                  });
                }
                else
                {
                    return Ok(
                   new ApiResult<MessageOut>
                   {
                       HasError = true,
                       Message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name,
                       StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("110").Code
                   });
                }

            }
            else
            {
                return Ok(
                  new ApiResult<MessageOut>
                  {
                      HasError = true,
                      Message = ApplicationResponseCode.LoadErrorMessageByCode("113").Name,
                      StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("113").Code
                  });


            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("UpdatePassword")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePassword([FromBody] ResetChangePasswordViewModel model)
        {

            if (!ModelState.IsValid)
                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = true,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("101").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("101").Code
                    });

            // confirm password check

            if (model.ConfirmPassword != model.NewPassword)
            {
                return Ok(
                     new ApiResult<MessageOut>
                     {
                         HasError = true,
                         Message = ApplicationResponseCode.LoadErrorMessageByCode("111").Name,
                         StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("111").Code
                     });


            }

            var hashPwd = ExtentionUtility.Encrypt(model.NewPassword);

            if (_context.ApplicationUserPasswordHistorys.Where(a => a.HashPassword == hashPwd && a.UserId == model.userid).Count() > 0)
            {
                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = true,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("114").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("114").Code
                    });
            }


            //check old password with email address
            var signedUser = await _userManager.FindByEmailAsync(model.Email);


            if (signedUser == null)
            {
                return Ok(
                  new ApiResult<MessageOut>
                  {
                      HasError = true,
                      Message = ApplicationResponseCode.LoadErrorMessageByCode("108").Name,
                      StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("108").Code
                  });

            }

            
            var result = await _userManager.ResetPasswordAsync(signedUser, model.token, model.NewPassword);

            if (result.Succeeded)
            {

                // update the pwd change date to enable the user login
                signedUser.PwdChangedDate = DateTime.Now;
                signedUser.LastModified = DateTime.Now;
                await _userManager.UpdateAsync(signedUser);


                ApplicationUserPasswordHistory passwordModel = new ApplicationUserPasswordHistory();
                passwordModel.UserId = signedUser.Id;
                passwordModel.CreatedDate = DateTime.Now;
                passwordModel.HashPassword = ExtentionUtility.Encrypt(model.NewPassword);
                passwordModel.CreatedBy = signedUser.Id;

                _context.ApplicationUserPasswordHistorys.Add(passwordModel);
                _context.SaveChanges();

                // update the password and send the login credentails 
                // SEND EMAIL 
                var emailtemplate = await _emailManager.GetEmailTemplateByCode(EmailTemplateCode.USER_CHANGE_PASSWORD);



                var emailTokens = new List<EmailTokenViewModel>
                        {
                            new EmailTokenViewModel {  Token = EmailTokenConstants.FULLNAME,  TokenValue = signedUser.FirstName +  ' ' + signedUser.LastName },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PORTALNAME,  TokenValue = _configuration["PortalName"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.USERNAME,  TokenValue = signedUser.Email },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.PASSWORD,  TokenValue = model.NewPassword },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.LogoURL,  TokenValue = _configuration["LogoURL"] },
                            new EmailTokenViewModel {  Token = EmailTokenConstants.URL,  TokenValue = _configuration["portalURL"] }
                        };

                bool mailresponse = await _emailManager.PrepareEmailLog(EmailTemplateCode.USER_CHANGE_PASSWORD, signedUser.Email,
                                                    _configuration["cc"], "", emailTokens, signedUser.Id, false);

                mailresponse = true;

                if (mailresponse == true)
                {


                    _activityRepo.CreateActivityLog(string.Format("Password change was successful for  : {0}",
                        //signedUser.Email), getController(), getAction(), CurrentUser.UserId, signedUser);
                        signedUser.Email), getMainController(), getMainAction(), signedUser.Id, signedUser,null);

                    return Ok(
                     new ApiResult<MessageOut>
                     {
                         HasError = false,
                         Message = ApplicationResponseCode.LoadErrorMessageByCode("109").Name,
                         StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("109").Code
                  

                     });

                }
                else
                {
                    return Ok(
                     new ApiResult<MessageOut>
                     {
                         HasError = true,
                         Message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name,
                         StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("110").Code
                     });
                }

            }
            else
            {

                return Ok(
                  new ApiResult<MessageOut>
                  {
                      HasError = true,
                      Message = result.Errors.FirstOrDefault().Description  /*ApplicationResponseCode.LoadErrorMessageByCode("113").Name*/,
                      StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("113").Code

                  }); ;

            }
        }

        #region Helper
        [NonAction]
        private async Task<List<Claim>> PrepareSignInClaims([FromRoute] ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var _claims = userClaims.ToList();
            string roles = string.Empty;
            IList<string> role = await _userManager.GetRolesAsync(user);

            //if (role.Any())
            //{
            //    roles = role.Join();
            //}

            string USERPERMISSION = SetUserPermissions(user.Id);

            var RoleName = await _roleManager.FindByIdAsync(user.RoleId);


            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.FirstName + ' '  + user.LastName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, RoleName.Name),
                            new Claim(ClaimTypes.Surname, user.LastName),
                            new Claim(ClaimTypes.MobilePhone, user.MobileNumber),
                            new Claim(ClaimTypes.GivenName, user.FirstName + ' '  + user.LastName),

                        }.Union(userClaims);

            foreach (var r in role)
            {
                claims = claims.Append(new Claim(ClaimTypes.Role, r));
            }

            _claims = claims.ToList();

            User.AddIdentity(new ClaimsIdentity(_claims));

            await _signInManager.SignInWithClaimsAsync(user, false, _claims);

            return _claims;
        }

        [NonAction]
        private AuthModel PrepareUserMenu([FromRoute] ApplicationUser user, [FromRoute] List<ApplicationRole> roles, [FromRoute] List<Claim> claimList)
        {
            var AuthModelResult = new AuthModel();
            var role = roles.ToList().Where(r => r.Id.Equals(user.RoleId ?? "")).FirstOrDefault();

            if (role != null)
            {
                var roleId = role.Id;
                // Get User's Role Permissions:
                var sd = _context.Database.GetDbConnection().ConnectionString;
                AccessDataLayer accessDataLayer = new AccessDataLayer(_context);
                DBManager dBManager = new DBManager(_context);

                var parameters = new List<IDbDataParameter>();

                parameters.Add(dBManager.CreateParameter("@RoleId", user.RoleId, DbType.String));
                DataTable menuList = accessDataLayer.
                    FetchRolePermissionsByRoleId(parameters.ToArray(), "FetchUserPermissionAndRole");

                List<Permission> menus = new List<Permission>();

                foreach (DataRow dataRow in menuList.Rows)
                {
                    var permission = new Permission
                    {
                        CreatedBy = dataRow["CreatedBy"].ToString(),
                        Icon = dataRow["IconName"].ToString(),
                        ParentId = Convert.ToInt32(dataRow["ParentId"]),
                        PermissionCode = dataRow["PermissionCode"].ToString(),
                        PermissionName = dataRow["PermissionName"].ToString(),
                        Url = dataRow["Url"].ToString(),
                        ID = Convert.ToInt64(dataRow["ID"])
                    };

                    menus.Add(permission);

                }


                var parentMenus = menuList.Select("ParentId = 0").ToList();
                List<Permission> SelectedParentMenus = new List<Permission>();

                foreach (DataRow dataRow in parentMenus)
                {
                    var permission = new Permission
                    {
                        //  CreatedBy = dataRow["CreatedBy"].ToString();
                        Icon = dataRow["IconName"].ToString(),
                        ParentId = Convert.ToInt32(dataRow["ParentId"]),
                        PermissionCode = dataRow["PermissionCode"].ToString(),
                        PermissionName = dataRow["PermissionName"].ToString(),
                        Url = dataRow["Url"].ToString(),
                        ID = Convert.ToInt64(dataRow["PermissionId"])
                    };

                    SelectedParentMenus.Add(permission);

                }


                var sb = new StringBuilder();
                List<SidebarMenuViewModel> sidebarMenus = GenerateUrl(SelectedParentMenus, menus, sb);


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:JwtKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:JwtExpire"]));

                var token = new JwtSecurityToken(

                    _configuration["JWT:JwtIssuer"],
                    _configuration["JWT:JwtIssuer"],

                     claimList,
                     DateTime.Now,
                     expires: expires,
                     signingCredentials: creds
                );

                var auth = new AuthModel
                {
                    ExpiryTime = token.ValidTo,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    UserInformation = user,
                    Role = role.RoleName,
                    RoleId = user.RoleId != null ? user.RoleId : "0",
                    MenuString = JsonConvert.SerializeObject(sidebarMenus),
                    Menus = JsonConvert.SerializeObject(menus),

                };

                return auth;


            }
            else
            {
                return AuthModelResult;
            }

        }



        [NonAction]
        public List<SidebarMenuViewModel> GenerateUrl([FromRoute] List<Permission> parentMenus, [FromRoute] List<Permission> menus, [FromRoute] StringBuilder sb)
        {
            List<SidebarMenuViewModel> sidebarMenus = null;
            if (parentMenus.Count > 0)
            {
                string alias = _configuration["PortalAlias"];

                // Looping through Parent Menu
                sidebarMenus = new List<SidebarMenuViewModel>();

                foreach (var menu in parentMenus)
                {
                    // Get all Menu Components into variables:
                    string url = menu.Url;
                    string menuText = menu.PermissionName;
                    string icon = menu.Icon;


                    // Get out the current Parent menu Id & ParentId inside the loop
                    var pid = menu.ID;
                    var parentId = menu.ParentId;
                    //var subMenus = menus.FindAll(x => x.ParentId == pid);

                    sidebarMenus.Add(new SidebarMenuViewModel
                    {

                        Icon = menu.Icon,
                        MenuText = menu.PermissionName,
                        Alias = alias,
                        Url = menu.Url,
                        PID = menu.ID.ToString(),
                        ParentId = menu.ParentId.ToString(),
                        SubMenus = menus.FindAll(x => x.ParentId == pid).ToList(),


                    }); ;


                }
            }
            return sidebarMenus;
        }

        [NonAction]
        private string SetUserPermissions(string UserId)
        {
            try
            {

                AccessDataLayer accessDataLayer = new AccessDataLayer(_context);
                DBManager dBManager = new DBManager(_context);


                dBManager.GetDatabasecOnnection(_configuration["Default"]);
                var parameters = new List<IDbDataParameter>();

                parameters.Add(dBManager.CreateParameter("@UserId", UserId, DbType.String));
                DataTable mUserPremissionRolemodel = accessDataLayer.FetchUserPermissionAndRole(parameters.ToArray(), "FetchUserPermissionAndRole");



                string userPermissions = "";
                if (mUserPremissionRolemodel != null)
                {
                    int i = 0;
                    foreach (DataRow item in mUserPremissionRolemodel.Rows)
                    {
                        i = i + 1;
                        if (i == 0)
                        {
                            userPermissions = item["PermissionCode"] + ",";
                        }
                        else
                        {
                            userPermissions = userPermissions + item["PermissionCode"].ToString() + ",";
                        }
                    }
                }
                return userPermissions;
            }
            catch (Exception ex)
            {
                //  _log.Error(ex);
                return string.Empty;
            }
        }

        #endregion
    }
}
