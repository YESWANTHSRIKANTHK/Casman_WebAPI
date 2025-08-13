using System.ComponentModel.DataAnnotations.Schema;

namespace casman.Models
{
    public class IndemnifierDto
    {
        [Column("def_org")]
        public string DefOrg { get; set; }

        [Column("def_org_name")]
        public string DefOrgName { get; set; }

        [Column("valid")]
        public string Valid { get; set; }
    }

}
