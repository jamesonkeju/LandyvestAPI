using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Landyvest.API.Shared;
using Landyvest.Services.Emailing.DTO;
using Landyvest.Services.Emailing.Interface;
using Landyvest.Utilities.Common;

namespace Landyvest.API.Controllers
{
    [CustomRoleFilter]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailTemplateController : BaseController
    {
        private IEmailing _emailTemplateServices;

        public EmailTemplateController(IEmailing emailTemplateService)
        {
            _emailTemplateServices = emailTemplateService;
        }
        /// <summary>
        ///  API for creating and updating email template
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add-emailTemplate")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddEmailTemplate ([FromBody] EmailTemplateViewModel payload)
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
           
          var result = await _emailTemplateServices.CreateEmailTemplate(payload,getMainController(),getMainAction() );
            

            if ((result != CommonResponseMessage.RecordSaved))
            {
                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = false,
                       // Result = result,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code




                    });
            }

            var response = new ApiResult<MessageOut>
            {
                HasError = false,
               // Result = result,
                Message = ApplicationResponseCode.LoadErrorMessageByCode("100").Name,
                StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("100").Code

            };
            return Ok(response);
        }

        [HttpPost]
        [Route("Update-emailTemplate")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEmailTemplate([FromBody] EmailTemplateViewModel payload)
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

            var result = await _emailTemplateServices.UpdateEmailTemplate(payload, getMainController(), getMainAction());


            if ((result != CommonResponseMessage.RecordSaved))
            {
                return Ok(
                    new ApiResult<MessageOut>
                    {
                        HasError = false,
                        // Result = result,
                        Message = ApplicationResponseCode.LoadErrorMessageByCode("200").Name,
                        StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("200").Code

                    });
            }

            var response = new ApiResult<MessageOut>
            {
                HasError = false,
                // Result = result,
                Message = ApplicationResponseCode.LoadErrorMessageByCode("100").Name,
                StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("100").Code

            };
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetAllEmailTemplate")]
        [ProducesResponseType(typeof(ApiResult<List<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCountries(bool CheckDeleted = true)
        {
            // var loginUser = _loginUser;
            try
            {
                var result = new ApiResult<IList<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>>();


                var dataresponse = await _emailTemplateServices.GetAllEmailTemplates(CheckDeleted); 

                if (dataresponse.Count() == 0)
                {
                    result = new ApiResult<IList<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>>
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
                    result = new ApiResult<IList<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>>
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

                var u = new ApiResult<IList<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message,
                    StatusCode = ApplicationResponseCode.LoadErrorMessageByCode("1000").Code
                };
                return BadRequest(u);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CheckDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmailTemplateById")]
        [ProducesResponseType(typeof(ApiResult<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmailTemplateById([FromQuery] long Id, bool CheckDeleted = true)
        {
            try
            {
                // var loginUser = _loginUser;
                var result = new ApiResult<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>
                {
                    HasError = false,
                    Result = await _emailTemplateServices.GetEmailTemplateById(Id, getMainController(), getMainAction())
                };
                return Ok(result);
            }
            catch (Exception ex)
            {

                var u = new ApiResult<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message
                };
                return BadRequest(u);
            }
        }


        [HttpGet]
        [Route("GetEmailTemplateByCode")]
        [ProducesResponseType(typeof(ApiResult<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmailTemplateByCode([FromQuery] string Code, bool CheckDeleted = true)
        {
            try
            {
                // var loginUser = _loginUser;
                var result = new ApiResult<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>
                {
                    HasError = false,
                    Result = await _emailTemplateServices.GetEmailTemplateByCode(Code)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {

                var u = new ApiResult<Landyvest.Services.Emailing.DTO.EmailTemplateViewModel>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message
                };
                return BadRequest(u);
            }
        }



        [HttpPost]
        [Route("Delete-EmailTemplate")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEmailTemplate(long id, string Modifyby)
        {

            var response = new ApiResult<MessageOut> { HasError = true };
            //  var result = await _countryServices.ToggleCountryStatus(id);

            var result = await _emailTemplateServices.DeleteEmailTemplate(id, getMainController(), getMainAction(), Modifyby);

            if (result == false)
            {
                response.Message = "failed deleting Email Teplate";
                return Ok(response);
            }

            response.HasError = false;
            response.Message = "Successfully deleted email Template";
           // response.Result = result;
            return Ok(response);
        }

    }
}
