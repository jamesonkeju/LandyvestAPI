using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Landyvest.API.Shared;
using Landyvest.Services.Role.DTO;
using Landyvest.Services.SystemSetting.Interface;
using Landyvest.Utilities.Common;


namespace Landyvest.API.Controllers
{
    [CustomRoleFilter]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemSettingController : BaseController
    {
        private ISystemSetting _systemSettingServices;
        public SystemSettingController(ISystemSetting  systemSettingServices)
        {
            _systemSettingServices = systemSettingServices;
        }
       

        [HttpPost]
        [Route("Add-Update-SystemSetting")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddUpdateSystemSetting([FromBody] SystemSettingViewModel payload)
        {
            // var userObj = null;
            if (!ModelState.IsValid)
                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = false,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("101").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("101").Code
                    });

            var result = await _systemSettingServices.CreateUpdateSystemSetting(payload);

            if (!result.IsSuccessful)
            {
                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = false,
                        Result = result,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                    });
            }

            var response = new ApiResult<MessageOut>
            {
                HasError = false,

                Result = result,
                Message = ApplicationResponseCode.LoadErrorMessageByCode("100").Name,
                StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("100").Code

            };
            return Ok(response);
        }


        [HttpGet]
        [Route("GetSystemSettings")]
        [ProducesResponseType(typeof(ApiResult<List<SystemSettingViewModel>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSystemSettings()
        {
            // var loginUser = _loginUser;
            try
            {
                var result = new ApiResult<IList<SystemSettingViewModel>>();


                var dataresponse = await _systemSettingServices.GetSystemSettings();


                if (dataresponse.Count() == 0)
                {
                    result = new ApiResult<IList<SystemSettingViewModel>>
                    {
                        HasError = true,
                        Result = dataresponse,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("115").Code
                    };
                    return Ok(result);

                }

                else
                {
                    result = new ApiResult<IList<SystemSettingViewModel>>
                    {
                        HasError = false,
                        Result = dataresponse.ToList(),
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("100").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("100").Code
                    };
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {

                var u = new ApiResult<IList<SystemSettingViewModel>>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u);
            }
        }

        [HttpGet]
        [Route("DeleteSystemSetting")]
        [ProducesResponseType(typeof(ApiResult<SystemSettingViewModel>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSystemSetting([FromQuery] DeleteSystemSettingViewModel payload)
        {
            try
            {
                // var loginUser = _loginUser;
                var result = new ApiResult<MessageOut>
                {
                    HasError = false,
                    Result = await _systemSettingServices.DeleteSystemSetting(payload)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {

                var u = new ApiResult<MessageOut>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message
                };
                return BadRequest(u);
            }
        }



        [HttpGet]
        [Route("GetSystemSettingByLookUpCode")]
        [ProducesResponseType(typeof(ApiResult<List<SystemSettingViewModel>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSystemSettingByLookUpCode([FromQuery] string LookUpCode)
        {
            // var loginUser = _loginUser;
            try
            {
                var result = new ApiResult<List<SystemSettingViewModel>>();


                var dataresponse = await _systemSettingServices.GetSystemSettingByLookUpCodeAsync(LookUpCode);


                if (dataresponse == null)
                {
                    result = new ApiResult<List<SystemSettingViewModel>>
                    {
                        HasError = true,
                        Result = dataresponse,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("115").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("115").Code
                    };
                    return Ok(result);

                }

                else
                {
                    result = new ApiResult<List<SystemSettingViewModel>>
                    {
                        HasError = false,
                        Result = dataresponse,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("100").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("100").Code
                    };
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {

                var u = new ApiResult<IList<List<SystemSettingViewModel>>>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u);
            }
        }


      
    }
}
