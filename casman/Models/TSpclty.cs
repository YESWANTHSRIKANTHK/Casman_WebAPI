using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace casman.Models
{
    [Table("t_spclty")]
    public class TSpclty
    {
        [Key]
        [Column("spclty_code")]
        public string SpcltyCode { get; set; }

        [Column("spclty_desc")]
        public string SpcltyDesc { get; set; }

        [Column("claims_use")]
        public string ClaimsUse { get; set; }
    }
}
