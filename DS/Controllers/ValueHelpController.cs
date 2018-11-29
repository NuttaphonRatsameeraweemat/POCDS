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
        private readonly IValueHelp _valueHelp;

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
        /// <param name="valueHelp"></param>
        public ValueHelpController(IValueHelp valueHelp)
        {
            _valueHelp = valueHelp;
        }

        #endregion

        #region [Method]

        [HttpGet]
        [Route("GetCurrency")]
        public IActionResult GetCurrency()
        {
            return Ok(_valueHelp.GetValueHelp(CURRENCY));
        }

        [HttpGet]
        [Route("GetReceiveType")]
        public IActionResult GetReceiveType()
        {
            return Ok(_valueHelp.GetValueHelp(RECEIVETYPE));
        }

        [HttpGet]
        [Route("GetPaymentPlace")]
        public IActionResult GetPaymentPlace()
        {
            return Ok(_valueHelp.GetValueHelp(PAYMENTPLACE));
        }

        #endregion
        
    }
}