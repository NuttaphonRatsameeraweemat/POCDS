using AutoMapper;
using DS.Bll.Interfaces;
using DS.Bll.Models;
using DS.Data.Pocos;
using DS.Data.Repository.Interfaces;
using DS.Helper.Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using DS.Bll.Context;
using System.Transactions;

namespace DS.Bll
{
    public class Ca : ICa
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

        /// <summary>
        /// The Attachment file function.
        /// </summary>
        private readonly IAttachment _attachmemt;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Ca" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="attachmemt">The attachment file function.</param>
        public Ca(IUnitOfWork unitOfWork, IMapper mapper, IAttachment attachmemt)
        {
            _unitOfWork = unitOfWork;
            _attachmemt = attachmemt;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Detail CA.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public CaViewModel Get(int id)
        {
            var result = new CaViewModel();
            return result;
        }

        /// <summary>
        /// Insert Ca and upload file.
        /// </summary>
        /// <param name="model">The infomation ca.</param>
        /// <param name="file">The content file.</param>
        /// <returns></returns>
        public ValidationResultViewModel Add(CaViewModel model, IFormFileCollection file)
        {
            var response = VaildateData(model);
            if (response.ErrorFlag)
            {
                return response;
            }
            using (var scope = new TransactionScope())
            {
                var emp = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.EmpNo == "").FirstOrDefault();
                var ca = _mapper.Map<CaViewModel, DS.Data.Pocos.Ca>(model);
                ca.Cano = DateTime.Now.ToString(ConstantValue.DateTimeFormat);
                ca.Status = ConstantValue.TransStatusSaved;
                ca.CreateBy = emp.EmpNo;
                ca.CreateOrg = emp.OrgId;
                ca.CreatePos = emp.PositionId;
                ca.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<DS.Data.Pocos.Ca>().Add(ca);
                _unitOfWork.Complete();

                //Attachment file
                _attachmemt.UploadFile(file, ca.Id, CaViewModel.ProcessCode, ca.Cano);
            }

            return response;
        }

        /// <summary>
        /// Validate value of ca.
        /// </summary>
        /// <param name="model">The infomation ca.</param>
        /// <returns></returns>
        private ValidationResultViewModel VaildateData(CaViewModel model)
        {
            var result = new ValidationResultViewModel();
            return result;
        }

        #endregion

    }
}
