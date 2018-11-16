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
    public class CompanyController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The CA manager provides CA functionality.
        /// </summary>
        private readonly ICompany _company;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CompanyController" /> class.
        /// </summary>
        /// <param name="company"></param>
        public CompanyController(ICompany company)
        {
            _company = company;
        }

        #endregion

        #region [Method]

        [HttpGet]
        [Route("GetCompanyByEmp")]
        public IActionResult GetCompanyByEmp(string empNo)
        {
            return Ok(_company.GetCompanyByEmp(empNo));
        }

        #endregion

    }
}