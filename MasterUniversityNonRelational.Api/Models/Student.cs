using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MasterUniversityNonRelational.Api.Models
{
    public class Student
    {

        [BsonId]
        public string Id { get; set; }

        [BsonElement("universityID")]
        public string UniversityID { get; set; }

        [BsonElement("studentNumber")]
        public int StudentNumber { get; set; }
        
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

        [BsonElement("isDeleted ")]
        public bool IsDeleted { get; set; }
    }
}
