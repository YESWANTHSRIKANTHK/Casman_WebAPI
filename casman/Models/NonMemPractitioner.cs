using System.ComponentModel.DataAnnotations.Schema;

namespace casman.Models
{
    public class NonMemPractitioner
    {
        [Column("gmc_num")]
        public string PracNumber { get; set; }
        [Column("intls")]
        public string Initials { get; set; }
        [Column("last_name")]
        public string SurName { get; set; }
        [Column("first_name")]
        public string ForeName { get; set; }
        [Column("sex")]
        public string Sex { get; set; }
    }
}
