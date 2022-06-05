using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Landyvest.API.Shared;
using Landyvest.Data.Payload;
using Landyvest.Services.Country;
using Landyvest.Services.Country.DTO;
using Landyvest.Services.FAQs.Interface;
using Landyvest.Services.FAQsDTO.DTO;
using Landyvest.Utilities.Common;


namespace Landyvest.API.Controllers
{
    [CustomRoleFilter]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FAQsController : BaseController
    {
        private IFAQs _faqService;

        public FAQsController(IFAQs FaqServices)
        {
            _faqService = FaqServices;
        }
        /// <summary>
        ///  API for creating and updating country
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add-Update-Faqs")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddUpdateQuestionAndAnswer([FromBody] NotificationLogDTO payload)
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

            var result = await _faqService.AddUpdateQuestionAndAnswer(payload);

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

       /// <summary>
       /// 
       /// </summary>
       /// <param name="filter"></param>
       /// <returns></returns>

        [HttpGet]
        [Route("GetAllQuestionAndAnswers")]
        [ProducesResponseType(typeof(ApiResult<List<Landyvest.Data.Models.Domains.Country>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllQuestionAndAnswers([FromQuery] FAQsFilter filter)
        {
            // var loginUser = _loginUser;
            try
            {
                var result = new ApiResult<IList<Landyvest.Data.Models.Domains.Faq>>();


                var dataresponse = await _faqService.GetAllQuestionAndAnswers(filter);


                if(dataresponse.Count() ==0)
                {
                    result = new ApiResult<IList<Landyvest.Data.Models.Domains.Faq>>
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
                    result = new ApiResult<IList<Landyvest.Data.Models.Domains.Faq>>
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

                var u = new ApiResult<IList<Landyvest.Data.Models.Domains.Faq>>
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
        [Route("GetFAQsById")]
        [ProducesResponseType(typeof(ApiResult<Landyvest.Data.Models.Domains.Faq>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<IActionResult> GetFAQsById([FromQuery] long Id, bool CheckDeleted=true)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                // var loginUser = _loginUser;
                var result = new ApiResult<Landyvest.Data.Models.Domains.Faq>
                {
                    HasError = false,
                    Result = await _faqService.GetFAQsById(Id, CheckDeleted)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {

                var u = new ApiResult<Landyvest.Data.Models.Domains.Faq>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message
                };
                return BadRequest(u);
            }
        }



       

    }
}
