using DS.Bll.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Interfaces
{
    public interface IAttachment
    {
        ValidationResultViewModel UploadFile(List<AttachmentViewModel> model, int dataId, string processCode, string folder1 = "");
    }
}
