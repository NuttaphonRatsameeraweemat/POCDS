using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Models
{
    public class CaSearchViewModel
    {
        [Number(Name = "id")]
        public int ID { get; set; }
        [Text(Name = "cano")]
        public string CANo { get; set; }
        [Text(Name = "comcode")]
        public string ComCode { get; set; }
        [Text(Name = "fundiosap")]
        public string FundIOSAP { get; set; }
        [Text(Name = "requiredate")]
        public string RequireDate { get; set; }
        [Text(Name = "requiredatetext")]
        public string RequireDateText { get; set; }
        [Text(Name = "requiredatesort")]
        public DateTime? RequireDateSort { get; set; }
        [Text(Name = "duedate")]
        public string DueDate { get; set; }
        [Text(Name = "duedatetext")]
        public string DueDateText { get; set; }
        [Text(Name = "duedatesort")]
        public DateTime? DueDateSort { get; set; }
        [Text(Name = "amount")]
        public string Amount { get; set; }
        [Text(Name = "amounttext")]
        public string AmountText { get; set; }
        [Text(Name = "status")]
        public string Status { get; set; }
        [Text(Name = "statustext")]
        public string StatusText { get; set; }
        [Text(Name = "receivedate")]
        public string ReceiveDate { get; set; }
        [Text(Name = "receivedatetext")]
        public string ReceiveDateText { get; set; }
        [Text(Name = "receivedatesort")]
        public DateTime? ReceiveDateSort { get; set; }
        [Text(Name = "requestfor")]
        public string RequestFor { get; set; }
        [Text(Name = "requestpos")]
        public string RequestPos { get; set; }
        [Text(Name = "requestorg")]
        public string RequestOrg { get; set; }
        [Text(Name = "createby")]
        public string CreateBy { get; set; }
        [Text(Name = "createbytext")]
        public string CreateByText { get; set; }
        [Text(Name = "createpos")]
        public string CreatePos { get; set; }
        [Text(Name = "createorg")]
        public string CreateOrg { get; set; }
        [Text(Name = "approver01")]
        public string Approver01 { get; set; }
        [Text(Name = "approver02")]
        public string Approver02 { get; set; }
        [Text(Name = "approver03")]
        public string Approver03 { get; set; }
        [Text(Name = "approver04")]
        public string Approver04 { get; set; }
        [Text(Name = "approver05")]
        public string Approver05 { get; set; }
        [Text(Name = "approver06")]
        public string Approver06 { get; set; }
        [Text(Name = "approver07")]
        public string Approver07 { get; set; }
        [Text(Name = "approver08")]
        public string Approver08 { get; set; }
        [Text(Name = "approver09")]
        public string Approver09 { get; set; }
        [Text(Name = "approver10")]
        public string Approver10 { get; set; }
        [Text(Name = "lastapprover")]
        public string LastApprover { get; set; }
        [Text(Name = "cadate")]
        public string CADate { get; set; }
        [Text(Name = "cadatetext")]
        public string CADateText { get; set; }
       
    }
}
