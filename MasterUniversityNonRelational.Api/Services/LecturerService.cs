using MasterUniversityNonRelational.Api.Interfaces;
using MasterUniversityNonRelational.Api.Models;
using MongoDB.Driver;

namespace MasterUniversityNonRelational.Api.Services
{
    public class LecturerService : ILecturerService
    {
        private readonly IMongoCollection<Lecturer> _lecturer;

        public LecturerService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings)
        {

            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
            databaseSettings.CollectionName = "Lecturer";
            _lecturer = database.GetCollection<Lecturer>(databaseSettings.CollectionName);
        }
        public async Task<IEnumerable<Lecturer>> GetAllAsync()
        {
            try
            {
                var data = await _lecturer.Find(Lecturer => Lecturer.IsDeleted == false).ToListAsync();
                //var data = await _lecturer.Find(Lecturer => true).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<Lecturer> GetByIdAsync(Guid id)
        {
            var ID = id.ToString();
            try
            {
                var data = await _lecturer.Find(Lecturer => Lecturer.Id.Equals(ID) && Lecturer.IsDeleted == false).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<Lecturer> Save(Lecturer lecturerData)
        {
            lecturerData.Id = Guid.NewGuid().ToString();
            lecturerData.IsDeleted = false;
            try
            {
                await _lecturer.InsertOneAsync(lecturerData);
                return lecturerData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Saving Data");
            }
        }

        public async Task<Lecturer> Update(string id, Lecturer lecturerData)
        {
            lecturerData.Id = id;
            lecturerData.IsDeleted = false;
            try
            {
                var data = await _lecturer.ReplaceOneAsync(lecturerData => lecturerData.Id.Equals(id), lecturerData);
                return lecturerData;
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
                await _lecturer.DeleteOneAsync(lecturerData => lecturerData.Id.Equals(ID));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("error when deleting data");
            }
        }
    }
}
