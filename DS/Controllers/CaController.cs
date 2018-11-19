using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DS.Bll.Interfaces;
using DS.Bll.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The CA manager provides CA functionality.
        /// </summary>
        private readonly ICa _ca;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CaController" /> class.
        /// </summary>
        /// <param name="hremployee"></param>
        public CaController(ICa ca)
        {
            _ca = ca;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]data data)
        {
            var file = Request.Form.Files;
            var jsonFile = JsonConvert.DeserializeObject<List<file>>(Request.Form["dataFile"]);
            var json = JsonConvert.DeserializeObject<dataJson>(Request.Form["dataForm"]);
            var response = _ca.Add(new CaViewModel(), file);
            if (response.ErrorFlag)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_ca.GetList());
        }

        #endregion

    }

    public class file
    {
        public string name { get; set; }
    }

    public class dataJson
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
    }

    public class data
    {
        public string test { get; set; }
    }

}