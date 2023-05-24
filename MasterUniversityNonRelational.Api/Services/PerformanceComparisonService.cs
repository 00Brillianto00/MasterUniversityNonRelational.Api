//using MasterUniversityNonRelational.Api.Interfaces;
//using MasterUniversityNonRelational.Api.Models;
//using MongoDB.Driver;
//using NUnit.Framework.Internal;
//using System.Diagnostics;

//namespace MasterUniversityNonRelational.Api.Services
//{
//    public class PerformanceComparisonService : IPerformanceComparisonService
//    {
//        private readonly IMongoCollection<Enrollment> _enrollment;

//        public PerformanceComparisonService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings)
//        {
//            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
//            databaseSettings.CollectionName = "Enrollment";
//            _enrollment = database.GetCollection<Enrollment>(databaseSettings.CollectionName);
//        }

     
//    }
//}
