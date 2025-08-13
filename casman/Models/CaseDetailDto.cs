using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace casman.Models
{
    public class CaseDetailDto
    {
        [Required]
        [Column("case_id")]
        public string CaseId { get; set; }

        [Column("subsid_id")]
        public string SubId { get; set; }
        //[Column("case_stat")]
        //public string Case_Status { get; set; }
        [Column("shared_case")]
        public string Shared_Case { get; set; }
        [Column("stat_desc")]
        public string STAT_DESC { get; set; }
        [Column("prac_last_name")]
        public string PRAC_LAST_NAME { get; set; }
        [Column("staff_name")]
        public string STAFF_NAME { get; set; }
        [Column("liab_desc")]
        public string LIAB_DESC { get; set; }
        [Column("prac_num")]
        public string PRAC_NUM { get; set; }

        public string EXTSOL { get; set; }

        public string PATIENT { get; set; }
        [Column("prodType")]
        public int prodType { get; set; }

    }
}
