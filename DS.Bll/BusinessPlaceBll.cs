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
    public class BusinessPlaceBll : IBusinessPlace
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
        /// Initializes a new instance of the <see cref="BusinessPlaceBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        public BusinessPlaceBll(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get business place list by company code.
        /// </summary>
        /// <param name="comCode">The company code.</param>
        /// <returns></returns>
        public IEnumerable<ValueHelpViewModel> GetBusinessPlace(string comCode)
        {
            var result = new List<ValueHelpViewModel>();
            var businessPlaceList = _unitOfWork.GetRepository<HrbusinessPlace>().GetCache(x => x.ComCode == comCode);
            foreach (var item in businessPlaceList)
            {
                result.Add(new ValueHelpViewModel { ValueKey = item.BusinessPlace, ValueText = item.BusinessPlaceName });
            }
            return result;
        }

        /// <summary>
        /// Get oraganization name by company code and employee no.
        /// </summary>
        /// <param name="comCode">The company code.</param>
        /// <param name="empNo">The employee no.</param>
        /// <returns></returns>
        public string GetOrgName(string comCode, string empNo)
        {
            var empOrg = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.EmpNo == empNo && x.ComCode == comCode).FirstOrDefault()?.OrgId;
            var org = _unitOfWork.GetRepository<Hrorg>().GetCache(x => x.OrgId == empOrg).FirstOrDefault();
            return org?.OrgName;
        }

        #endregion


    }
}
