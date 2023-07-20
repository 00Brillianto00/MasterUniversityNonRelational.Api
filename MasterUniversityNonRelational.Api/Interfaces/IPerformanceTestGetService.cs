using MasterUniversityNonRelational.Api.Models;
using NUnit.Framework.Internal;

namespace MasterUniversityNonRelational.Api.Interfaces
{
    public interface IPerformanceTestGetService
    {
        //Task<IEnumerable<TestResultData>> GetAllAsync();
        Task<TestResultData> GetLatestPerformanceTestData();
        Task<List<TestResultData>> GetTopPerformanceTestData(int topData);
        Task<TestResultData> SavePerformanceTestData(TestResultData testData);
        Task<List<GraphData>> GetTopPerformanceGraphData();
    }
}
