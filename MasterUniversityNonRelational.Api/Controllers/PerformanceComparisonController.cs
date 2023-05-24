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

        public PerformanceComparisonController(IStudentService studentService, 
            IEnrollmentService enrollmentService, ICourseService courseService, ILecturerService lecturerService, IUniversityService universityService)
        {
            //this._performanceComparisonService = performanceComparisonService;
            this._studentService = studentService;
            this._enrollmentService = enrollmentService;
            this._courseService = courseService;
            this._lecturerService = lecturerService;
            this._universityService = universityService;
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
            double seconds = (stopWatch.ElapsedMilliseconds / 1000.00);
            double averages;
            string averageDesc;
            if (result.Seconds == 0)
            {
                averages = result.DataProcessed / (seconds * 1000.00);
                averageDesc = " Datas per Milisecond";
            }
            else
            {
                averages = result.DataProcessed / seconds;
                averageDesc = " Datas per Second";
            }
            result.AverageTime = "Averaging about " + averages.ToString("0.##") + averageDesc;
            return result;
        }
    }
}
