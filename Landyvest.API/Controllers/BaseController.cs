using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Landyvest.API.Shared;
using Landyvest.Data;
using Landyvest.Data.Models;
using Landyvest.Data.Models.Indentity;
using Landyvest.Data.Payload;
using Landyvest.Services.DataAccess;
using Landyvest.Services.Role.DTO;
using Landyvest.Utilities.Common;

namespace Landyvest.API.Controllers
{
    public class BaseController : ControllerBase
    {
      
        public BaseController()
        {
            
        }

        [NonAction]
        public string getMainAction()
        {
            return ControllerContext.ActionDescriptor.ActionName;
        }

        [NonAction]
        public string getMainController()
        {
            return ControllerContext.ActionDescriptor.ControllerName;
        }


    }
}
