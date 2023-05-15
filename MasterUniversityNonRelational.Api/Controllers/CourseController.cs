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
    public class CourseController:ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            this._courseService = courseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Courses>>> Get()
        {
            var result = await _courseService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Courses>> GetByID(Guid id)
        {
            var result = await _courseService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Courses>> Save([FromBody] Courses courseData)
        {
            var result = await _courseService.Save(courseData);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Courses>> Update(Guid id, [FromBody] Courses courseData)
        {
            var checkData = await _courseService.GetByIdAsync(id);

            if (id.ToString() != checkData.Id)
            {
                return BadRequest();
            }
            else
            {
                await _courseService.Update(id.ToString(), courseData);
            }
            return Ok(courseData);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var data = await _courseService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                await _courseService.Delete(id);
            }
            return NoContent();
        }
    }
}
