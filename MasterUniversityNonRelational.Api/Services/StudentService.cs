using MasterUniversityNonRelational.Api.Interfaces;
using MasterUniversityNonRelational.Api.Models;
using MongoDB.Driver;

namespace MasterUniversityNonRelational.Api.Services
{
    public class StudentService : IStudentService
    {
        private IMongoCollection<Student> _student;

        public StudentService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings)
        {

            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
            databaseSettings.CollectionName = "Student";
            _student = database.GetCollection<Student>(databaseSettings.CollectionName);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            try
            {
                var data = await _student.Find(Student => Student.IsDeleted == false).ToListAsync();
                //var data = await _student.Find(Student => true).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<Student> GetByIdAsync(Guid id)
        {
            var ID = id.ToString();
            try
            {
                var data = await _student.Find(Student => Student.Id.Equals(ID) && Student.IsDeleted == false).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<Student> Save(Student studentData)
        {
            studentData.Id = Guid.NewGuid().ToString();
            studentData.IsDeleted = false;
            try
            {
                await _student.InsertOneAsync(studentData);
                return studentData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Saving Data");
            }
        }

        public async Task<Student> Update(string id, Student studentData)
        {
            studentData.Id = id;
            studentData.IsDeleted = false;
            try
            {
                var data = await _student.ReplaceOneAsync(studentData => studentData.Id.Equals(id), studentData);
                return studentData;
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
                await _student.DeleteOneAsync(studentData => studentData.Id.Equals(ID));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("error when deleting data");
            }
        }
    }
}
