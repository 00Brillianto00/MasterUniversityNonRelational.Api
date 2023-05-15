using MongoDB.Bson.Serialization.Attributes;

namespace MasterUniversityNonRelational.Api.Models
{
    public class Enrollment
    {
        [BsonId]
        public string? Id { get; set; }

        [BsonElement("studentID")]
        public string studentID { get; set; }
        
        [BsonElement("semesterType")]
        public string SemesterType { get; set; }

        [BsonElement("year")]
        public string Year { get; set; }

        [BsonElement("totalCreditPerYear")]
        public int TotalCreditsPerYear { get; set; }

        [BsonElement("totalCostPerSemester")]
        public int TotalCostPerSemester { get; set; }

        [BsonElement("totalCoursePerSemester")]
        public int TotalCoursePerSemester { get; set; }

        [BsonElement("gpaPerSemester")]
        public double GPAPerSemester { get; set; }
        public List<EnrollmentDetail>? enrollmentDetail { get; set; }

        [BsonElement("isdeleted")]
        public bool IsDeleted { get; set; }
    }

    public class EnrollmentDetail
    {
        public Courses courses { get; set; }
        public double AssignmentScore { get; set; }
        public Lecturer lecturer { get; set; } 
        public double MidExamScore { get; set; }
        public double FinalExamScore { get; set; }
        public double CourseAverageScore { get; set; }
    }
}
