using casman.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace casman.Repositories
{
    public class CaseRepository : ICaseRepository
    {
        private readonly CaseDbContext _context;

        public CaseRepository(CaseDbContext context)
        {
            _context = context;
        }

        public async Task<List<IndemnifierDto>> GetIndemnifiersAsync()
        {
            var tablenameParam = new SqlParameter("@TABLENAME", "t_def_org");
            var valueParam = new SqlParameter("@VALUE", DBNull.Value);

            return await _context.Set<IndemnifierDto>()
                .FromSqlRaw("EXEC dbo.CMS_SP_MASTER_RTR @TABLENAME, @VALUE", tablenameParam, valueParam)
                .ToListAsync();
        }

        public async Task<List<CaseDetailDto>> GetCaseDetailsAsync(string caseId, string? subId)
        {
            var caseIdParam = new SqlParameter("@CASEID", caseId ?? (object)DBNull.Value);
            var subIdParam = new SqlParameter("@SUBID", string.IsNullOrEmpty(subId) ? (object)DBNull.Value : subId);

            return await _context.Set<CaseDetailDto>()
                .FromSqlRaw("EXEC CD_SP_GETCNXT_RTR @CASEID, @SUBID", caseIdParam, subIdParam)
                .ToListAsync();
        }

        public async Task<List<PractitionerDto>> GetPractitionersAsync(string caseId, string subId)
        {
            var caseIdParam = new SqlParameter("@CASEID", caseId ?? (object)DBNull.Value);
            var subIdParam = new SqlParameter("@SUBID", string.IsNullOrEmpty(subId) ? (object)DBNull.Value : subId);

            return await _context.Set<PractitionerDto>()
                .FromSqlRaw("EXEC CD_SP_GETPRAC_RTR @CASEID, @SUBID", caseIdParam, subIdParam)
                .ToListAsync();
        }


        public List<SpecialityDto> GetSpecialities()
        {
            var results = _context.TSpclty
                .Select(s => new SpecialityDto
                {
                    SpcltyCode = s.SpcltyCode,
                    SpcltyDesc = s.ClaimsUse == "N" ? "~" + s.SpcltyDesc : s.SpcltyDesc,
                    ClaimsUse = s.ClaimsUse
                })
                .ToList();

            results.Insert(0, new SpecialityDto
            {
                SpcltyCode = "Choose",
                SpcltyDesc = "",
                ClaimsUse = "Y"
            });

            return results
                .OrderByDescending(r => r.ClaimsUse)
                .ThenBy(r => r.SpcltyDesc)
                .ToList();
        }

        public async Task CreateNewCaseAsync(CreateCaseDto dto)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            using (var command = new SqlCommand("ADM_SP_CREATENEWCASE_PRI", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@prac_num", DBNull.Value);
                command.Parameters.AddWithValue("@prac_role", dto.Role ?? "1");
                command.Parameters.AddWithValue("@prac_last_name", dto.Surname);
                command.Parameters.AddWithValue("@prac_first_name", string.IsNullOrWhiteSpace(dto.FirstName) ? (object)DBNull.Value : dto.FirstName);
                command.Parameters.AddWithValue("@prac_init", string.IsNullOrWhiteSpace(dto.Initials) ? (object)DBNull.Value : dto.Initials);
                command.Parameters.AddWithValue("@prac_sex", string.IsNullOrWhiteSpace(dto.Sex) ? (object)DBNull.Value : dto.Sex);
                command.Parameters.AddWithValue("@prac_def_org", dto.DefOrg);
                command.Parameters.AddWithValue("@prac_tow", DBNull.Value);
                command.Parameters.AddWithValue("@prac_source", 1);
                command.Parameters.AddWithValue("@userid", string.IsNullOrWhiteSpace(dto.UserId) ? "vignesh." : dto.UserId);
                command.Parameters.AddWithValue("@Case_Create_Source", "Casman");

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<NonMemPractitioner> GetNonMemberDetailsAsync(string pracNumber)
        {
            var result = new NonMemPractitioner();

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var command = new SqlCommand("ADM_SP_NONMEMBERDETAILS_RTR", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@gmc_num", pracNumber);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                result.PracNumber = reader["gmc_num"]?.ToString();
                result.Initials = reader["intls"]?.ToString();
                result.SurName = reader["last_name"]?.ToString();
                result.ForeName = reader["first_name"]?.ToString();
                result.Sex = reader["sex"]?.ToString();
            }
            else
            {
                throw new KeyNotFoundException("Practitioner not found.");
            }

            return result;
        }

        private object GetValidSqlDate(DateTime? date)
        {
            DateTime sqlMinDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            if (!date.HasValue || date.Value < sqlMinDate)
                return DBNull.Value;

            return date.Value;
        }

        public async Task<(string message, string? pracSeqNum)> InsertPractitionerAsync(PractitionerInsertModel model)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var command = new SqlCommand("CD_SP_PRACDETAILS_PRU", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CASEID", model.CaseId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SUBID", model.SubId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PTNUM", model.PtNum ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PRAC_ROLE", model.PracRole);
            command.Parameters.AddWithValue("@PRAC_DEF_ORG", model.PracDefOrg);
            command.Parameters.AddWithValue("@PRAC_LAST_NAME", model.PracLastName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PRAC_FIRST_NAME", model.PracFirstName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PRAC_INIT", model.PracInit ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PRAC_SEX", model.PracSex ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PRAC_TOW", model.PracTow);
            command.Parameters.AddWithValue("@PRAC_INVL", model.PracInvl);
            command.Parameters.AddWithValue("@DATE_INVOLVED", GetValidSqlDate(model.DateInvolved));
            command.Parameters.AddWithValue("@DATE_NOTIFIED", GetValidSqlDate(model.DateNotified));
            command.Parameters.AddWithValue("@DATE_CLAIM_MADE", GetValidSqlDate(model.DateClaimMade));
            command.Parameters.AddWithValue("@DATE_NONIRISH_CLAIM_MADE", GetValidSqlDate(model.DateNonIrishClaimMade));
            command.Parameters.AddWithValue("@PRAC_SOURCE", model.PracSource);
            command.Parameters.AddWithValue("@COVER_TYPE", model.CoverType);
            command.Parameters.AddWithValue("@USER_ID", model.UserId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@LOCK_NO", 0);
            command.Parameters.AddWithValue("@DDRAPPLICABLE", model.DdrApplicable ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@DDRCOVERTYPE", model.DdrCovertype ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CHARMPOLICYNUMBER", model.CharmPolicyNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CHARMPOLICYVERSION", model.CharmPolicyVersion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MDUNO", model.Groupnum ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Member_Entitled", model.MemberEntitled ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ProdRefNo_SubSegCode", model.ProdRefNo_SubSegCode ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@DON_PolSelection", model.DonPolSelection ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PrivateGPClaims", model.PrivateGPClaims ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MODE", "I");

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var message = reader["Message"]?.ToString() ?? "Insert completed.";
                string? pracSeqNum = null;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals("PracSeqNum", StringComparison.OrdinalIgnoreCase))
                    {
                        pracSeqNum = reader["PracSeqNum"]?.ToString();
                        break;
                    }
                }
                return (message, pracSeqNum);
            }

            return ("Insert completed.", null);
        }
        public async Task<CaseGeneralDetailDto?> GetCaseGeneralDetailAsync(string caseId, string subId)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var command = new SqlCommand("CD_SP_CASEGENERALDETAILS_RTR", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@CASEID", caseId);
            command.Parameters.AddWithValue("@SUBID", subId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new CaseGeneralDetailDto
                {
                    TypeDesc = reader["type_desc"]?.ToString(),
                    IncdtDate = reader["incdt_date"]?.ToString(),
                    LiabilityDesc = reader["liab_desc"]?.ToString(),
                    OpenDate = reader["open_date"]?.ToString(),
                    DateClaim = reader["date_claim"]?.ToString(),
                    SharedCase = reader["shared_case"]?.ToString() == "1",
                    RedactCase = reader["redact_case"]?.ToString() == "1",
                    DeptName = reader["dept_name"]?.ToString(),
                    StaffName1 = reader["staff_name1"]?.ToString(),
                    StaffName2 = reader["staff_name2"]?.ToString(),
                    StaffName3 = reader["staff_name3"]?.ToString(),
                    Description = reader["description"]?.ToString(),
                    SpcltyDesc = reader["spclty_desc"]?.ToString(),
                    PracAreaDesc = reader["prac_area_desc"]?.ToString(),
                    ClassDesc = reader["class_desc"]?.ToString(),
                    AreaDesc = reader["area_desc"]?.ToString(),
                    HptlLoc = reader["hptl_loc"]?.ToString(),
                    FacPostcode = reader["fac_postcode"]?.ToString(),
                    BarcodeId = reader["barcode_id"]?.ToString(),
                    MficheNo = reader["mfiche_no"]?.ToString(),
                    CaseDesc = reader["case_desc"]?.ToString(),
                    LegalCaseDocumentStatus = reader["Legal_case_Document_Status"]?.ToString(),
                    ProdType = reader["prodType"]?.ToString() == "1",
                    DOIStatus = reader["DOI_Status"]?.ToString(),
                    CaseCreateSource = reader["Case_Create_Source"]?.ToString()
                };
            }

            return null;
        }
        public async Task<List<CaseTypeDto>> GetCaseTypesAsync()
        {
            var caseTypes = new List<CaseTypeDto>();

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using (var command = new SqlCommand("CMS_SP_MASTER_RTR", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TABLENAME", "T_case_type");

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        caseTypes.Add(new CaseTypeDto
                        {
                            CaseType = reader["case_type"]?.ToString(),
                            TypeDesc = reader["type_desc"]?.ToString(),
                            Valid = reader["valid"]?.ToString()
                        });
                    }
                }
            }

            return caseTypes;
        }
        public async Task<List<MduLiabilityDto>> GetMduLiabilitiesAsync(string? caseType)
        {
            var result = new List<MduLiabilityDto>();

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            using (var command = new SqlCommand("Cms_SP_MASTER_RTR", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TABLENAME", "Mdu_liability");
                command.Parameters.AddWithValue("@value", string.IsNullOrEmpty(caseType) ? DBNull.Value : caseType);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    // Get all column names from the result set
                    var availableColumns = Enumerable.Range(0, reader.FieldCount)
                                                     .Select(reader.GetName)
                                                     .ToHashSet(StringComparer.OrdinalIgnoreCase);

                    while (await reader.ReadAsync())
                    {
                        result.Add(new MduLiabilityDto
                        {
                            Mdu_Liability = availableColumns.Contains("mdu_liability") ? reader["mdu_liability"]?.ToString() : null,
                            Liab_Desc = availableColumns.Contains("liab_desc") ? reader["liab_desc"]?.ToString() : null,
                            Valid = availableColumns.Contains("valid") ? reader["valid"]?.ToString() : null,
                            Case_Type = availableColumns.Contains("case_type") ? reader["case_type"]?.ToString() : null
                        });
                    }
                }
            }

            return result;
        }
        public async Task<List<DepartmentDto>> GetDepartmentsAsync(string tableName, string? value)
        {
            var departments = new List<DepartmentDto>();

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var command = new SqlCommand("CMS_SP_MASTER_RTR", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TABLENAME", tableName);

            if (value == null)
                command.Parameters.AddWithValue("@VALUE", DBNull.Value);
            else
                command.Parameters.AddWithValue("@VALUE", value);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                departments.Add(new DepartmentDto
                {
                    DeptId = reader["dept_id"]?.ToString()?.Trim(),
                    DeptName = reader["dept_name"]?.ToString()?.Trim(),
                    Valid = reader["valid"]?.ToString()?.Trim()
                });
            }

            return departments;
        }

        public async Task<List<CaseHandlerDto>> GetCaseHandler1Async(string tableName, string? value)
        {
            var staff = new List<CaseHandlerDto>();

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var command = new SqlCommand("CMS_SP_MASTER_RTR", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add stored procedure parameters
            command.Parameters.AddWithValue("@TABLENAME", tableName);

            if (value == null)
                command.Parameters.AddWithValue("@VALUE", DBNull.Value);
            else
                command.Parameters.AddWithValue("@VALUE", value);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                staff.Add(new CaseHandlerDto
                {
                    StaffId = reader["staff_id"]?.ToString()?.Trim(),
                    StaffName = reader["staff_name"]?.ToString()?.Trim(),
                    // Add Team or other fields here if needed, e.g.:
                    // Team = reader["team"]?.ToString()?.Trim()
                });
            }

            return staff;
        }


        public async Task<List<CategoryDto>> GetCategoryDropdownAsync()
        {
            var list = new List<CategoryDto>();

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var command = new SqlCommand("CMS_SP_MASTER_RTR", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TABLENAME", "category");
            command.Parameters.AddWithValue("@VALUE", DBNull.Value); // assuming VALUE is optional

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new CategoryDto
                {
                    Category = reader["category"]?.ToString()?.Trim(),
                    Description = reader["description"]?.ToString()?.Trim(),
                    Valid = reader["valid"]?.ToString()?.Trim()
                });
            }

            return list;
        }
        public async Task<List<PracticeAreaDto>> GetPracticeAreaDropdownAsync()
        {
            var list = new List<PracticeAreaDto>();

            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var command = new SqlCommand("CMS_SP_MASTER_RTR", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TABLENAME", "t_prac_area");
            command.Parameters.AddWithValue("@VALUE", DBNull.Value); // adjust if needed

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new PracticeAreaDto
                {
                    PracAreaCode = reader["prac_area_code"]?.ToString()?.Trim(),
                    PracAreaDesc = reader["prac_area_desc"]?.ToString()?.Trim(),
                    Valid = reader["valid"]?.ToString()?.Trim()
                });
            }

            return list;
        }
        public async Task UpdateCaseAsync(CaseEditGeneralDto dto)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var command = new SqlCommand("CD_SP_CASE_PRU", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Mandatory parameters based on your SP
            command.Parameters.AddWithValue("@CASEID", dto.CaseId ?? string.Empty);
            command.Parameters.AddWithValue("@SUBID", dto.SubId ?? string.Empty);
            command.Parameters.AddWithValue("@NCASE_TYPE", dto.NCaseType ?? string.Empty);
            command.Parameters.AddWithValue("@MDU_LIABILITY", dto.LiabilityDesc ?? string.Empty);
            command.Parameters.AddWithValue("@SHARED_CASE", dto.SharedCase);
            command.Parameters.AddWithValue("@REDACT_CASE", dto.RedactCase);
            command.Parameters.AddWithValue("@INCDT_DATE", (object?)dto.IncdtDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@DATE_CLAIM", (object?)dto.DateClaim ?? DBNull.Value);
            command.Parameters.AddWithValue("@OPEN_DATE", (object?)dto.OpenDate ?? DBNull.Value);

            command.Parameters.AddWithValue("@DEPARTMENT", dto.DeptId ?? string.Empty);
            command.Parameters.AddWithValue("@SCRT_USR1", dto.StaffId1 ?? string.Empty);
            command.Parameters.AddWithValue("@SCRT_USR2", dto.StaffId2 ?? string.Empty);
            command.Parameters.AddWithValue("@SCRT_USR3", dto.StaffId3 ?? string.Empty);
            command.Parameters.AddWithValue("@CASE_CAT", dto.Category ?? string.Empty);
            command.Parameters.AddWithValue("@SPCLTY_MAJ_CODE", dto.SpcltyDesc ?? string.Empty);
            command.Parameters.AddWithValue("@PRAC_AREA_CODE", dto.PracAreaDesc ?? string.Empty);
            command.Parameters.AddWithValue("@CASE_CLASS", dto.ClassDesc ?? string.Empty);
            command.Parameters.AddWithValue("@AREA_CODE", dto.AreaCode ?? string.Empty);
            command.Parameters.AddWithValue("@HPTL_LOC", dto.HptlLoc ?? string.Empty);
            command.Parameters.AddWithValue("@FAC_POSTCODE", dto.FacPostcode ?? string.Empty);
            command.Parameters.AddWithValue("@MFICHE_NO", dto.MFicheNo ?? string.Empty);
            command.Parameters.AddWithValue("@USER_ID", dto.UserId ?? string.Empty);
            //command.Parameters.AddWithValue("@CASE_STAT", dto.CaseStatus ?? string.Empty);
            command.Parameters.AddWithValue("@CASE_DOCUMENT_STATUS", dto.CaseDocumentStatus ?? string.Empty);
            command.Parameters.AddWithValue("@LEGAL_CASE_DOCUMENT_STATUS", dto.LegalCaseDocumentStatus ?? string.Empty);
            command.Parameters.AddWithValue("@DOI_Status", dto.DoiStatus ?? string.Empty);
            command.Parameters.AddWithValue("@LOCK_NO", (object?)dto.LockNo ?? DBNull.Value);
            // NEW


            command.Parameters.AddWithValue("@BARCODE_ID", dto.BarcodeId ?? string.Empty);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

    }
}
