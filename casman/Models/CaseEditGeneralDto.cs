namespace casman.Models
{
    public class CaseEditGeneralDto
    {
        public string CaseId { get; set; }
        public string SubId { get; set; }                // @SUBID
        public string NCaseType { get; set; }            // @NCASE_TYPE => CASE_TYPE
        public string LiabilityDesc { get; set; }        // @MDU_LIABILITY
        public bool SharedCase { get; set; }             // @SHARED_CASE
        public bool RedactCase { get; set; }             // @REDACT_CASE
        public DateTime? IncdtDate { get; set; }
        // @NEW_INCDT_DATE
        public DateTime? DateClaim { get; set; }            // @NEW_CLAIM_DATE
        public DateTime? OpenDate { get; set; }             // @NEW_OPEN_DATE
        public string DeptId { get; set; }               // @DEPARTMENT => MDU_UNIT
        public string StaffId1 { get; set; }             // @SCRT_USR1
        public string StaffId2 { get; set; }             // @SCRT_USR2
        public string StaffId3 { get; set; }             // @SCRT_USR3
        public string Category { get; set; }             // @CASE_CAT
        public string SpcltyDesc { get; set; }           // @SPCLTY_MAJ_CODE
        public string PracAreaDesc { get; set; }         // @PRAC_AREA_CODE
        public string ClassDesc { get; set; }            // @CASE_CLASS
        public string AreaCode { get; set; }             // @AREA_CODE
        public string HptlLoc { get; set; }              // @HPTL_LOC
        public string FacPostcode { get; set; }          // @FAC_POSTCODE
        public string MFicheNo { get; set; }             // @MFICHE_NO
        public int LockNo { get; set; }               // @NEWLOCKNO
        public string UserId { get; set; }               // @USER_ID
        public string CaseStatus { get; set; }           // @CASE_STAT
        public string CaseDocumentStatus { get; set; }   // @CASE_DOCUMENT_STATUS
        public string LegalCaseDocumentStatus { get; set; } // @LEGAL_CASE_DOCUMENT_STATUS
        public string DoiStatus { get; set; }            // @DOI_Status
        public string BarcodeId { get; set; }

    }

}
