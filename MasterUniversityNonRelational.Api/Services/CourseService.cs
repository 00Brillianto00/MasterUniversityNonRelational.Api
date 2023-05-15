using MasterUniversityNonRelational.Api.Interfaces;
using MasterUniversityNonRelational.Api.Models;
using MongoDB.Driver;

namespace MasterUniversityNonRelational.Api.Services
{
    public class CourseService : ICourseService
    {
        //private readonly IDatabaseSettings _databaseSettings;
        private readonly IMongoCollection<Courses> _course;

        public CourseService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings)
        {

            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
            databaseSettings.CollectionName = "Course";
            _course = database.GetCollection<Courses>(databaseSettings.CollectionName);
        }

        public async Task<IEnumerable<Courses>> GetAllAsync()
        {
            try
            {
                var data = await  _course.Find(Courses => Courses.IsDeleted == false).ToListAsync();
                //var data = await  _course.Find(Courses => true).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<Courses> GetByIdAsync(Guid id)
        {
            var ID = id.ToString();
            try
            {
                var data = await  _course.Find(Courses => Courses.Id.Equals(ID) && Courses.IsDeleted == false).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<Courses> Save(Courses universityData)
        {
            universityData.Id = Guid.NewGuid().ToString();
            universityData.IsDeleted = false;
            try
            {
                await  _course.InsertOneAsync(universityData);
                return universityData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Saving Data");
            }
        }

        public async Task<Courses> Update(string id, Courses universityData)
        {
            universityData.Id = id;
            universityData.IsDeleted = false;
            try
            {
                var data = await  _course.ReplaceOneAsync(universityData => universityData.Id.Equals(id), universityData);
                return universityData;
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
                await  _course.DeleteOneAsync(universityData => universityData.Id.Equals(ID));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("error when deleting data");
            }
        }
    }
}
