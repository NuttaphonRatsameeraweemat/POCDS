using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DS.Data.Pocos
{
    [Table("HRCompany", Schema = "ds")]
    public partial class Hrcompany
    {
        [Key]
        [StringLength(10)]
        public string ComCode { get; set; }
        [Column("SAPComCode")]
        [StringLength(10)]
        public string SapcomCode { get; set; }
        [StringLength(10)]
        public string ShortText { get; set; }
        [StringLength(40)]
        public string LongText { get; set; }
        public DateTime? LastInterface { get; set; }
        [StringLength(40)]
        public string Email { get; set; }
    }
}
