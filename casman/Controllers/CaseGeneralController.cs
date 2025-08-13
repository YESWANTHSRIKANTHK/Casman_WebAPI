using casman.Models;
using casman.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class CaseGeneralController : ControllerBase
{
    private readonly ICaseRepository _repository;

    public CaseGeneralController(ICaseRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("details")]
    public async Task<ActionResult<CaseGeneralDetailDto>> GetCaseGeneralDetail(string caseId, string subId)
    {
        var detail = await _repository.GetCaseGeneralDetailAsync(caseId, subId);
        if (detail == null)
        {
            return NotFound();
        }
        return Ok(detail);
    }

    [HttpGet("case-types")]
    public async Task<IActionResult> GetCaseTypes()
    {
        var result = await _repository.GetCaseTypesAsync();
        return Ok(result);
    }


    [HttpGet("mdu-liability")]
    public async Task<IActionResult> GetMduLiabilities([FromQuery] string? caseType)
    {
        var data = await _repository.GetMduLiabilitiesAsync(caseType);
        return Ok(data);
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments([FromQuery] string tableName, [FromQuery] string? value)
    {
        var result = await _repository.GetDepartmentsAsync(tableName, value);
        return Ok(result);
    }
    [HttpGet("handler1")]
    public async Task<IActionResult> GetCaseHandler1([FromQuery] string tableName, [FromQuery] string? value = null)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            return BadRequest("tableName parameter is required.");
        }

        var data = await _repository.GetCaseHandler1Async(tableName, value);
        return Ok(data);
    }
    [HttpGet("categories")]
    public async Task<ActionResult<List<CategoryDto>>> Get()
    {
        var data = await _repository.GetCategoryDropdownAsync();
        return Ok(data);
    }
    [HttpGet("practice-areas")]
    public async Task<IActionResult> GetPracticeAreas()
    {
        var result = await _repository.GetPracticeAreaDropdownAsync();
        return Ok(result);
    }

    [HttpPut("update-case")]
    public async Task<IActionResult> UpdateCaseAsync([FromBody] CaseEditGeneralDto dto)
    {
        if (dto == null) return BadRequest("Invalid data.");

        try
        {
            await _repository.UpdateCaseAsync(dto);
            return Ok(new { message = "Case updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }




}


