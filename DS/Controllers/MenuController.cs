﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DS.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Menu manager provides Menu functionality.
        /// </summary>
        private readonly IMenu _menu;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="MenuController" /> class.
        /// </summary>
        /// <param name="menu"></param>
        public MenuController(IMenu menu)
        {
            _menu = menu;
        }

        #endregion

        #region [Methods]
        
        [HttpGet]
        [Authorize(Roles = "CA_MA_Role")]
        public IActionResult Get() => Ok(_menu.GenerateMenu("BOONRAWD_LOCAL\\ds01"));

        #endregion

    }
}