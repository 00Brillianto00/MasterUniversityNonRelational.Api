using MasterUniversityNonRelational.Api.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace MasterUniversityNonRelational.Api.Models
{
    [BsonIgnoreExtraElements]
    public class TestResultData
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int MiliSeconds { get; set; }
        public int DataProcessed { get; set; }
        public string AverageTime { get; set; }

    }
}
