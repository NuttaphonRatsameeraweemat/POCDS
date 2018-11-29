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
    public class UtilityService
    {

        #region [Methods]

        /// <summary>
        /// Validate min and max all property between entity and viewmodel.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TT">The view model type.</typeparam>
        /// <param name="validator"></param>
        /// <returns></returns>
        public static ValidationResultViewModel ValidateStringLength<T, TT>(TT validator)
        {
            var result = new ValidationResultViewModel();
            foreach (var propertyInfoModel in typeof(T).GetProperties().Where(m => m.PropertyType.FullName == "System.String"))
            {
                var propertyInfoViewModel = validator.GetType().GetProperties().FirstOrDefault(p => p.Name == propertyInfoModel.Name);
                if (propertyInfoViewModel == null) continue;
                int target = propertyInfoViewModel.GetValue(validator) != null ? propertyInfoViewModel.GetValue(validator).ToString().Length : 0;
                var length = typeof(T).GetProperty(propertyInfoModel.Name).GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().SingleOrDefault();
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
