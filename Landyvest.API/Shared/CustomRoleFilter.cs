using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.API.Shared
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomRoleFilter : ActionFilterAttribute
    {
        private static string _secretKey;
        private static bool _isLocal = Convert.ToBoolean(ConfigHelpers.AppSetting("App", "IsLocal"));
        private static string _CheckToken = ConfigHelpers.AppSetting("App", "CheckToken");
        private readonly string session_token = "";
        public string UserRole { get; set; }


        public override async void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (_CheckToken == "YES")
            {
                string jwt_token = "";
                try
                {
                    jwt_token = filterContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();


                    if (string.IsNullOrWhiteSpace(jwt_token))
                    {
                        filterContext.Result = new UnauthorizedObjectResult("Unauthorized access denied.");
                        return;
                    }

                    var isValid = ValidateToken(jwt_token);


                    if (!isValid)
                    {
                        filterContext.Result = new UnauthorizedObjectResult("Unauthorized access denied.");
                        return;
                    }

                    base.OnActionExecuting(filterContext);
                }
                catch (Exception ex)
                {
                    filterContext.Result = new UnauthorizedObjectResult(ex.Message);
                    return;
                }
            }
            
        }

        public static bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                _secretKey = ConfigHelpers.AppSetting("JWT", "JwtKey");
     
                var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null) return false;
             //   var key = Encoding.ASCII.GetBytes(_secretKey);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //IssuerSigningKey = new SymmetricSecurityKey(key)
                    IssuerSigningKey = key
                };
                var principal = tokenHandler.ValidateToken(token, parameters, out var securityToken);
                var claimValue = principal.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
                return !string.IsNullOrWhiteSpace(claimValue);

            }
            catch (Exception ex)
            {
                return false;
                // do nothing if jwt validation fails
                // token has expire
            }
        }
    }
}
