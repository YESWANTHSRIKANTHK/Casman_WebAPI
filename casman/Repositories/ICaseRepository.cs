using casman.Models;

namespace casman.Repositories
{
    public interface ICaseRepository
    {
        Task<List<IndemnifierDto>> GetIndemnifiersAsync();
        List<SpecialityDto> GetSpecialities();
        Task CreateNewCaseAsync(CreateCaseDto dto);

        Task<List<CaseDetailDto>> GetCaseDetailsAsync(string caseId, string? subId);
        Task<List<PractitionerDto>> GetPractitionersAsync(string caseId, string subId);

        Task<NonMemPractitioner> GetNonMemberDetailsAsync(string pracNumber);

        Task<(string message, string? pracSeqNum)> InsertPractitionerAsync(PractitionerInsertModel model);
        Task<CaseGeneralDetailDto?> GetCaseGeneralDetailAsync(string caseId, string subId);
        Task<List<CaseTypeDto>> GetCaseTypesAsync();
        Task<List<MduLiabilityDto>> GetMduLiabilitiesAsync(string? caseType);

        Task<List<DepartmentDto>> GetDepartmentsAsync(string tableName, string? value);
        Task<List<CaseHandlerDto>> GetCaseHandler1Async(string tableName, string? value);
        Task<List<CategoryDto>> GetCategoryDropdownAsync();
        Task<List<PracticeAreaDto>> GetPracticeAreaDropdownAsync();
        Task UpdateCaseAsync(CaseEditGeneralDto dto);

    }
}
