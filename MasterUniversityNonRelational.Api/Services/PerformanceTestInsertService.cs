﻿using MasterUniversityNonRelational.Api.Interfaces;
using MasterUniversityNonRelational.Api.Models;
using MongoDB.Driver;
using System.Runtime.Intrinsics.X86;

namespace MasterUniversityNonRelational.Api.Services
{
    public class PerformanceTestInsertService : IPerformanceTestInsertService
    {
        private readonly IMongoCollection<TestResultData> _testResult;

        public PerformanceTestInsertService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings)
        {

            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
            databaseSettings.CollectionName = "PerformanceTestInsert";
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

        //public async Task<List<GraphData>> GetTopPerformanceGraphData()
        //{
        //    try
        //    {
        //        List<GraphData> graphDatas = new List<GraphData>(); 
        //        GraphData graphData = new GraphData();  
        //        var testResult1000 = await _testResult.Find(testResult => true && testResult.DataProcessed == 1000).SortByDescending(testResult => testResult.ID).ToListAsync();
        //        var testResult5000 = await _testResult.Find(testResult => true && testResult.DataProcessed == 5000).SortByDescending(testResult => testResult.ID).ToListAsync();
        //        var testResult10000 = await _testResult.Find(testResult => true && testResult.DataProcessed == 10000).SortByDescending(testResult => testResult.ID).ToListAsync();
        //        var testResult50000 = await _testResult.Find(testResult => true && testResult.DataProcessed == 50000).SortByDescending(testResult => testResult.ID).ToListAsync();
        //        var testResult100000 = await _testResult.Find(testResult => true && testResult.DataProcessed == 100000).SortByDescending(testResult => testResult.ID).ToListAsync();

        //        //1000
        //        foreach (var data in testResult1000)
        //        {
        //            graphData.AveragePerformanceSpeed = graphData.AveragePerformanceSpeed + data.AverageTime;
        //        }
        //        graphData.AveragePerformanceSpeed = graphData.AveragePerformanceSpeed / testResult1000.Count();
        //        graphData.DataAmount = 1000;
        //        graphDatas.Add(graphData);
        //        graphData = null;

        //        //5000
        //        foreach (var data in testResult5000)
        //        {
        //            graphData.AveragePerformanceSpeed = graphData.AveragePerformanceSpeed + data.AverageTime;
        //        }
        //        graphData.AveragePerformanceSpeed = graphData.AveragePerformanceSpeed / testResult5000.Count();
        //        graphData.DataAmount = 5000;
        //        graphDatas.Add(graphData);
        //        graphData = null;

        //        //10000
        //        foreach (var data in testResult10000)
        //        {
        //            graphData.AveragePerformanceSpeed = graphData.AveragePerformanceSpeed + data.AverageTime;
        //        }
        //        graphData.AveragePerformanceSpeed = graphData.AveragePerformanceSpeed / testResult10000.Count();
        //        graphData.DataAmount = 10000;
        //        graphDatas.Add(graphData);
        //        graphData = null;

        //        //50000
        //        foreach (var data in testResult50000)
        //        {
        //            graphData.AveragePerformanceSpeed = graphData.AveragePerformanceSpeed + data.AverageTime;
        //        }
        //        graphData.AveragePerformanceSpeed = graphData.AveragePerformanceSpeed / testResult50000.Count();
        //        graphData.DataAmount = 50000;
        //        graphDatas.Add(graphData);
        //        graphData = null;

        //        //100000
        //        foreach (var data in testResult100000)
        //        {
        //            graphData.AveragePerformanceSpeed = graphData.AveragePerformanceSpeed + data.AverageTime;
        //        }
        //        graphData.AveragePerformanceSpeed = graphData.AveragePerformanceSpeed / testResult100000.Count();
        //        graphData.DataAmount = 100000;
        //        graphDatas.Add(graphData);
        //        graphData = null;

        //        return graphDatas;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error when retrieving data");
        //    }
        //}

        public async Task<List<GraphData>> GetTopPerformanceGraphData()
        {
            try
            {
                List<GraphData> graphDatas = new List<GraphData>();
                GraphData graphData1k = new GraphData();
                GraphData graphData5k = new GraphData();
                GraphData graphData10k = new GraphData();
                GraphData graphData50k = new GraphData();
                GraphData graphData100k = new GraphData();
                var testResult1000 = await _testResult.Find(testResult => true && testResult.DataProcessed == 1000).SortByDescending(testResult => testResult.ID).ToListAsync();
                var testResult5000 = await _testResult.Find(testResult => true && testResult.DataProcessed == 5000).SortByDescending(testResult => testResult.ID).ToListAsync();
                var testResult10000 = await _testResult.Find(testResult => true && testResult.DataProcessed == 10000).SortByDescending(testResult => testResult.ID).ToListAsync();
                var testResult50000 = await _testResult.Find(testResult => true && testResult.DataProcessed == 50000).SortByDescending(testResult => testResult.ID).ToListAsync();
                var testResult100000 = await _testResult.Find(testResult => true && testResult.DataProcessed == 100000).SortByDescending(testResult => testResult.ID).ToListAsync();

                //1000
                foreach (var data in testResult1000)
                {
                    graphData1k.AveragePerformanceSpeed = graphData1k.AveragePerformanceSpeed + data.AverageTime;
                }
                graphData1k.AveragePerformanceSpeed = graphData1k.AveragePerformanceSpeed / testResult1000.Count();
                graphData1k.DataAmount = 1000;
                graphDatas.Add(graphData1k);

                //5000
                foreach (var data in testResult5000)
                {
                    graphData5k.AveragePerformanceSpeed = graphData5k.AveragePerformanceSpeed + data.AverageTime;
                }
                graphData5k.AveragePerformanceSpeed = graphData5k.AveragePerformanceSpeed / testResult5000.Count();
                graphData5k.DataAmount = 5000;
                graphDatas.Add(graphData5k);

                //10000
                foreach (var data in testResult10000)
                {
                    graphData10k.AveragePerformanceSpeed = graphData10k.AveragePerformanceSpeed + data.AverageTime;
                }
                graphData10k.AveragePerformanceSpeed = graphData10k.AveragePerformanceSpeed / testResult10000.Count();
                graphData10k.DataAmount = 10000;
                graphDatas.Add(graphData10k);

                //50000
                foreach (var data in testResult50000)
                {
                    graphData50k.AveragePerformanceSpeed = graphData50k.AveragePerformanceSpeed + data.AverageTime;
                }
                graphData50k.AveragePerformanceSpeed = graphData50k.AveragePerformanceSpeed / testResult50000.Count();
                graphData50k.DataAmount = 50000;
                graphDatas.Add(graphData50k);

                //100000
                foreach (var data in testResult100000)
                {
                    graphData100k.AveragePerformanceSpeed = graphData100k.AveragePerformanceSpeed + data.AverageTime;
                }
                graphData100k.AveragePerformanceSpeed = graphData100k.AveragePerformanceSpeed / testResult100000.Count();
                graphData100k.DataAmount = 100000;
                graphDatas.Add(graphData100k);

                return graphDatas;
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
