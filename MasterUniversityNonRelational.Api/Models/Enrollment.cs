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

        [BsonElement("totalCreditsPerSemester")]
        public int TotalCreditsPerSemester { get; set; }

        [BsonElement("totalCostPerSemester")]
        public int TotalCostPerSemester { get; set; }

        [BsonElement("totalCoursePerSemester")]
        public int TotalCoursePerSemester { get; set; }

        [BsonElement("averageScorePerSemester")]
        public double AverageScorePerSemester { get; set; }
        public List<EnrollmentDetail>? enrollmentDetail { get; set; }

        [BsonElement("isdeleted")]
        public bool IsDeleted { get; set; }
    }

    public class EnrollmentDetail
    {
        
        [BsonElement("lecturerId")]
        public string LecturerID { get; set; }

        [BsonElement("courseId")]
        public string CourseID { get; set; }

        [BsonElement("assignmentScore")]
        public double AssignmentScore { get; set; }

        [BsonElement("midExamScore")]
        public double MidExamScore { get; set; }

        [BsonElement("finalExamScore")]
        public double FinalExamScore { get; set; }

        [BsonElement("courseAverageScore")]
        public double CourseAverageScore { get; set; }
    }
}
