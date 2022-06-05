using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Landyvest.Data.Payload;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Landyvest.Data;
using Landyvest.Data.Models;
using Landyvest.Services.AuditLog.Concrete;
using Landyvest.Services.Permission.Interface;
using Landyvest.Services.Role.Interface;
using Landyvest.Services.Emailing.Interface;
using Landyvest.Services.User.Interface;
using Landyvest.Data.Models.Domains;
using Landyvest.Data.Models.ViewModel;
using System;

using System.Text.RegularExpressions;
using Landyvest.Utilities.Common;

namespace Landyvest.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]

    public class UserController : BaseController
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
        private readonly IUserManagement _user;


        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, IConfiguration configuration,
            IRole roleSignManager, IRolePermission rolePermissionManager,
            LandyvestAppContext context, IActivityLog activityRepo, IHttpContextAccessor httpContextAccessor, IEmailing emailManager, IUserManagement user)
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
            _user = user;
           
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Add-Update-Customer")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddUpdateCustomer([FromBody] UserViewModel payload)

        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(
                        new ApiResult<MessageOut>
                        {
                            HasError = true,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("101").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("101").Code
                        });


                var expectedPasswordPattern = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
                var isValidPassword = expectedPasswordPattern.IsMatch(payload.Password);

                if (!isValidPassword)
                return Ok(
                            new ApiResult<MessageOut>
                            {
                                HasError = true,
                                Message = ApplicationResponseCode.LoadErrorMessageByCode("707").Name,
                                StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("707").Code
                            });



                if (payload.Password != payload.ConfirmPassword && payload.Password != string.Empty)
                    return Ok(
                             new ApiResult<MessageOut>
                             {
                                 HasError = true,
                                 Message = ApplicationResponseCode.LoadErrorMessageByCode("111").Name,
                                 StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("111").Code
                             });

                if (payload.Id == string.Empty &&
                    (payload.Password == string.Empty || payload.ConfirmPassword == string.Empty)
                    )
                    return Ok(
                             new ApiResult<MessageOut>
                             {
                                 HasError = true,
                                 Message = ApplicationResponseCode.LoadErrorMessageByCode("111").Name,
                                 StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("111").Code
                             });
                else
                {
                    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == payload.Email);
                    if (user != null && string.IsNullOrEmpty(payload.Id))

                    {
                        return Ok(
                           new ApiResult<MessageOut>
                           {
                               HasError = true,
                               Message = ApplicationResponseCode.LoadErrorMessageByCode("700").Name,
                               StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("700").Code
                           });

                    }
                }


                return null;
               
            }
            catch (Exception ex)
            {
                return Ok(
              new ApiResult<MessageOut>
              {
                  HasError = true,
                  Message = ex.Message,
                  StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
              });
            }
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("RegenerateOTP")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RegenerateOTP([FromBody] OTPPayload payload)

        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(
                        new ApiResult<MessageOut>
                        {
                            HasError = true,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("101").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("101").Code
                        });

                var validate = await _user.ValidateOTPToken(payload);

                return Ok(
                  new ApiResult<MessageOut>
                  {
                      HasError = false,
                      Message = validate.message,
                      StatusCode = validate.code,

                  });
            }
            catch (Exception ex)
            {
                return Ok(
              new ApiResult<MessageOut>
              {
                  HasError = true,
                  Message = ex.Message,
                  StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
              });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ValidateOTP")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ValidateOTP([FromBody] OTPPayload payload)

        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(
                        new ApiResult<MessageOut>
                        {
                            HasError = true,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("101").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("101").Code
                        });

                var validate = await _user.ValidateOTPToken(payload);

                return Ok(
                  new ApiResult<MessageOut>
                  {
                      HasError = false,
                      Message = validate.message,
                      StatusCode = validate.code,

                  });
            }
            catch (Exception ex)
            {
                return Ok(
              new ApiResult<MessageOut>
              {
                  HasError = true,
                  Message = ex.Message,
                  StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
              });
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Add-Update-Sub-Dealer")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddUpdateSubDealer([FromBody] UserViewModel payload)

        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(
                        new ApiResult<MessageOut>
                        {
                            HasError = true,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("101").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("101").Code
                        });

                if (payload.CreatedBy == string.Empty)
                    return Ok(
                        new ApiResult<MessageOut>
                        {
                            HasError = true,
                            Message = ApplicationResponseCode.LoadErrorMessageByCode("701").Name,
                            StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("701").Code
                        });

                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == payload.Email);

                if (user != null)
                {
                    return Ok(
                       new ApiResult<MessageOut>
                       {
                           HasError = true,
                           Message = ApplicationResponseCode.LoadErrorMessageByCode("700").Name,
                           StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("700").Code
                       });

                }



                var checkSubDealer = await _context.ApplicationUsers.FirstOrDefaultAsync(a => a.Id == payload.CreatedBy && a.IsActive == true && a.RoleId == Landyvest.Utilities.Common.DefineRole.Dealer);


                if (checkSubDealer == null)
                {
                    return Ok(
                       new ApiResult<MessageOut>
                       {
                           HasError = true,
                           Message = ApplicationResponseCode.LoadErrorMessageByCode("800").Name,
                           StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("800").Code
                       });

                }

                var response = await _user.AddUpdateCustomer(payload, false);

                if (response != null && response.code != "200" && response.code != "201")
                    return Ok(
                  new ApiResult<MessageOut>
                  {
                      HasError = true,
                      Message = ApplicationResponseCode.LoadErrorMessageByCode("110").Name,
                      StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("110").Code
                  });
                else
                {

                    return Ok(
                          new ApiResult<MessageOut>
                          {
                              HasError = false,
                              Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                              StatusCode = response.code


                          });
                }
            }
            catch (Exception ex)
            {
                return Ok(
              new ApiResult<MessageOut>
              {
                  HasError = true,
                  Message = ApplicationResponseCode.LoadErrorMessageByCode("1000").Name,
                  StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
              });
            }
        }


       

       

    }
}
