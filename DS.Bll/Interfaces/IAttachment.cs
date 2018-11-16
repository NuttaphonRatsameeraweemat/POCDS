using DS.Bll.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Interfaces
{
    public interface IAttachment
    {
        ValidationResultViewModel UploadFile(IFormFileCollection model, int dataId, string processCode, string folder1 = "");
    }
}
