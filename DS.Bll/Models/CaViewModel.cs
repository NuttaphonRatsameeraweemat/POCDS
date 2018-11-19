using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Models
{
    public class CaViewModel
    {
        public const string ProcessCode = "CAProcess";

        public int Id { get; set; }
        public string CANo { get; set; }
        public DateTime? CADate { get; set; }
        public string FundSAP { get; set; }
        public string IOSAP { get; set; }
        public string CostCenter { get; set; }
        public string ReserveBudget { get; set; }
        public string InternalMemoNo { get; set; }
        public string Objective { get; set; }
        public string ObjectiveDesc { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public DateTime? RequireDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string ReceiveID { get; set; }
        public string ReceiveName { get; set; }
        public DateTime? DueDate { get; set; }
        public string BusinessPlace { get; set; }
        public string PaymentPlace { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestBy { get; set; }
        public string RequestFor { get; set; }
        public string RequestOrg { get; set; }
        public string RequestPos { get; set; }
        public string SAPMessage { get; set; }
        public string OrgName { get; set; }
        public string CreateBy { get; set; }
        public string CreateByText { get; set; }
        public string ReceiveType { get; set; }
        public string ReceiveRemark { get; set; }
        public string StatusText { get; set; }
        //Attachment List
        public List<AttachmentViewModel> AttachmentList { get; set; }
    }
}
