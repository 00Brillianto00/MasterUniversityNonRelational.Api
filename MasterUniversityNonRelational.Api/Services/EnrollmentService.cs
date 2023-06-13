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
        private readonly IMongoCollection<Student> _students;
        private Random rng = new Random();
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;

        public EnrollmentService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings, ICourseService courseService, IStudentService studentService)
        {

            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
            databaseSettings.CollectionName = "Enrollment";
            _enrollment = database.GetCollection<Enrollment>(databaseSettings.CollectionName);
            
            databaseSettings.CollectionName = "Courses";
            _courses = database.GetCollection<Courses>(databaseSettings.CollectionName);
            this._courseService = courseService;
            
            databaseSettings.CollectionName = "Student";
            _students = database.GetCollection<Student>(databaseSettings.CollectionName);
            this._studentService = studentService;
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

        public async Task<List<Enrollment>> TestEnrollmentInsert(int testCases, List<UniversityData> universities, List<Lecturer> lecturers, List<Courses> courses,int studentNom)
        {
            List<Enrollment> totalEnrollments = new List<Enrollment>();
            //List<Student> studentDatas = new List<Student>();
            long StudentNumber = rng.NextInt64(1, 9999999999);
            string firstName = "StudentFirstName";
            string middleName = "StudentMiddleName";
            string lastName = "StudentLastName_";
            string address = "JL Kemanggisan Raya";
            string country = "Indonesia";
            string province = "DKI Jakarta";
            string city = "Jakarta Barat";
            try
            {
                for (int x = 0; x < studentNom; x++)
                {
                    List<Student> studentDatas = new List<Student>();
                    Student studentData = new Student();
                    studentData.Id = Guid.NewGuid().ToString();
                    studentData.UniversityID = universities[rng.Next(0, universities.Count())].Id;
                    studentData.StudentNumber = StudentNumber++;
                    string getModifier = studentData.StudentNumber.ToString().Substring(StudentNumber.ToString().Length - 4, 4);
                    studentData.StudentEmail = firstName + "." + lastName + getModifier + "@Univ.ac.id";
                    studentData.StudentName = firstName + " " + middleName + " " + lastName + getModifier;
                    studentData.StudentGPA = rng.NextDouble() * (4.0 - 1.0) + 1.0;
                    studentData.TotalCreditsEarned = rng.Next(0, 100);
                    studentData.EnrolledYear = rng.Next(2000, 2023).ToString();
                    studentData.StudentDateOfBirth = generateDoB();
                    studentData.StudentPhoneNumber = generatePhoneNum();
                    studentData.StudentAddress = address + " No." + rng.Next(0, 50) + "," + city + "," + province + "," + country;
                    studentData.StudentPostalCode = rng.Next(1000, 9999);
                    studentData.IsDeleted = false;
                    //await _students.InsertOneAsync(studentData);
                    studentDatas.Add(studentData);

                    List<Enrollment> enrollments = new List<Enrollment>();
                    for (int y = 0; y < testCases; y++)
                    {
                        //each student should have 10
                        Enrollment enrollmentHeader = new Enrollment();
                        enrollmentHeader.enrollmentDetail = new List<EnrollmentDetail>();
                        Guid id = Guid.NewGuid();
                        enrollmentHeader.Id = id.ToString();
                        enrollmentHeader.studentID = studentData.Id;
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
                        //await _enrollment.InsertOneAsync(enrollmentHeader);
                        enrollments.Add(enrollmentHeader);
                        totalEnrollments.Add(enrollmentHeader);
                        //totalEnrollments.Add(enrollmentHeader);
                    }
                    await _students.InsertManyAsync(studentDatas);
                    await _enrollment.InsertManyAsync(enrollments);
                }
                return totalEnrollments;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Running Test Cases");
            }
        }

        public async Task<Stopwatch> TestEnrollmentUpdate(int testCases, List<UniversityData> universities, List<Lecturer> lecturers, List<Courses> courses, List<Student> students, int totalNeedDoc)
        {
            Stopwatch stopwatch = new Stopwatch();
            Stopwatch stopwatch3 = new Stopwatch();
            Stopwatch stopwatch2 = new Stopwatch();
            try
            {
                List<Enrollment> newEnrollments = new List<Enrollment>();
                for (int x = 0; x < students.Count; x++)
                {
                    stopwatch3.Start();  
                    List<Enrollment> enrollmentHeader = await _enrollment.Find(Enrollment => Enrollment.studentID.Equals(students[x].Id) && Enrollment.IsDeleted == false).ToListAsync();
                    stopwatch3.Stop();
                    stopwatch2.Start();
                    try
                    {
                        for(int y=0; y<enrollmentHeader.Count(); y++)
                        {
                            stopwatch.Start();
                            enrollmentHeader[y].IsDeleted = false;
                            string enrollID = enrollmentHeader[y].Id;
                            //enrollmentHeader.GPAPerSemester = 0;
                            string type = enrollmentHeader[y].SemesterType;
                            enrollmentHeader[y].SemesterType = "UPDATED_" + type;

                            int countCourse = 0;
                            double countAverages = 0.0;
                            int countCost = 0;
                            int countCredit = 0;

                            for (int z = 0; z < enrollmentHeader[y].enrollmentDetail.Count(); z++)
                            {
                                string ID = courses[rng.Next(0, courses.Count)].Id;
                                enrollmentHeader[y].enrollmentDetail[z].CourseID = ID;
                                enrollmentHeader[y].enrollmentDetail[z].LecturerID = lecturers[rng.Next(0, lecturers.Count)].Id;
                                enrollmentHeader[y].enrollmentDetail[z].AssignmentScore = rng.Next(1, 100);
                                enrollmentHeader[y].enrollmentDetail[z].MidExamScore = rng.Next(1, 100);
                                enrollmentHeader[y].enrollmentDetail[z].FinalExamScore = rng.Next(1, 100);
                                enrollmentHeader[y].enrollmentDetail[z].CourseAverageScore = (enrollmentHeader[y].enrollmentDetail[z].AssignmentScore + 
                                    enrollmentHeader[y].enrollmentDetail[z].MidExamScore + enrollmentHeader[y].enrollmentDetail[z].FinalExamScore) / 3.0;
                                //Courses data = await _courseService.GetByIdStringAsync(ID);
                                int cost = rng.Next(50000, 200000);
                                int credit = rng.Next(5, 20);
                                countCourse++;
                                countAverages = countAverages + enrollmentHeader[y].enrollmentDetail[z].CourseAverageScore;
                                countCost = countCost + cost;
                                countCredit= countCredit + credit;
                            }

                            enrollmentHeader[y].TotalCoursePerSemester = countCourse;
                            enrollmentHeader[y].TotalCostPerSemester = countCost;
                            enrollmentHeader[y].TotalCreditsPerSemester = countCredit;
                            enrollmentHeader[y].AverageScorePerSemester= (countAverages/countCourse)/1.0;

                            await _enrollment.ReplaceOneAsync(enrollmentData => enrollmentData.Id.Equals(enrollID), enrollmentHeader[y]);
                            newEnrollments.Add(enrollmentHeader[y]);
                        }
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("Not Enough Datas in Database, Please Repopulate Datas.");
                    }
                    stopwatch.Stop();
                    stopwatch2.Stop();
                }
                return stopwatch;
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
                    //Optimized
                    var deleteFilter = Builders<Enrollment>.Filter.Eq(x => x.studentID, data.Id);
                    await _enrollment.DeleteManyAsync(deleteFilter);
                    count++;

                    //for (int x = 0; x < 10; x++)
                    ////each students has 10 enrollments
                    //{
                    //    await _enrollment.DeleteOneAsync(enrollData => enrollData.studentID.Equals(data.Id));
                    //    count++;
                    //}
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Running Test Cases");
            }

        }

        private string generatePhoneNum()
        {
            string firsTwoDigits = rng.Next(0, 99).ToString("00");
            string nextFourDigits = rng.Next(0, 1000).ToString("0000");
            string lastFourDigits = rng.Next(0, 1000).ToString("0000");
            string phoneNum = "08" + firsTwoDigits + "-" + nextFourDigits + "-" + lastFourDigits;
            return phoneNum;
        }

        private DateTime generateDoB()
        {
            DateTime startDate = new DateTime(1960, 1, 1);
            DateTime endDate = new DateTime(2000, 1, 1);
            int rangeDate = (endDate - startDate).Days;
            DateTime RandomDay = startDate.AddDays(rng.Next(rangeDate));
            return RandomDay;
        }
    }
}