using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DS.Bll.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPlaceController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The BusinessPlace manager provides BusinessPlace functionality.
        /// </summary>
        private readonly IBusinessPlace _businessplace;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CaController" /> class.
        /// </summary>
        /// <param name="businessplace"></param>
        public BusinessPlaceController(IBusinessPlace businessplace)
        {
            _businessplace = businessplace;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetBuisnessPlace")]
        public IActionResult GetBusinessPlace(string comCode)
        {
            return Ok(_businessplace.GetBusinessPlace(comCode));
        }

        [HttpGet]
        [Route("GetOrgName")]
        public IActionResult GetOrgName(string comCode, string empNo)
        {
            return Ok(_businessplace.GetOrgName(comCode, empNo));
        }

        #endregion

    }
}