using casman.Models;
using casman.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace casman.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetPractitionerController : ControllerBase
    {
        private readonly ICaseRepository _repository;

        public GetPractitionerController(ICaseRepository repository)
        {
            _repository = repository;
        }

        // GET api/GetPractitioner?caseId=xxx&subId=yyy
        [HttpGet]
        public async Task<IActionResult> GetPractitioners(string caseId, string subId)
        {
            if (string.IsNullOrEmpty(caseId) || string.IsNullOrEmpty(subId))
                return BadRequest("caseId and subId are required.");

            var practitioners = await _repository.GetPractitionersAsync(caseId, subId);
            return Ok(practitioners);
        }

        // GET api/GetPractitioner/nonmember/{pracNumber}
        [HttpGet("nonmember/{pracNumber}")]
        public async Task<IActionResult> GetNonMemberDetails(string pracNumber)
        {
            try
            {
                var result = await _repository.GetNonMemberDetailsAsync(pracNumber);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Practitioner not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        // POST api/GetPractitioner/add
        [HttpPost("add")]
        public async Task<IActionResult> InsertPractitioner([FromBody] PractitionerInsertModel model)
        {
            if (model == null)
                return BadRequest("Practitioner data is required.");

            try
            {
                var (message, pracSeqNum) = await _repository.InsertPractitionerAsync(model);
                return Ok(new { message, pracSeqNum });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
