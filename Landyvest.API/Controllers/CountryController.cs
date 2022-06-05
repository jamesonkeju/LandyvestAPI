using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Landyvest.API.Shared;
using Landyvest.Data.Payload;
using Landyvest.Services.Country;
using Landyvest.Services.Country.DTO;
using Landyvest.Utilities.Common;


namespace Landyvest.API.Controllers
{
    [CustomRoleFilter]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CountryController : BaseController
    {
        private ICountry _countryServices;

        public CountryController(ICountry CountryServices)
        {
            _countryServices = CountryServices;
        }
        /// <summary>
        ///  API for creating and updating country
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add-Update-Country")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddUpdateCountry([FromBody] CountryDTO payload)
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

            var result = await _countryServices.AddUpdateCountry(payload);

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
        [Route("GetAllCountries")]
        [ProducesResponseType(typeof(ApiResult<List<Landyvest.Data.Models.Domains.Country>>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCountries([FromQuery] CountryFilter filter)
        {
            // var loginUser = _loginUser;
            try
            {
                var result = new ApiResult<IList<Landyvest.Data.Models.Domains.Country>>();


                var dataresponse = await _countryServices.GetAllCountries(filter);


                if(dataresponse.Count() ==0)
                {
                    result = new ApiResult<IList<Landyvest.Data.Models.Domains.Country>>
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
                    result = new ApiResult<IList<Landyvest.Data.Models.Domains.Country>>
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

                var u = new ApiResult<IList<Landyvest.Data.Models.Domains.Country>>
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
        [Route("GetCountryById")]
        [ProducesResponseType(typeof(ApiResult<Landyvest.Data.Models.Domains.Country>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCountryById([FromQuery] long Id, bool CheckDeleted=true)
        {
            try
            {
                // var loginUser = _loginUser;
                var result = new ApiResult<Landyvest.Data.Models.Domains.Country>
                {
                    HasError = false,
                    Result = await _countryServices.GetCountryById(Id, CheckDeleted)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {

                var u = new ApiResult<Landyvest.Data.Models.Domains.Country>
                {
                    HasError = true,
                    Result = null,
                    Message = ex.Message
                };
                return BadRequest(u);
            }
        }



        [HttpPost]
        [Route("Toggle-Country-Status")]
        [ProducesResponseType(typeof(ApiResult<MessageOut>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ToggleCountryStatus(long id)
        {
      
            var response = new ApiResult<MessageOut> { HasError = true };
            var result = await _countryServices.ToggleCountryStatus(id);
            if (!result.IsSuccessful)
            {
                response.Message = result.Message;
                return Ok(response);
            }

            response.HasError = false;
            response.Message = result.Message;
            response.Result = result;
            return Ok(response);
        }

    }
}
