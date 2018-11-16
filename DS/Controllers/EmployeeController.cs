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
    public class EmployeeController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The CA manager provides CA functionality.
        /// </summary>
        private readonly IEmployee _employee;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="EmployeeController" /> class.
        /// </summary>
        /// <param name="employee"></param>
        public EmployeeController(IEmployee employee)
        {
            _employee = employee;
        }

        #endregion

        #region [Method]

        [HttpGet]
        [Route("GetEmployeeList")]
        public IActionResult GetEmployeeList()
        {
            return Ok(_employee.GetEmployee());
        }

        #endregion

    }
}