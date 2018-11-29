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
    public class CaBll : ICa
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
        /// The manage payload on token.
        /// </summary>
        private readonly IManageToken _manageToken;

        /// <summary>
        /// The elastic search function.
        /// </summary>
        private readonly IElasticSearch<CaSearchViewModel> _elastic;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="attachmemt">The attachment file function.</param>
        /// <param name="manageToken">The manage token payload value.</param>
        /// <param name="elastic">The elastic search function.</param>
        public CaBll(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAttachment attachmemt,
            IManageToken manageToken,
            IElasticSearch<CaSearchViewModel> elastic)
        {
            _unitOfWork = unitOfWork;
            _attachmemt = attachmemt;
            _mapper = mapper;
            _manageToken = manageToken;
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
            return _mapper.Map<DS.Data.Pocos.Ca, CaViewModel>(_unitOfWork.GetRepository<DS.Data.Pocos.Ca>().GetById(id));
        }

        /// <summary>
        /// Insert Ca and upload file.
        /// </summary>
        /// <param name="model">The infomation ca.</param>
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
                var ca = _mapper.Map<CaViewModel, DS.Data.Pocos.Ca>(model);
                ca.Cano = DateTime.Now.ToString(ConstantValue.DATETIME_YEARMONTHDAYTIME);
                ca.Status = ConstantValue.TransStatusSaved;
                ca.CreateBy = _manageToken.EmpNo;
                ca.CreateOrg = _manageToken.Org;
                ca.CreatePos = _manageToken.Position;
                ca.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<DS.Data.Pocos.Ca>().Add(ca);
                _unitOfWork.Complete();

                //Attachment file
                _attachmemt.UploadFile(model.AttachmentList, ca.Id, CaViewModel.ProcessCode, ca.Cano);

                _elastic.Insert(this.InitialCAListViewModel(ca), ConstantValue.CAIndex, ConstantValue.CAType);

                scope.Complete();
            }

            return response;
        }

        /// <summary>
        /// Update Ca and upload file.
        /// </summary>
        /// <param name="model">The infomation ca.</param>
        /// <returns></returns>
        public ValidationResultViewModel Edit(CaViewModel model)
        {
            var response = ValidateData(model);
            if (response.ErrorFlag)
            {
                return response;
            }
            using (var scope = new TransactionScope())
            {
                var ca = _mapper.Map<CaViewModel, DS.Data.Pocos.Ca>(model);
                ca.Status = ConstantValue.TransStatusSaved;
                ca.LastModifyBy = _manageToken.EmpNo;
                ca.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<DS.Data.Pocos.Ca>().Update(ca);
                _unitOfWork.Complete();

                //Attachment file
                _attachmemt.UploadFile(model.AttachmentList, ca.Id, CaViewModel.ProcessCode, ca.Cano);

                _elastic.Update(this.InitialCAListViewModel(ca), ConstantValue.CAIndex, ConstantValue.CAType);

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
            var result = UtilityService.ValidateStringLength<DS.Data.Pocos.Ca, CaViewModel>(model);

            if (string.IsNullOrWhiteSpace(model.RequestFor))
            {
                var property = model.GetType().GetProperty(nameof(model.RequestFor));
                result.ModelStateErrorList.Add(new ModelStateError
                {
                    Key = property.Name,
                    Message = AppText.PleaseFill + property.Name
                });
            }

            if (result.ModelStateErrorList.Count > 0)
            {
                result.ErrorFlag = true;
                result.Message = "Error";
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
                                                                                      (q.Terms(t => t.Field(f => f.CreateBy).Terms(_manageToken.EmpNo)) ||
                                                                                       q.Terms(t => t.Field(f => f.RequestFor).Terms(_manageToken.EmpNo)) ||
                                                                                       q.Terms(t => t.Field(f => f.RequestOrg).Terms(_manageToken.Org)) ||
                                                                                       q.Terms(t => t.Field(f => f.CreateOrg).Terms(_manageToken.Org)))
                                                                                    &&
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

        /// <summary>
        /// Initial Ca Model to Ca Search Model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private CaSearchViewModel InitialCAListViewModel(Ca model)
        {
            var status = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.VALUE_HELP_STATUS &&
                                                                              x.ValueKey == model.Status).FirstOrDefault();

            var result = new CaSearchViewModel
            {
                ID = model.Id,
                CANo = UtilityService.DataOrDefault(model.Cano),
                ComCode = model.ComCode,
                FundIOSAP = UtilityService.DataOrDefault(model.FundSap),
                Amount = model.Amount.HasValue ? model.Amount.Value.ToString("0.##") : string.Empty,
                AmountText = UtilityService.DataOrDefault(model.Amount),
                DueDate = UtilityService.DataOrDefault(model.DueDate, ConstantValue.DATETIME_DAYMONTHYEAR),
                DueDateText = UtilityService.DataOrDefault(model.DueDate, ConstantValue.DATETIME_DAYMONTHTEXTYEAR),
                DueDateSort = model.DueDate,
                RequireDate = UtilityService.DataOrDefault(model.RequireDate, ConstantValue.DATETIME_DAYMONTHYEAR),
                RequireDateText = UtilityService.DataOrDefault(model.RequireDate, ConstantValue.DATETIME_DAYMONTHTEXTYEAR),
                RequireDateSort = model.RequireDate,
                Status = model.Status,
                StatusText = status.ValueText,
                ReceiveDate = UtilityService.DataOrDefault(model.ReceiveDate, ConstantValue.DATETIME_DAYMONTHYEAR),
                ReceiveDateText = UtilityService.DataOrDefault(model.ReceiveDate, ConstantValue.DATETIME_DAYMONTHTEXTYEAR),
                ReceiveDateSort = model.ReceiveDate,
                RequestFor = model.RequestFor,
                RequestPos = model.RequestPos,
                RequestOrg = model.RequestOrg,
                CreateBy = model.CreateBy,
                CreatePos = model.CreatePos,
                CreateOrg = model.CreateOrg,
                //CreateByText = HRService.GetEmployee(item.RequestFor)
            };

            result = this.InitialApprove(result, model);

            return result;
        }

        /// <summary>
        /// Initial approve to elastic search model.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private CaSearchViewModel InitialApprove(CaSearchViewModel result, Ca item)
        {
            var workflowLog = new List<WorkflowActivityLog>();
            var process = _unitOfWork.GetRepository<WorkflowProcessInstance>().Get(x => x.DataId == item.Id && 
                                                                                        x.ProcessCode == CaViewModel.ProcessCode).FirstOrDefault();
            if (process != null)
            {
                workflowLog = _unitOfWork.GetRepository<WorkflowActivityLog>().Get(x => x.ProcessInstanceId == process.ProcessInstanceId).ToList();
            }
            workflowLog = workflowLog.Where(x => x.Step > 1).ToList();
            foreach (var workItem in workflowLog)
            {
                result = SetApproveElastic(result, workItem);
            }
            return result;
        }

        /// <summary>
        /// Set property approve elastic search.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="workItem"></param>
        /// <returns></returns>
        private CaSearchViewModel SetApproveElastic(CaSearchViewModel result, WorkflowActivityLog workItem)
        {
            if (string.IsNullOrEmpty(result.Approver01))
            {
                result.Approver01 = workItem.ActionUser;
            }
            else if (string.IsNullOrEmpty(result.Approver02))
            {
                result.Approver02 = workItem.ActionUser;
            }
            else if (string.IsNullOrEmpty(result.Approver03))
            {
                result.Approver03 = workItem.ActionUser;
            }
            else if (string.IsNullOrEmpty(result.Approver04))
            {
                result.Approver04 = workItem.ActionUser;
            }
            else if (string.IsNullOrEmpty(result.Approver05))
            {
                result.Approver05 = workItem.ActionUser;
            }
            else if (string.IsNullOrEmpty(result.Approver06))
            {
                result.Approver06 = workItem.ActionUser;
            }
            else if (string.IsNullOrEmpty(result.Approver07))
            {
                result.Approver07 = workItem.ActionUser;
            }
            else if (string.IsNullOrEmpty(result.Approver08))
            {
                result.Approver08 = workItem.ActionUser;
            }
            else if (string.IsNullOrEmpty(result.Approver09))
            {
                result.Approver09 = workItem.ActionUser;
            }
            else if (string.IsNullOrEmpty(result.Approver10))
            {
                result.Approver10 = workItem.ActionUser;
            }
            return result;
        }

        #endregion

    }
}
