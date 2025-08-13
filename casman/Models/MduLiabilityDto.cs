using System.ComponentModel.DataAnnotations.Schema;

namespace casman.Models
{
    public class MduLiabilityDto
{
    public string Mdu_Liability { get; set; }
        [Column("liab_desc")]
        public string Liab_Desc { get; set; }

        public string Valid { get; set; }
    public string Case_Type { get; set; }
}
}
