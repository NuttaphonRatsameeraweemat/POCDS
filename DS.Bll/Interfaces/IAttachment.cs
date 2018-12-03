using DS.Bll.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Interfaces
{
    public interface IAttachment
    {
        void UploadFile(List<AttachmentViewModel> model, int dataId, string processCode, string folder1 = "");
        List<AttachmentViewModel> GetFile(int dataId, string processCode);
        void RemoveFile(int dataId, string processCode, string folder1 = "");
    }
}
