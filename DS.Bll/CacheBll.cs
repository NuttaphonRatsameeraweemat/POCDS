using DS.Bll.Interfaces;
using DS.Data.Pocos;
using DS.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll
{
    public class CacheBll : ICache
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public CacheBll(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region [Methods]

        public string LoadCache()
        {
            string result = string.Empty;
            try
            {
                DateTime startTime = DateTime.Now;
                _unitOfWork.GetRepository<AppMenu>().GetCache();
                _unitOfWork.GetRepository<AppCompositeRole>().GetCache();
                _unitOfWork.GetRepository<AppCompositeRoleItem>().GetCache();
                _unitOfWork.GetRepository<AppSingleRole>().GetCache();
                _unitOfWork.GetRepository<Hremployee>().GetCache();
                _unitOfWork.GetRepository<UserRole>().GetCache();
                DateTime endTime = DateTime.Now;
                TimeSpan diffTime = endTime - startTime;
                result = string.Format("Initial Time: {0} seconds, At {1} - {2}.", diffTime.Seconds.ToString(), startTime.ToString("dd/MM/yyyy HH:mm:ss"), endTime.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        #endregion

    }
}
