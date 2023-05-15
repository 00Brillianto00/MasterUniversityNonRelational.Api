namespace MasterUniversityNonRelational.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MasterUniversityNonRelational.Api.Models;
    using MasterUniversityNonRelational.Api.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            this._enrollmentService = enrollmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> Get()
        {
            var result = await _enrollmentService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> GetByID(Guid id)
        {
            var result = await _enrollmentService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Enrollment>> Save([FromBody] Enrollment universityData)
        {
            var result = await _enrollmentService.Save(universityData);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Enrollment>> Update(Guid id, [FromBody] Enrollment universityData)
        {
            var checkData = await _enrollmentService.GetByIdAsync(id);

            if (id.ToString() != checkData.Id)
            {
                return BadRequest();
            }
            else
            {
                await _enrollmentService.Update(id.ToString(), universityData);
            }
            return Ok(universityData);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var data = await _enrollmentService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                await _enrollmentService.Delete(id);
            }
            return NoContent();
        }

    }
}
