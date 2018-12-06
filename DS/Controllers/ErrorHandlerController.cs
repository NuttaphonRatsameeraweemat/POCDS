using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DS.Bll.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DS.Controllers
{
    public class ErrorHandlerController : ControllerBase
    {

        public IActionResult NotPermission()
        {
            var model = new ValidationResultViewModel
            {
                ErrorFlag = true,
                Message = "Not Permission."
            };
            string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return Forbid(json);
        }

    }
}