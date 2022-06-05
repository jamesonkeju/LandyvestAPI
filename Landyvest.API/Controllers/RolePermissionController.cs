using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Landyvest.API.Shared;
using Landyvest.Data.Payload;
using Landyvest.Services.Country;
using Landyvest.Services.Country.DTO;
using Landyvest.Services.Permission;
using Landyvest.Services.Permission.DTO;
using Landyvest.Utilities.Common;


namespace Landyvest.API.Controllers
{
    [CustomRoleFilter]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolePermissionController : BaseController
    {
        private IPermission _permissionService;
        public RolePermissionController(IPermission permissionService)
        {
            _permissionService = permissionService;
        }
       

        [HttpPost]
        [Route("Add-Update-Permission")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddUpdatePermission([FromBody] PermissionViewModel payload)
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

            var result = await _permissionService.AddUpdatePermission(payload);

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
        [Route("GetAllParent")]
        [ProducesResponseType(typeof(ApiResult<List<PermissionViewModel>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllParent()
        {
            // var loginUser = _loginUser;
            try
            {
                var result = new ApiResult<IList<PermissionViewModel>>();


                var dataresponse = await _permissionService.GetAllParent(true);


                if (dataresponse.Count() == 0)
                {
                    result = new ApiResult<IList<PermissionViewModel>>
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
                    result = new ApiResult<IList<PermissionViewModel>>
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

                var u = new ApiResult<IList<PermissionViewModel>>
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
        [Route("DeletePermission")]
        [ProducesResponseType(typeof(ApiResult<Landyvest.Data.Models.Domains.Country>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePermission([FromQuery] DeletePermissionViewModel payload)
        {
            try
            {
                // var loginUser = _loginUser;
                var result = new ApiResult<MessageOut>
                {
                    HasError = false,
                    Result = await _permissionService.DeletePermission(payload)
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
        [Route("GetAllPermissions")]
        [ProducesResponseType(typeof(ApiResult<List<PermissionViewModel>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPermissions([FromQuery] bool Id)
        {
            // var loginUser = _loginUser;
            try
            {
                var result = new ApiResult<IList<PermissionViewModel>>();


                var dataresponse = await _permissionService.GetAllPermissions(Id);


                if (dataresponse.Count() == 0)
                {
                    result = new ApiResult<IList<PermissionViewModel>>
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
                    result = new ApiResult<IList<PermissionViewModel>>
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

                var u = new ApiResult<IList<PermissionViewModel>>
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
        [Route("GetPermissionByID")]
        [ProducesResponseType(typeof(ApiResult<PermissionViewModel>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPermissionByID([FromQuery] long Id)
        {
            // var loginUser = _loginUser;
            try
            {
                var result = new ApiResult<PermissionViewModel>();


                var dataresponse = await _permissionService.GetPermissionByID(Id);


                if (dataresponse == null)
                {
                    result = new ApiResult<PermissionViewModel>
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
                    result = new ApiResult<PermissionViewModel>
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

                var u = new ApiResult<IList<PermissionViewModel>>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u);
            }
        }


        [HttpPost]
        [Route("Update-Role-Permission")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRolePermission([FromBody] UpdateRolePermissionViewModel payload)
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

            var result = await _permissionService.UpdateRolePermission(payload);

            if (!result)
            {
                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = false,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code
                    });
            }

            var response = new ApiResult<MessageOut>
            {
                HasError = false,

               Message = ApplicationResponseCode.LoadErrorMessageByCode("100").Name,
                StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("100").Code

            };
            return Ok(response);
        }

    }
}
