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
    public class AttachmentBll : IAttachment
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The Configuration Value.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The manage payload on token.
        /// </summary>
        private readonly IManageToken _manageToken;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="configuration">The configuration value.</param>
        /// <param name="manageToken">The manage token payload value.</param>
        public AttachmentBll(IUnitOfWork unitOfWork, IConfiguration configuration, IManageToken manageToken)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _manageToken = manageToken;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Upload file to temp folder.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dataId">The identity data id file.</param>
        /// <param name="processCode">The process code.</param>
        /// <param name="folder1">The subfolder.</param>
        /// <returns></returns>
        public void UploadFile(List<AttachmentViewModel> model, int dataId, string processCode, string folder1 = "")
        {
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
        }

        /// <summary>
        /// Get file list.
        /// </summary>
        /// <param name="dataId">The identity data id file.</param>
        /// <param name="processCode">The process code.</param>
        /// <returns></returns>
        public List<AttachmentViewModel> GetFile(int dataId, string processCode)
        {
            List<AttachmentViewModel> result = new List<AttachmentViewModel>();

            var attachList = _unitOfWork.GetRepository<DS.Data.Pocos.Attachment>().Get(x => x.DataKey == dataId.ToString()).ToList();

            foreach (var item in attachList)
            {
                result.Add(new AttachmentViewModel { ID = item.Id, FileName = item.FileName, IsDelete = false });
            } 

            return result;
        }

        /// <summary>
        /// Remove all file with data id.
        /// </summary>
        /// <param name="dataId">The identity data id file</param>
        /// <param name="processCode">The process code.</param>
        /// <param name="folder1">The sub folder.</param>
        public void RemoveFile(int dataId, string processCode, string folder1 = "")
        {
            string documentPath = GetDocumentFilePath(processCode, folder1);
            var attachList = _unitOfWork.GetRepository<DS.Data.Pocos.Attachment>().Get(x => x.DataKey == dataId.ToString()).ToList();
            if (Directory.Exists(documentPath))
            {
                Directory.Delete(documentPath, true);
            }
            if (attachList.Count > 0)
            {
                _unitOfWork.GetRepository<DS.Data.Pocos.Attachment>().RemoveRange(attachList);
                _unitOfWork.Complete();
            }
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
                if (attach != null && File.Exists(Path.Combine(documentPath,attach.SavedFileName)))
                {
                    //Remove file in local storage.
                    File.Delete(Path.Combine(documentPath, attach.SavedFileName));
                    //Remove Row in database.
                    _unitOfWork.GetRepository<DS.Data.Pocos.Attachment>().Remove(attach);
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
                var uniqueKey = DateTime.Now.ToString(ConstantValue.DATETIME_YEARMONTHDAYTIME);
                var attachment = new DS.Data.Pocos.Attachment
                {
                    AttachBy = _manageToken.EmpNo,
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
