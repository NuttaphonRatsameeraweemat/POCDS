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
    public class CacheController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Cache manager provides Cache functionality.
        /// </summary>
        private readonly ICache _cache;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CacheController" /> class.
        /// </summary>
        /// <param name="cache"></param>
        public CacheController(ICache cache)
        {
            _cache = cache;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("LoadCache")]
        public IActionResult LoadCache()
        {
            return Ok(_cache.LoadCache());
        }

        #endregion

    }
}