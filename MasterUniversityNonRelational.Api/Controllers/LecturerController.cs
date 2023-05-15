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
        private readonly ILecturerService _lecturerService;
        public LecturerController(ILecturerService lecturerService)
        {
            this._lecturerService = lecturerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lecturer>>> Get()
        {
            var result = await _lecturerService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lecturer>> GetByID(Guid id)
        {
            var result = await _lecturerService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Lecturer>> Save([FromBody] Lecturer lecturerData)
        {
            var result = await _lecturerService.Save(lecturerData);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Lecturer>> Update(Guid id, [FromBody] Lecturer lecturerData)
        {
            var checkData = await _lecturerService.GetByIdAsync(id);

            if (id.ToString() != checkData.Id)
            {
                return BadRequest();
            }
            else
            {
                await _lecturerService.Update(id.ToString(), lecturerData);
            }
            return Ok(lecturerData);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var data = await _lecturerService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                await _lecturerService.Delete(id);
            }
            return NoContent();
        }
    }
}
