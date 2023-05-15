using MasterUniversityNonRelational.Api.Interfaces;
using MasterUniversityNonRelational.Api.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterUniversityNonRelational.Api.Services
{
    public partial class UniversityService : IUniversityService
    {
        //private readonly IDatabaseSettings _databaseSettings;
        private readonly IMongoCollection<UniversityData> _university;

        public UniversityService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings)
        {
   
            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
            databaseSettings.CollectionName = "University";
            _university = database.GetCollection<UniversityData>(databaseSettings.CollectionName);
        }

        public async Task<IEnumerable<UniversityData>> GetAllAsync()
        {
            try
            {
                var data = await _university.Find(UniversityData => UniversityData.IsDeleted == false).ToListAsync();
                //var data = await _university.Find(UniversityData => true).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

         public async Task<UniversityData>  GetByIdAsync(Guid id)
        {
            var ID = id.ToString();
            try
            {
                var data = await _university.Find(UniversityData => UniversityData.Id.Equals(ID) && UniversityData.IsDeleted == false).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<UniversityData> Save(UniversityData universityData)
        {
            universityData.Id = Guid.NewGuid().ToString();
            universityData.IsDeleted = false;
            try
            {
                await _university.InsertOneAsync(universityData);
                return universityData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Saving Data");
            }
        }

        public async Task<UniversityData> Update(string id, UniversityData universityData)
        {
            universityData.Id = id;
            universityData.IsDeleted = false;
            try
            {
                var data = await _university.ReplaceOneAsync(universityData => universityData.Id.Equals(id), universityData);
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
                await _university.DeleteOneAsync(universityData => universityData.Id.Equals(ID));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("error when deleting data");
            }
        }
    }
}
