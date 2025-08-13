using casman.Models;
using casman.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace casman.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateCaseController : ControllerBase
    {
        private readonly ICaseRepository _caseRepository;

        public CreateCaseController(ICaseRepository caseRepository)
        {
            _caseRepository = caseRepository;
        }

        // GET: api/CreateCase/specialities
        [HttpGet("specialities")]
        public ActionResult<IEnumerable<SpecialityDto>> GetSpecialities()
        {
            var results = _caseRepository.GetSpecialities();
            return Ok(results);
        }

        // GET: api/CreateCase/indemnifiers
        [HttpGet("indemnifiers")]
        public async Task<ActionResult<List<IndemnifierDto>>> GetIndemnifiers()
        {
            var indemnifiers = await _caseRepository.GetIndemnifiersAsync();
            return Ok(indemnifiers);
        }

        // POST: api/CreateCase/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateNewCase([FromBody] CreateCaseDto dto)
        {
            try
            {
                await _caseRepository.CreateNewCaseAsync(dto);
                return Ok(new { message = "Case created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
