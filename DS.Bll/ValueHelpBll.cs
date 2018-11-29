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
    public class ValueHelpBll : IValueHelp
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
        /// Initializes a new instance of the <see cref="ValueHelpBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        public ValueHelpBll(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get List ValueHelp.
        /// </summary>
        /// <param name="valueType">The value type value.</param>
        /// <returns></returns>
        public IEnumerable<ValueHelpViewModel> GetValueHelp(string valueType)
        {
            return _mapper.Map<IEnumerable<ValueHelp>,
                IEnumerable<ValueHelpViewModel>>(
                _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == valueType, x => x.OrderBy(y => y.Sequence))
                );
        }

        #endregion

    }
}
