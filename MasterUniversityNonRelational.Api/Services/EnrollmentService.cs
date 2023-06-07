using MasterUniversityNonRelational.Api.Interfaces;
using MasterUniversityNonRelational.Api.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using NUnit.Framework.Internal;
using System.Data;
using System.Diagnostics;

namespace MasterUniversityNonRelational.Api.Services
{
    public partial class EnrollmentService :IEnrollmentService
    {
        private readonly IMongoCollection<Enrollment> _enrollment;
        private readonly IMongoCollection<Courses> _courses;
        private Random rng = new Random();
        private readonly ICourseService _courseService;

        public EnrollmentService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings, ICourseService courseService)
        {

            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
            databaseSettings.CollectionName = "Enrollment";
            _enrollment = database.GetCollection<Enrollment>(databaseSettings.CollectionName);
            databaseSettings.CollectionName = "Courses";
            _courses = database.GetCollection<Courses>(databaseSettings.CollectionName);
            this._courseService= courseService; 
            
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            try
            {
                var data = await _enrollment.Find(Enrollment => Enrollment.IsDeleted == false).ToListAsync();
                //var data = await _enrollment.Find(Enrollment => true).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<Enrollment> GetByIdAsync(Guid id)
        {
            var ID = id.ToString();
            try
            {
                var data = await _enrollment.Find(Enrollment => Enrollment.Id.Equals(ID) && Enrollment.IsDeleted == false).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<Enrollment> Save(Enrollment enrollmentData)
        {
            enrollmentData.Id = Guid.NewGuid().ToString();
            enrollmentData.IsDeleted = false;
            Random rng = new Random();
            int month = rng.Next(1, 13);
            int countCourses = 0;
            int countCredit = 0;
            int countCost = 0;
            double countGPA = 0;
            foreach (var data in enrollmentData.enrollmentDetail)
            {

                countCourses++;
                data.AssignmentScore = rng.Next(1, 100);
                data.MidExamScore = rng.Next(1, 100);
                data.FinalExamScore = rng.Next(1, 100);
                data.CourseAverageScore = (data.FinalExamScore + data.MidExamScore + data.AssignmentScore) / 3;
                countGPA = countGPA + data.CourseAverageScore;
            }
            enrollmentData.AverageScorePerSemester = countGPA / countCourses;
            enrollmentData.TotalCoursePerSemester= countCourses;
            countGPA = 0;
            countCourses = 0;

            try
            {
                await _enrollment.InsertOneAsync(enrollmentData);
                return enrollmentData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Saving Data");
            }
        }

        public async Task<Enrollment> Update(string id, Enrollment enrollmentData)
        {
            enrollmentData.Id = id;
            enrollmentData.IsDeleted = false;
            try
            {
                var data = await _enrollment.ReplaceOneAsync(enrollmentData => enrollmentData.Id.Equals(id), enrollmentData);
                return enrollmentData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Updating Data");
            }
        }
        public async Task<bool> Delete(Guid id)
        {
            var ID = id.ToString();
            try
            {
                await _enrollment.DeleteOneAsync(enrollmentData => enrollmentData.Id.Equals(ID));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("error when deleting data");
            }
        }

        public async Task<List<Enrollment>> TestEnrollmentInsert(int testCases, List<UniversityData> universities, List<Lecturer> lecturers, List<Courses> courses, List<Student>students)
        {
            List<Enrollment> enrollments = new List<Enrollment>();
            try
            {
                for (int x = 0; x < students.Count; x++)
                {
                    //50 student
                    for (int y = 0; y < testCases; y++)
                    {
                        //each student should have 10
                        Enrollment enrollmentHeader = new Enrollment();
                        enrollmentHeader.enrollmentDetail = new List<EnrollmentDetail>();
                        Guid id = Guid.NewGuid();
                        enrollmentHeader.Id = id.ToString();
                        enrollmentHeader.studentID = students[x].Id;
                        enrollmentHeader.IsDeleted = false;
                        //enrollmentHeader.GPAPerSemester = 0;
                        enrollmentHeader.TotalCoursePerSemester = 10;
                        enrollmentHeader.TotalCostPerSemester = rng.Next(10000000, 99999999);
                        enrollmentHeader.TotalCreditsPerSemester = rng.Next(10, 20);
                        enrollmentHeader.Year = rng.Next(2000, 2023).ToString();
                        int STYPE = rng.Next(1, 2);
                        if (STYPE == 1)
                        {
                            enrollmentHeader.SemesterType = "ODD";
                        }
                        else
                        {
                            enrollmentHeader.SemesterType = "EVEN";
                        }
                        //List<EnrollmentDetail> details = new List <EnrollmentDetail>();
                        for (int z = 0; z < 10; z++)
                        {
                            EnrollmentDetail enrollmentDetail = new EnrollmentDetail();
                            enrollmentDetail.CourseID = courses[z].Id;
                            enrollmentDetail.LecturerID = lecturers[rng.Next(0, lecturers.Count)].Id;
                            enrollmentDetail.AssignmentScore = rng.Next(1, 100);
                            enrollmentDetail.MidExamScore = rng.Next(1, 100);
                            enrollmentDetail.FinalExamScore = rng.Next(1, 100);
                            enrollmentDetail.CourseAverageScore = (enrollmentDetail.AssignmentScore + enrollmentDetail.MidExamScore + enrollmentDetail.FinalExamScore) / 3.0;
                            enrollmentHeader.enrollmentDetail.Add(enrollmentDetail);
                        }
                        //enrollmentHeader.enrollmentDetail.Add(details);
                        await _enrollment.InsertOneAsync(enrollmentHeader);
                        enrollments.Add(enrollmentHeader);
                    }
                }
                return enrollments;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Running Test Cases");
            }
        }

        public async Task<List<Enrollment>> TestEnrollmentUpdate(int testCases, List<UniversityData> universities, List<Lecturer> lecturers, List<Courses> courses, List<Student> students)
        {
            try
            {
                List<Enrollment> newEnrollments = new List<Enrollment>();
                for (int x = 0; x < students.Count; x++)
                {
                    Enrollment enrollmentHeader = await _enrollment.Find(Enrollment => Enrollment.studentID.Equals(students[x].Id) && Enrollment.IsDeleted == false).FirstOrDefaultAsync();
                    enrollmentHeader.IsDeleted = false;
                    string enrollID = enrollmentHeader.Id;
                    //enrollmentHeader.GPAPerSemester = 0;
                    string type = enrollmentHeader.SemesterType;
                    enrollmentHeader.SemesterType = "UPDATED_" + type;

                    int countCourse = 0;
                    double countAverages = 0.0;
                    int countCost = 0;
                    int countCredit = 0;
                    

                    for (int z = 0; z<enrollmentHeader.enrollmentDetail.Count(); z++)
                    {
                        string ID = courses[rng.Next(0, courses.Count)].Id;
                        enrollmentHeader.enrollmentDetail[z].CourseID = ID;
                        enrollmentHeader.enrollmentDetail[z].LecturerID = lecturers[rng.Next(0, lecturers.Count)].Id;
                        enrollmentHeader.enrollmentDetail[z].AssignmentScore = rng.Next(1, 100);
                        enrollmentHeader.enrollmentDetail[z].MidExamScore = rng.Next(1, 100);
                        enrollmentHeader.enrollmentDetail[z].FinalExamScore = rng.Next(1, 100);
                        enrollmentHeader.enrollmentDetail[z].CourseAverageScore = (enrollmentHeader.enrollmentDetail[z].AssignmentScore + 
                            enrollmentHeader.enrollmentDetail[z].MidExamScore + enrollmentHeader.enrollmentDetail[z].FinalExamScore) / 3.0;
                        Courses data = await _courseService.GetByIdStringAsync(ID);
                        countCourse++;
                        countAverages = countAverages + enrollmentHeader.enrollmentDetail[z].CourseAverageScore;
                        countCost = countCost + data.Cost;
                        countCredit= countCredit + data.Credit;
                    }

                    enrollmentHeader.TotalCoursePerSemester = countCourse;
                    enrollmentHeader.TotalCostPerSemester = countCost;
                    enrollmentHeader.TotalCreditsPerSemester = countCredit;
                    enrollmentHeader.AverageScorePerSemester= (countAverages/countCourse)/1.0;

                   await _enrollment.ReplaceOneAsync(enrollmentData => enrollmentData.Id.Equals(enrollID), enrollmentHeader);
                   newEnrollments.Add(enrollmentHeader);
                }
                return newEnrollments;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Running Test Cases");
            }
        }

        public async Task<List<StudentEnrollmentDataModel>> TestEnrollmentGet(int testCases, List<Student> students)
        {
            List<Enrollment> enrollments = new List<Enrollment>();
            List <StudentEnrollmentDataModel>studentEnrollments = new List<StudentEnrollmentDataModel>();
            Enrollment enrollment = new Enrollment();
            try
            {
                for (int x=0; x<students.Count(); x++)
                {
                    StudentEnrollmentDataModel studentEnrollment = new StudentEnrollmentDataModel();
                    studentEnrollment.enrollment = new List<Enrollment>();
                    string ID = students[x].Id;
                    studentEnrollment.Id = ID;
                    studentEnrollment.UniversityID = students[x].UniversityID;
                    studentEnrollment.StudentNumber = students[x].StudentNumber;
                    studentEnrollment.StudentEmail = students[x].StudentEmail;
                    studentEnrollment.EnrolledYear = students[x].EnrolledYear;
                    studentEnrollment.TotalCreditsEarned = students[x].TotalCreditsEarned;
                    studentEnrollment.StudentGPA = students[x].StudentGPA;
                    studentEnrollment.StudentName = students[x].StudentName;
                    studentEnrollment.StudentDateOfBirth = students[x].StudentDateOfBirth;
                    studentEnrollment.StudentPhoneNumber = students[x].StudentPhoneNumber;
                    studentEnrollment.StudentAddress = students[x].StudentAddress;
                    studentEnrollment.StudentPostalCode = students[x].StudentPostalCode;
                    studentEnrollment.IsDeleted = students[x].IsDeleted;
                    List<Enrollment> listEnrollment= await _enrollment.Find(Enrollment => Enrollment.IsDeleted == false && Enrollment.studentID.Equals(ID)).ToListAsync();

                    foreach (var data in listEnrollment)
                    {
                        studentEnrollment.enrollment.Add(data);
                    }
                    studentEnrollments.Add(studentEnrollment);
                }
                return studentEnrollments;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Running Test Cases");
            }
        }

        public async Task <bool> TestEnrollmentDelete(List<Student> students)
        {
            try
            {
                int count = 0;
                foreach (var data in students)
                {
                    var deleteFilter = Builders<Enrollment>.Filter.Eq(x => x.studentID, data.Id);
                    await _enrollment.DeleteManyAsync(deleteFilter);
                    count++;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Running Test Cases");
            }

        }
    }
}