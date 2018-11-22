using DS.Bll.Interfaces;
using DS.Bll.Models;
using DS.Data.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DS.Bll
{
    public class Login : ILogin
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="Login" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public Login(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Validate username and password is valid.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool Authenticate(LoginViewModel login)
        {
            bool result = false;
            if (login.Username == _config["Authen:Username"] && login.Password == _config["Authen:Password"])
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Create and setting payload on token.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string BuildToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, username));

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(360),
              signingCredentials: creds,
              claims: identity.Claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion

    }
}
