using Microsoft.AspNetCore.Mvc;
using casman.Models;
using casman.Repositories;

namespace casman.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseSearchController : ControllerBase
    {
        private readonly ICaseRepository _repository;

        // Inject ICaseRepository, NOT CaseDbContext
        public CaseSearchController(ICaseRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaseDetailDto>>> GetCaseDetails([FromQuery] string caseId, [FromQuery] string? subId)
        {
            if (string.IsNullOrEmpty(caseId))
                return BadRequest("Case ID is required.");

            var result = await _repository.GetCaseDetailsAsync(caseId, subId);
            return Ok(result);
        }
    }
}
