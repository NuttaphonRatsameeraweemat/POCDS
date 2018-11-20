using AutoMapper;
using DS.Bll.Interfaces;
using DS.Bll.Models;
using DS.Data.Pocos;
using DS.Data.Repository.Interfaces;
using DS.Helper.Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DS.Bll.Context;
using System.Transactions;
using Nest;

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

        /// <summary>
        /// The Utility function.
        /// </summary>
        private readonly IUtilityService _utility;

        /// <summary>
        /// The elastic search function.
        /// </summary>
        private readonly IElasticSearch<CaSearchViewModel> _elastic;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Ca" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="attachmemt">The attachment file function.</param>
        /// <param name="utility">The utility function.</param>
        /// <param name="elastic">The elastic search function.</param>
        public Ca(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAttachment attachmemt,
            IUtilityService utility,
            IElasticSearch<CaSearchViewModel> elastic)
        {
            _unitOfWork = unitOfWork;
            _attachmemt = attachmemt;
            _mapper = mapper;
            _utility = utility;
            _elastic = elastic;
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
            return _mapper.Map<DS.Data.Pocos.Ca, CaViewModel>(_unitOfWork.GetRepository<DS.Data.Pocos.Ca>().GetById(id)); ;
        }

        /// <summary>
        /// Insert Ca and upload file.
        /// </summary>
        /// <param name="model">The infomation ca.</param>
        /// <param name="file">The content file.</param>
        /// <returns></returns>
        public ValidationResultViewModel Add(CaViewModel model)
        {
            var response = ValidateData(model);
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
                _attachmemt.UploadFile(model.AttachmentList, ca.Id, CaViewModel.ProcessCode, ca.Cano);
                scope.Complete();
            }

            return response;
        }

        /// <summary>
        /// Validate value of ca.
        /// </summary>
        /// <param name="model">The infomation ca.</param>
        /// <returns></returns>
        private ValidationResultViewModel ValidateData(CaViewModel model)
        {
            var result = _utility.ValidateStringLength<DS.Data.Pocos.Ca, CaViewModel>(model);

            if (string.IsNullOrWhiteSpace(model.RequestFor))
            {
                var property = model.GetType().GetProperty(nameof(model.RequestFor));
                result.ModelStateErrorList.Add(new ModelStateError
                {
                    Key = property.Name,
                    Message = ConstantValue.PleaseFill + property.Name
                });
            }

            if (result.ModelStateErrorList.Count > 0)
            {
                result.ErrorFlag = true;
            }

            return result;
        }

        /// <summary>
        /// Get Ca List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CaSearchViewModel> GetList(DataTableAjaxPost model)
        {
            List<CaSearchViewModel> result = new List<CaSearchViewModel>();
            result = _elastic.SearchFilter(this.GetQueryFilter(model.Search)).ToList();
            return result;
        }

        /// <summary>
        /// Get query filter search descriptor.
        /// </summary>
        /// <param name="search">The search value.</param>
        /// <returns></returns>
        private Func<SearchDescriptor<CaSearchViewModel>, ISearchRequest> GetQueryFilter(string search)
        {
            ISearchRequest searchFunc(SearchDescriptor<CaSearchViewModel> s) => s
                                                                       .Index(ConstantValue.CAIndex)
                                                                       .Type(ConstantValue.CAType)
                                                                       .From(0)
                                                                       .Take(1000)
                                                                       .Query(q =>
                                                                                    //Filter
                                                                                    //  (q.Terms(t => t.Field(f => f.CreateBy).Terms(users)) ||
                                                                                    //   q.Terms(t => t.Field(f => f.RequestFor).Terms(users)) ||
                                                                                    //   q.Terms(t => t.Field(f => f.RequestOrg).Terms(orgs)) ||
                                                                                    //   q.Terms(t => t.Field(f => f.CreateOrg).Terms(orgs)))
                                                                                    //&&
                                                                                    //Search All Field
                                                                                    q.MultiMatch(mm => mm
                                                                                            .Query(search)
                                                                                            .Type(TextQueryType.PhrasePrefix)
                                                                                            .Fields(fs => fs
                                                                                                                         .Field(p => p.Amount)
                                                                                                                         .Field(p => p.AmountText)
                                                                                                                         .Field(p => p.Approver01)
                                                                                                                         .Field(p => p.Approver02)
                                                                                                                         .Field(p => p.Approver03)
                                                                                                                         .Field(p => p.Approver04)
                                                                                                                         .Field(p => p.Approver05)
                                                                                                                         .Field(p => p.Approver06)
                                                                                                                         .Field(p => p.Approver07)
                                                                                                                         .Field(p => p.Approver08)
                                                                                                                         .Field(p => p.Approver09)
                                                                                                                         .Field(p => p.Approver10)
                                                                                                                         .Field(p => p.CANo)
                                                                                                                         .Field(p => p.DueDate)
                                                                                                                         .Field(p => p.DueDateText)
                                                                                                                         .Field(p => p.ReceiveDate)
                                                                                                                         .Field(p => p.ReceiveDateText)
                                                                                                                         .Field(p => p.CreateByText)
                                                                                                                         .Field(p => p.StatusText)
                                                                                                          )
                                                                                            ))
                                                                       .Sort(m => m.Descending(f => f.ID));
            return searchFunc;
        }

        #endregion

    }
}
