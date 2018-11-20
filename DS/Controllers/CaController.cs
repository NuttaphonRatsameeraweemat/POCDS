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
        public IActionResult Save([FromBody]CaViewModel model)
        {
            var response = _ca.Add(model);
            if (response.ErrorFlag)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) => Ok(_ca.Get(id));

        [HttpPost]
        [Route("GetList")]
        public IActionResult GetList(DataTableAjaxPost model)
        {
            return Ok(_ca.GetList(model));
        }

        #endregion

    }

}