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
    public class ValueHelpController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The CA manager provides CA functionality.
        /// </summary>
        private readonly IUtilityService _utility;

        /// <summary>
        /// The value type currency list.
        /// </summary>
        private const string CURRENCY = "CURRENCY";

        /// <summary>
        /// The value type receive list.
        /// </summary>
        private const string RECEIVETYPE = "RECEIVETYPE";

        /// <summary>
        /// The value type paymentplace list.
        /// </summary>
        private const string PAYMENTPLACE = "PAYMENTPLACE";

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="ValueHelpController" /> class.
        /// </summary>
        /// <param name="utility"></param>
        public ValueHelpController(IUtilityService utility)
        {
            _utility = utility;
        }

        #endregion

        #region [Method]

        [HttpGet]
        [Route("GetCurrency")]
        public IActionResult GetCurrency()
        {
            return Ok(_utility.GetValueHelp(CURRENCY));
        }

        [HttpGet]
        [Route("GetReceiveType")]
        public IActionResult GetReceiveType()
        {
            return Ok(_utility.GetValueHelp(RECEIVETYPE));
        }

        [HttpGet]
        [Route("GetPaymentPlace")]
        public IActionResult GetPaymentPlace()
        {
            return Ok(_utility.GetValueHelp(PAYMENTPLACE));
        }

        #endregion
        
    }
}