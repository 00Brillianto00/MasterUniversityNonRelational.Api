using MasterUniversityNonRelational.Api.Interfaces;
using MasterUniversityNonRelational.Api.Models;
using MongoDB.Driver;

namespace MasterUniversityNonRelational.Api.Services
{
    public partial class EnrollmentService :IEnrollmentService
    {
        private readonly IMongoCollection<Enrollment> _enrollment;
        private readonly IMongoCollection<Courses> _courses;

        public EnrollmentService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings)
        {

            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
            databaseSettings.CollectionName = "Enrollment";
            _enrollment = database.GetCollection<Enrollment>(databaseSettings.CollectionName);
            databaseSettings.CollectionName = "Courses";
            _courses = database.GetCollection<Courses>(databaseSettings.CollectionName);
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
            Random rnd = new Random();
            int month = rnd.Next(1, 13);
            int countCourses = 0;
            int countCredit = 0;
            int countCost = 0;
            double countGPA = 0;
            foreach (var data in enrollmentData.enrollmentDetail)
            {

                countCourses++;
                data.AssignmentScore = rnd.Next(1, 100);
                data.MidExamScore = rnd.Next(1, 100);
                data.FinalExamScore = rnd.Next(1, 100);
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
    }
}
