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
    public class LecturerController : ControllerBase
    {
        private readonly IUniversityService _universityService;
        public LecturerController(IUniversityService universityService)
        {
            this._universityService = universityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UniversityData>>> Get()
        {
            var result = await _universityService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UniversityData>> GetByID(Guid id)
        {
            var result = await _universityService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<UniversityData>> Save([FromBody] UniversityData universityData)
        {
            var result = await _universityService.Save(universityData);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UniversityData>> Update(Guid id, [FromBody] UniversityData universityData)
        {
            var checkData = await _universityService.GetByIdAsync(id);

            if (id.ToString() != checkData.Id)
            {
                return BadRequest();
            }
            else
            {
                await _universityService.Update(id.ToString(), universityData);
            }
            return Ok(universityData);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var data = await _universityService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                await _universityService.Delete(id);
            }
            return NoContent();
        }
    }
}
