using AutoMapper;
using DS.Bll.Interfaces;
using DS.Bll.Models;
using DS.Data.Repository.Interfaces;
using DS.Helper.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using DS.Bll.Context;
using System.Transactions;

namespace DS.Bll
{
    public class Attachment : IAttachment
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
        /// The Configuration Value.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The httpcontext.
        /// </summary>
        private readonly HttpContext _httpContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Attachment" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="log">The Log Manager.</param>
        /// <param name="configuration">The Configuration Value</param>
        public Attachment(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Upload file to temp folder.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ValidationResultViewModel UploadFile(List<AttachmentViewModel> model, int dataId, string processCode, string folder1 = "")
        {
            var result = new ValidationResultViewModel();
            string documentPath = GetDocumentFilePath(processCode, folder1);
            var attachDate = DateTime.Now;
            this.CreateDirectory(processCode, folder1);
            foreach (var item in model)
            {
                if (!this.IsDelete(item,documentPath))
                {
                    this.SaveAttachment(item, dataId, attachDate, processCode, documentPath);
                }
            }
            _unitOfWork.Complete();
            return result;
        }

        /// <summary>
        /// Delete file from local and row.
        /// </summary>
        /// <param name="item">The attachment data.</param>
        /// <param name="documentPath">The document path file.</param>
        /// <returns></returns>
        private bool IsDelete(AttachmentViewModel item, string documentPath)
        {
            bool result = false;
            if (item.IsDelete)
            {
                result = true;
                var attach = _unitOfWork.GetRepository<DS.Data.Pocos.Attachment>().GetById(item.ID);
                if (File.Exists(Path.Combine(documentPath,attach.SavedFileName)))
                {
                    File.Delete(Path.Combine(documentPath, attach.SavedFileName));
                }
            }
            return result;
        }

        /// <summary>
        /// Save file to local and insert to database.
        /// </summary>
        /// <param name="item">The attachment data.</param>
        /// <param name="dataId">The identity id of document.</param>
        /// <param name="attachDate">The attachDate.</param>
        /// <param name="processCode">The menu key.</param>
        /// <param name="documentPath">The document path file.</param>
        private void SaveAttachment(AttachmentViewModel item, int dataId, DateTime attachDate, string processCode, string documentPath)
        {
            if (item.ID == 0)
            {
                var uniqueKey = DateTime.Now.ToString(ConstantValue.DateTimeFormat);
                var attachment = new DS.Data.Pocos.Attachment
                {
                    AttachBy = _httpContext.User.Identity.Name ?? null,
                    AttachDate = attachDate,
                    DataKey = dataId.ToString(),
                    FileExtension = Path.GetExtension(item.FileName),
                    FileName = item.FileName,
                    FileSize = item.FileSize,
                    FileUniqueKey = uniqueKey,
                    ProcessCode = processCode,
                    SavedFileName = string.Format("{0}_{1}", uniqueKey, item.FileName)
                };
                var file = Convert.FromBase64String(item.FileBase64);
                string savePath = Path.Combine(documentPath, attachment.SavedFileName);
                File.WriteAllBytes(savePath, file);

                _unitOfWork.GetRepository<DS.Data.Pocos.Attachment>().Add(attachment);
            }
        }

        /// <summary>
        /// Get document path file.
        /// </summary>
        /// <param name="processCode">The menu key.</param>
        /// <param name="folder1">The sub folder</param>
        /// <returns></returns>
        public string GetDocumentFilePath(string processCode, string folder1)
        {
            string result = Path.Combine(Directory.GetCurrentDirectory(),
                                        _configuration["Directory:DocumentFilePath"],
                                        processCode,
                                        folder1);
            return result;
        }

        /// <summary>
        /// Create Directory for save file.
        /// </summary>
        /// <param name="processCode">The menu key</param>
        /// <param name="folder1">The sub folder.</param>
        private void CreateDirectory(string processCode, string folder1)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                                        _configuration["Directory:DocumentFilePath"]);
            if (!Directory.Exists(Path.Combine(path, processCode)))
            {
                Directory.CreateDirectory(Path.Combine(path, processCode));
            }
            if (!Directory.Exists(Path.Combine(path, processCode, folder1)))
            {
                Directory.CreateDirectory(Path.Combine(path, processCode, folder1));
            }
        }

        #endregion

    }
}
