using MongoDB.Bson.Serialization.Attributes;

namespace MasterUniversityNonRelational.Api.Models
{
    public class Courses
    {
        [BsonId]
        public string Id { get; set; }
        
        [BsonElement("courseCode")]
        public string CourseCode { get; set; }

        [BsonElement("courseName")]
        public string CourseName { get; set; }
        
        [BsonElement("syllabus")]
        public string Syllabus { get; set; }
        
        [BsonElement("credit")]
        public int Credit { get; set; }
        
        [BsonElement("cost")]
        public int Cost { get; set; }

        [BsonElement("isDeleted")]
        public int isDeleted { get; set; }
    }
}
