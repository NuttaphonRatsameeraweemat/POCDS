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
        public double FileSizeMB { get; set; }
        public byte[] FileContent { get; set; }
        public string FileBase64 { get; set; }
        public string MimeType { get; set; }
        public DateTime AttachDate { get; set; }
        public string AttachBy { get; set; }
        public string DownloadURL { get; set; }
        public string SavedFileName { get; set; }
        public string FileExtension { get; set; }
        public string FileUniqueKey { get; set; }
        public bool ErrorFlag { get; set; }
        public bool DeleteFlag { get; set; }
        public bool ReadOnlyFlag { get; set; }
        public string DocumentFilePath { get; set; }

        public string ProcessCode { get; set; }
        public string DataKey { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string Remark { get; set; }
        public string AttachmentGroup { get; set; }

        public string DocumentTypeValueType { get; set; }
    }
}
