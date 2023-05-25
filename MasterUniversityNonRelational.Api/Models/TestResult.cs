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

    public class StudentEnrollmentDataModel
    {
        [BsonId]
        public string? Id { get; set; }

        [BsonElement("universityID")]
        public string UniversityID { get; set; }

        [BsonElement("studentNumber")]
        public long StudentNumber { get; set; }

        [BsonElement("studentEmail")]
        public string StudentEmail { get; set; }

        [BsonElement("enrolledYear")]
        public string EnrolledYear { get; set; }

        [BsonElement("totalCreditsEarned")]
        public int TotalCreditsEarned { get; set; }

        [BsonElement("studentGPA")]
        public double StudentGPA { get; set; }

        [BsonElement("studentName")]
        public string StudentName { get; set; }

        [BsonElement("studentDateOfBirth")]
        public DateTime StudentDateOfBirth { get; set; }

        [BsonElement("studentPhoneNumber")]
        public string StudentPhoneNumber { get; set; }

        [BsonElement("studentAddress")]
        public string StudentAddress { get; set; }

        [BsonElement("studentPostalCode")]
        public int StudentPostalCode { get; set; }
        public List<Enrollment> enrollment { get; set; }
        [BsonElement("isDeleted ")]
        public bool IsDeleted {get; set;}
    }
}