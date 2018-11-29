using DS.Bll.Context;
using DS.Bll.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DS.Bll
{
    public class ManageToken : IManageToken
    {

        #region [Fields]

        /// <summary>
        /// The httpcontext.
        /// </summary>
        private readonly HttpContext _httpContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentBll" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The httpcontext value.</param>
        public ManageToken(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Current Employee No.
        /// </summary>
        public string EmpNo => _httpContext.User.Claims.FirstOrDefault(x => x.Type == ConstantValue.CLAMIS_EMPNO)?.Value;

        /// <summary>
        /// Get Current Position of employee.
        /// </summary>
        public string Position => _httpContext.User.Claims.FirstOrDefault(x => x.Type == ConstantValue.CLAMIS_POS)?.Value;

        /// <summary>
        /// Get Current Organization of employee.
        /// </summary>
        public string Org => _httpContext.User.Claims.FirstOrDefault(x => x.Type == ConstantValue.CLAMIS_ORG)?.Value;

        /// <summary>
        /// Get Currrent Aduser.
        /// </summary>
        public string AdUser => _httpContext.User.Identity.Name;

        #endregion

    }
}
