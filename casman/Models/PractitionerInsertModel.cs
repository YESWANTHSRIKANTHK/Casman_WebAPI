namespace casman.Models
{
    public class PractitionerInsertModel
    {
        public string CaseId { get; set; }
        public string SubId { get; set; }
        public string PtNum { get; set; }
        public short PracRole { get; set; }
        public short PracDefOrg { get; set; }
        public string PracLastName { get; set; }
        public string PracFirstName { get; set; }
        public string PracInit { get; set; }
        public string PracSex { get; set; }
        public short PracTow { get; set; }
        public short PracInvl { get; set; }
        public DateTime? DateInvolved { get; set; }
        public DateTime? DateNotified { get; set; }
        public DateTime? DateClaimMade { get; set; }
        public DateTime? DateNonIrishClaimMade { get; set; }
        public short PracSource { get; set; }
        public short CoverType { get; set; }
        public string UserId { get; set; }
        //public string PracSeqNum { get; set; }

        public string DdrApplicable { get; set; }
        public string DdrCovertype { get; set; }
        public string CharmPolicyNumber { get; set; }
        public string CharmPolicyVersion { get; set; }
        public string GroupTypeDesc { get; set; }
        public string Groupnum { get; set; }
        public string Organisation { get; set; }
        public string MemberEntitled { get; set; }
        public string ProdRefNo_SubSegCode { get; set; }
        public string DonPolSelection { get; set; }
        public string PrivateGPClaims { get; set; }    
    }
}
