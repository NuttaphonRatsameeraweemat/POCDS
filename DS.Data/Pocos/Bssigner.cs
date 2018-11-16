using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DS.Data.Pocos
{
    [Table("BSSigner", Schema = "ds")]
    public partial class Bssigner
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(11)]
        public string Signer { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
