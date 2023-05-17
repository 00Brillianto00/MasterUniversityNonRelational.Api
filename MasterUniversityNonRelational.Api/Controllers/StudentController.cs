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
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            this._studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            var result = await _studentService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetByID(Guid id)
        {
            var result = await _studentService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> Save([FromBody] Student studentData)
        {
            var result = await _studentService.Save(studentData);
            return Ok(result);
        }

        [HttpPost("testInsert/{testCases}")]
        public async Task<ActionResult<string>> TestSave([FromBody] Student studentData, int testCases)
        {
            var result = await _studentService.TestCase(studentData, testCases);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> Update(Guid id, [FromBody] Student studentData)
        {
            var checkData = await _studentService.GetByIdAsync(id);

            if (id.ToString() != checkData.Id)
            {
                return BadRequest();
            }
            else
            {
                await _studentService.Update(id.ToString(), studentData);
            }
            return Ok(studentData);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var data = await _studentService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                await _studentService.Delete(id);
            }
            return NoContent();
        }
    }
}
