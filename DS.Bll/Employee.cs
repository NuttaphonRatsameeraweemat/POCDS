using AutoMapper;
using DS.Bll.Context;
using DS.Bll.Interfaces;
using DS.Bll.Models;
using DS.Data.Pocos;
using DS.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DS.Bll
{
    public class Employee : IEmployee
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The auto mapper.
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Employee" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        public Employee(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get employee list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValueHelpViewModel> GetEmployee()
        {
            var result = new List<ValueHelpViewModel>();
            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache();
            foreach (var item in empList)
            {
                result.Add(new ValueHelpViewModel
                {
                    ValueKey = item.EmpNo,
                    ValueText = string.Format(ConstantValue.EmployeeTemplate, item.FirstnameTh, item.LastnameTh)
                });
            }
            return result;
        }

        #endregion


    }
}
