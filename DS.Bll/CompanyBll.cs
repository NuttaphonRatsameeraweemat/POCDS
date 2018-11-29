using AutoMapper;
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
    public class CompanyBll : ICompany
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
        /// Initializes a new instance of the <see cref="CaBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        public CompanyBll(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get company list by employeeNo.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValueHelpViewModel> GetCompanyByEmp(string empNo)
        {
            var result = new List<ValueHelpViewModel>();
            var comList = _unitOfWork.GetRepository<Hrcompany>().GetCache();
            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache().ToList();
            var employee = empList.FirstOrDefault(x => x.EmpNo == empNo)?.Aduser;
            var empAd = empList.Where(x => x.Aduser == employee).ToList();
            foreach (var item in empAd)
            {
                var temp = comList.FirstOrDefault(x => x.ComCode == item.ComCode);
                if (temp != null)
                {
                    result.Add(new ValueHelpViewModel { ValueKey = temp.ComCode, ValueText = temp.LongText });
                }
            }
            return result;
        }

        #endregion

    }
}
