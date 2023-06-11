using MasterUniversityNonRelational.Api.Interfaces;
using MasterUniversityNonRelational.Api.Models;
using MongoDB.Driver;

namespace MasterUniversityNonRelational.Api.Services
{
    public class PerformanceTestDeleteService : IPerformanceTestDeleteService
    {
        private readonly IMongoCollection<TestResultData> _testResult;

        public PerformanceTestDeleteService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings)
        {

            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
            databaseSettings.CollectionName = "PerformanceTestDelete";
            _testResult = database.GetCollection<TestResultData>(databaseSettings.CollectionName);
        }
        
        public async Task<TestResultData> GetLatestPerformanceTestData()
        {
            //TestResultData testResult = new TestResultData();
            try
            {
                var testResult = await _testResult.Find(testResult => true).SortByDescending(testResult => testResult.ID).Limit(1).FirstOrDefaultAsync();
                return testResult;
            }
            catch (Exception ex) {
                throw new Exception("Error when retrieving data");
            }
        }

        public async Task<List<TestResultData>> GetTopPerformanceTestData(int TopData)
        {
            try
            {
                var testResult = await _testResult.Find(testResult => true).SortByDescending(testResult => testResult.ID).Limit(TopData).ToListAsync();
                return testResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Error when retrieving data");
            }
        }


        public async Task<TestResultData> SavePerformanceTestData(TestResultData testData)
        {
            var getLatestData = GetLatestPerformanceTestData();
            if (getLatestData.Result == null)
            {
                testData.ID = 1;
            }
            else
            {
                testData.ID = getLatestData.Result.ID + 1;
            }
            try
            {
                await _testResult.InsertOneAsync(testData);
                return testData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error when saving data");
            }

        }
    }
}
