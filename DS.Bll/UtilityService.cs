using AutoMapper;
using DS.Bll.Interfaces;
using DS.Bll.Models;
using DS.Data.Pocos;
using DS.Data.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DS.Bll
{
    public class UtilityService : IUtilityService
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
        /// Initializes a new instance of the <see cref="UtilityService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        public UtilityService(IUnitOfWork unitOfWork, IMapper mapper)
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
            _unitOfWork.GetRepository<ValueHelp>().GetMaxLength();
            return _mapper.Map<IEnumerable<ValueHelp>,
                IEnumerable<ValueHelpViewModel>>(
                _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == valueType, x => x.OrderBy(y => y.Sequence))
                );
        }

        /// <summary>
        /// Validate min and max all property between entity and viewmodel.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TT">The view model type.</typeparam>
        /// <param name="validator"></param>
        /// <returns></returns>
        public ValidationResultViewModel ValidateStringLength<T, TT>(TT validator)
        {
            var result = new ValidationResultViewModel();
            foreach (var propertyInfoModel in typeof(T).GetProperties().Where(m => m.PropertyType.FullName == "System.String"))
            {
                var propertyInfoViewModel = validator.GetType().GetProperties().FirstOrDefault(p => p.Name == propertyInfoModel.Name);
                if (propertyInfoViewModel == null) continue;
                int target = propertyInfoViewModel.GetValue(validator) != null ? propertyInfoViewModel.GetValue(validator).ToString().Length : 0;
                var length = typeof(T).GetProperty("ObjectiveDesc").GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().SingleOrDefault();
                if (length != null && target > length.MaximumLength)
                {
                    var error = new ModelStateError();
                    result.ErrorFlag = true;
                    error.Key = propertyInfoModel.Name;
                    error.Message = string.Format("{0} ความยาวเกินกำหนด (ไม่เกิน {1} ตัวอักษร)", propertyInfoModel.Name, length.ToString());
                    result.ModelStateErrorList.Add(error);
                }
            }
            return result;
        }

        #endregion

    }
}
