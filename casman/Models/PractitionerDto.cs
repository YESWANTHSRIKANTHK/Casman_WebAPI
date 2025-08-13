using System.ComponentModel.DataAnnotations.Schema;

namespace casman.Models
{
    public class PractitionerDto
    {
        [Column("role_desc")]
        public string? Role { get; set; }  // Nullable string

        [Column("prac_seq_num")]
        public short? PracSeqNum { get; set; }  // Nullable short

        [Column("prac_num")]
        public string? PracNum { get; set; }

        [Column("prac_last_name")]
        public string? PracLastName { get; set; }

        [Column("prac_first_name")]
        public string? PracFirstName { get; set; }

        [Column("def_org_name")]
        public string? DefOrgName { get; set; }

        [Column("prac_invl")]
        public short? PracInvl { get; set; }

        [Column("cover_type_desc")]
        public string? CoverTypeDesc { get; set; }

        [Column("corp_status")]
        public string? CorpStatus { get; set; }
      
    }
}
