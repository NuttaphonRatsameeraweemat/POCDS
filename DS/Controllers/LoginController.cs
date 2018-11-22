using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DS.Bll.Interfaces;
using DS.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        #region Fields

        /// <summary>
        /// The Login manager provides Login functionality.
        /// </summary>
        private readonly ILogin _login;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="LoginController" /> class.
        /// </summary>
        /// <param name="login"></param>
        public LoginController(ILogin login)
        {
            _login = login;
        }

        #endregion

        #region Methods

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody]LoginViewModel auth)
        {
            IActionResult response = Unauthorized();

            if (_login.Authenticate(auth))
            {
                var tokenString = _login.BuildToken(auth.Username);
                response = Ok(tokenString);
            }

            return response;
        }

        #endregion

    }
}