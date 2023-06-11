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
    using NUnit.Framework;

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
        private readonly IPerformanceTestGetService _performanceTestGetService;
        private readonly IPerformanceTestUpdateService _performanceTestUpdateService;
        private readonly IPerformanceTestDeleteService _performanceTestDeleteService;

        public PerformanceComparisonController(IStudentService studentService, 
            IEnrollmentService enrollmentService, ICourseService courseService, ILecturerService lecturerService, 
            IUniversityService universityService , 
            IPerformanceTestInsertService performanceTestInsertService,
            IPerformanceTestGetService performanceTestGetService,
            IPerformanceTestUpdateService performanceTestUpdateService,
            IPerformanceTestDeleteService performanceTestDeleteService)
        {
            //this._performanceComparisonService = performanceComparisonService;
            this._studentService = studentService;
            this._enrollmentService = enrollmentService;
            this._courseService = courseService;
            this._lecturerService = lecturerService;
            this._universityService = universityService;
            this._performanceTestInsertService = performanceTestInsertService;
            this._performanceTestGetService = performanceTestGetService;
            this._performanceTestUpdateService = performanceTestUpdateService;
            this._performanceTestDeleteService = performanceTestDeleteService;
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

            var enrollments = await _enrollmentService.TestEnrollmentUpdate(enrollmentDataNom, universities, lecturers, courses, updatedStudents, testCases);

            stopwatch.Stop();

            result = getTestResult(stopwatch, testCases);
            var testResult = await _performanceTestUpdateService.SavePerformanceTestData(result);

            return Ok(testResult);
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
            var testResult = await _performanceTestGetService.SavePerformanceTestData(result);

            return Ok(testResult);
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
            int enrollmentDataNom = 10;;
            stopwatch.Start();

            var students = await _studentService.TestStudentGet(studentDataNom);
            if(students.Count < studentDataNom )
            {
                throw new Exception("Not Enough Datas in Database, Please Repopulate Datas.");
            }
            var tes = await _studentService.TestStudentDelete(studentDataNom, students);
            var tes2 = await _enrollmentService.TestEnrollmentDelete(students);

            if(tes == true && tes2 == true)
            {
                stopwatch.Stop();
            }

            result = getTestResult(stopwatch, testCases);
            var testResult = await _performanceTestDeleteService.SavePerformanceTestData(result);

            return Ok(testResult);
        }

        [HttpGet("GetTopInsertData/{topData}")]

        public async Task<ActionResult<IEnumerable<TestResultData>>> GetTopInsertData(int topData)
        {
            var result = await _performanceTestInsertService.GetTopPerformanceTestData(topData);
            return Ok(result);
        }

        [HttpGet("GetTopUpdateData/{topData}")]

        public async Task<ActionResult<IEnumerable<TestResultData>>> GetTopUpdateData(int topData)
        {
            var result = await _performanceTestUpdateService.GetTopPerformanceTestData(topData);
            return Ok(result);
        }
        [HttpGet("GetTopGetData/{topData}")]

        public async Task<ActionResult<IEnumerable<TestResultData>>> GetTopGetData(int topData)
        {
            var result = await _performanceTestGetService.GetTopPerformanceTestData(topData);
            return Ok(result);
        }
        [HttpGet("GetTopDeleteData/{topData}")]

        public async Task<ActionResult<IEnumerable<TestResultData>>> GetTopDeleteData(int topData)
        {
            var result = await _performanceTestDeleteService.GetTopPerformanceTestData(topData);
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
            double miliseconds = stopWatch.ElapsedMilliseconds* 1.0;
            result.AverageTime= miliseconds/result.DataProcessed;
            return result;
        }
    }
}
