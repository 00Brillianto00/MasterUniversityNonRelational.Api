namespace MasterUniversityNonRelational.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MasterUniversityNonRelational.Api.Models;
    using MasterUniversityNonRelational.Api.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using MasterUniversityNonRelational.Api.Services;
    using System.Diagnostics;

    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class PerformanceComparisonController : ControllerBase
    {
        //private readonly IPerformanceComparisonService _performanceComparisonService;
        private readonly IStudentService _studentService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly ICourseService _courseService;
        private readonly ILecturerService _lecturerService;
        private readonly IUniversityService _universityService;
        private readonly IPerformanceTestInsertService _performanceTestInsertService;

        public PerformanceComparisonController(IStudentService studentService, 
            IEnrollmentService enrollmentService, ICourseService courseService, ILecturerService lecturerService, IUniversityService universityService , IPerformanceTestInsertService performanceTestInsertService)
        {
            //this._performanceComparisonService = performanceComparisonService;
            this._studentService = studentService;
            this._enrollmentService = enrollmentService;
            this._courseService = courseService;
            this._lecturerService = lecturerService;
            this._universityService = universityService;
            this._performanceTestInsertService = performanceTestInsertService;
        }

        [HttpPost("testInsert/{testCases}")]
        public async Task<ActionResult<TestResultData>> TestSave(int testCases)
        {
            Stopwatch stopwatch = new Stopwatch();
            TestResultData result = new TestResultData();
            int studentDataNom = testCases/100; 
            int enrollmentDataNom = 10 ;


            var getCourses = await _courseService.GetAllAsync();
            List<Courses> courses = getCourses.ToList();

            var getLecturers = await _lecturerService.GetAllAsync();
            List<Lecturer> lecturers = getLecturers.ToList();

            var getUniv = await _universityService.GetAllAsync();
            List<UniversityData> universities = getUniv.ToList();

            stopwatch.Start();
            var students = await _studentService.TestStudentInsert(studentDataNom, universities);

            var enrollments = await _enrollmentService.TestEnrollmentInsert(enrollmentDataNom, universities, lecturers, courses, students); 
            stopwatch.Stop();

            result = getTestResult(stopwatch, testCases);
            
            var testResult = await _performanceTestInsertService.SavePerformanceTestData(result);
           
            return Ok(testResult);
        }

        [HttpPut("testUpdate/{testCases}")]
        public async Task<ActionResult<TestResultData>> TestUpdate(int testCases)
        {
            Stopwatch stopwatch = new Stopwatch();
            TestResultData result = new TestResultData();
            int studentDataNom = testCases / 100;
            int enrollmentDataNom = 10;


            var getCourses = await _courseService.GetAllAsync();
            List<Courses> courses = getCourses.ToList();

            var getLecturers = await _lecturerService.GetAllAsync();
            List<Lecturer> lecturers = getLecturers.ToList();

            var getUniv = await _universityService.GetAllAsync();
            List<UniversityData> universities = getUniv.ToList();

            stopwatch.Start();

            var updatedStudents = await _studentService.TestStudentUpdate(studentDataNom);

            var enrollments = await _enrollmentService.TestEnrollmentUpdate(enrollmentDataNom, universities, lecturers, courses, updatedStudents);

            stopwatch.Stop();

            result = getTestResult(stopwatch, testCases);

            return Ok(result);
        }

        [HttpGet("testGet/{testCases}")]
        public async Task<ActionResult<TestResultData>> TestGet(int testCases)
        {
            Stopwatch stopwatch = new Stopwatch();
            TestResultData result = new TestResultData();
            int studentDataNom = testCases / 100;
            int enrollmentDataNom = 10;

            stopwatch.Start();

            var students = await _studentService.TestStudentGet(studentDataNom);

            var enrollments = await _enrollmentService.TestEnrollmentGet(enrollmentDataNom,students);

            stopwatch.Stop();

            result = getTestResult(stopwatch, testCases);

            return Ok(result);
        }

        //[HttpGet("testGetReturnObject/{testCases}")]
        //public async Task<ActionResult<List<StudentEnrollmentDataModel>>> TestGetObject(int testCases)
        //{
        //    int studentDataNom = testCases / 100;
        //    int enrollmentDataNom = 10;

        //    var students = await _studentService.TestStudentGet(studentDataNom);

        //    var result = await _enrollmentService.TestEnrollmentGet(enrollmentDataNom, students);

            
        //    return Ok(result);
        //}

        [HttpDelete("testDelete/{testCases}")]
        public async Task<ActionResult<TestResultData>> TestDelete(int testCases)
        {
            Stopwatch stopwatch = new Stopwatch();
            TestResultData result = new TestResultData();
            int studentDataNom = testCases / 100;
            int enrollmentDataNom = 10;

            stopwatch.Start();

            var students = await _studentService.TestStudentGet(studentDataNom);
            var tes = await _studentService.TestStudentDelete(studentDataNom, students);
            var tes2 = await _enrollmentService.TestEnrollmentDelete(students);

            if(tes == true && tes2 == true)
            {
                stopwatch.Stop();
            }

            result = getTestResult(stopwatch, testCases);

            return Ok(result);
        }

        [HttpGet("GetTopInsertData/{topData}")]

        public async Task<ActionResult<IEnumerable<TestResultData>>> GetTopInsertData(int topData)
        {
            var result = await _performanceTestInsertService.GetTopPerformanceTestData(topData);
            return Ok(result);
        }

        private TestResultData getTestResult(Stopwatch stopWatch, int testCases)
        {
            TestResultData result = new TestResultData();
            result.DataProcessed = testCases;
            result.Hours = stopWatch.Elapsed.Hours;
            result.Minutes = stopWatch.Elapsed.Minutes;
            result.Seconds = stopWatch.Elapsed.Seconds;
            result.MiliSeconds = stopWatch.Elapsed.Milliseconds;
            double miliseconds = stopWatch.Elapsed.Milliseconds * 1.0;
            result.AverageTime= miliseconds/result.DataProcessed;
            return result;
        }
    }
}
