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
    using NUnit.Framework.Internal;

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
            int studentDataNom = testCases/100; 
            int enrollmentDataNom = 10 ;
            var getCourses = await _courseService.GetAllAsync();
            List<Courses> courses = getCourses.ToList();
            TestResultData testResult = new TestResultData();
            var getLecturers = await _lecturerService.GetAllAsync();
            List<Lecturer> lecturers = getLecturers.ToList();

            var getUniv = await _universityService.GetAllAsync();
            List<UniversityData> universities = getUniv.ToList();
            //for (int x=0; x<10; x++)
            //{
                //Stopwatch stopwatch = new Stopwatch();
                TestResultData result = new TestResultData();
                //stopwatch.Start();
                //var students = await _studentService.TestStudentInsert(studentDataNom, universities);
                var enrollmentStopwatch = await _enrollmentService.TestEnrollmentInsert(enrollmentDataNom, universities, lecturers, courses, studentDataNom); 
                //stopwatch.Stop();

                result = getTestResult(enrollmentStopwatch, testCases);
            
                testResult = await _performanceTestInsertService.SavePerformanceTestData(result);
               //await TestDelete(testCases);
            //}
            return Ok(testResult);
        }

        [HttpPut("testUpdate/{testCases}")]
        public async Task<ActionResult<TestResultData>> TestUpdate(int testCases)
        {
            int enrollmentDataNom = 10;
            int studentDataNom = testCases / 100;
            TestResultData testResult = new TestResultData();
            var getCourses = await _courseService.GetAllAsync();
            List<Courses> courses = getCourses.ToList();
            var getLecturers = await _lecturerService.GetAllAsync();
            List<Lecturer> lecturers = getLecturers.ToList();
            var getUniv = await _universityService.GetAllAsync();
            List<UniversityData> universities = getUniv.ToList();
            List<Student> oldStudentDatas = new List<Student>();
            try
            {
                oldStudentDatas = await _studentService.TestStudentGet(studentDataNom);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

                TestResultData result = new TestResultData();
                TestResultData result2 = new TestResultData();
                TestResultData total = new TestResultData();
                Stopwatch stopwatch = new Stopwatch();
                Stopwatch stopwatch2 = new Stopwatch();
                double count = 0;
                double roundDown =0;

                stopwatch.Start();
                var studentStopwatch = await _studentService.TestStudentUpdate(studentDataNom,oldStudentDatas);
                stopwatch.Stop();
                result = getTestResult(studentStopwatch, testCases);
            
                stopwatch2.Start();
                var enrollStopwatch = await _enrollmentService.TestEnrollmentUpdate(enrollmentDataNom, universities, lecturers, courses, oldStudentDatas, testCases);
                result2 = getTestResult(enrollStopwatch, testCases);
                stopwatch2.Stop();
                //milis
                
            
                total.MiliSeconds = result.MiliSeconds +result2.MiliSeconds;
                if (total.MiliSeconds > 1000)
                {
                    count = total.MiliSeconds / 1000;
                    roundDown = Math.Floor(count * 100) / 100;
                    int storeSeconds = (int)roundDown;
                    total.MiliSeconds = total.MiliSeconds - 1000;
                    total.Seconds = total.Seconds + storeSeconds;
                }

                //secs
                total.Seconds = total.Seconds + result.Seconds +result2.Seconds;
                if (total.Seconds > 60)
                {
                    count = total.Seconds / 60;
                    roundDown = Math.Floor(count * 100) / 100;
                    int storeMinutes = (int)roundDown;
                    total.Seconds = total.Seconds - 60;
                    total.Minutes = total.Minutes + storeMinutes;
                }
                total.DataProcessed = testCases;
                total.Minutes = total.Minutes+ result.Minutes + result2.Minutes;
                total.Hours = result.Hours + result2.Hours;
                long totalMilis = stopwatch.ElapsedMilliseconds + enrollStopwatch.ElapsedMilliseconds;
                double miliseconds = totalMilis * 1.0;
                total.AverageTime = miliseconds / total.DataProcessed;
                testResult = await _performanceTestUpdateService.SavePerformanceTestData(total);

            return Ok(testResult);
        }

        [HttpGet("testGet/{testCases}")]
        public async Task<ActionResult<TestResultData>> TestGet(int testCases)
        {
            int studentDataNom = testCases / 100;
            int enrollmentDataNom = 10;
            TestResultData testResult = new TestResultData();
            //for (int x=0; x<10; x++)
            //{
                TestResultData result = new TestResultData();
                Stopwatch stopwatch = new Stopwatch();
                try
                {
                    stopwatch.Start();
                    var students = await _studentService.TestStudentGet(studentDataNom);
                    var enrollments = await _enrollmentService.TestEnrollmentGet(enrollmentDataNom,students);
                    stopwatch.Stop();

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message.ToString());
                }

                result = getTestResult(stopwatch, testCases);
                testResult = await _performanceTestGetService.SavePerformanceTestData(result);
            //}

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

            List<Student> students = new List<Student>();
            try
            {
                students = await _studentService.TestStudentGet(studentDataNom);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }


            stopwatch.Start();
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

        [HttpGet("GetTopInsertDataGraph/")]

        public async Task<ActionResult<IEnumerable<GraphData>>> GetTopInsertDataGraph()
        {
            var result = await _performanceTestInsertService.GetTopPerformanceGraphData();
            return Ok(result);

        }

        [HttpGet("GetTopUpdateDataGraph/")]

        public async Task<ActionResult<IEnumerable<GraphData>>> GetTopUpdateDataGraph()
        {
            var result = await _performanceTestUpdateService.GetTopPerformanceGraphData();
            return Ok(result);

        }

        [HttpGet("GetTopGetDataGraph/")]

        public async Task<ActionResult<IEnumerable<GraphData>>> GetTopGetDataGraph()
        {
            var result = await _performanceTestGetService.GetTopPerformanceGraphData();
            return Ok(result);

        }

        [HttpGet("GetTopDeleteDataGraph/")]

        public async Task<ActionResult<IEnumerable<GraphData>>> GetTopDeleteDataGraph()
        {
            var result = await _performanceTestDeleteService.GetTopPerformanceGraphData();
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
