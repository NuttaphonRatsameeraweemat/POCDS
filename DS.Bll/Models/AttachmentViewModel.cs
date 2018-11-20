using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Models
{
    public class AttachmentViewModel
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileBase64 { get; set; }
        public bool IsDelete { get; set; }
    }
}
