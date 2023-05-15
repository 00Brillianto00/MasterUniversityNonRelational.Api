using MongoDB.Bson.Serialization.Attributes;

namespace MasterUniversityNonRelational.Api.Models
{
    public class Lecturer
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("universityID")]
        public string UniversityID { get; set; }
        [BsonElement("LecturerCode")]
        public string LecturerCode { get; set; }

        [BsonElement("LecturerEmail")]
        public string LecrturerEmail { get; set; }

        [BsonElement("joinedYear")]
        public DateTime joinedYear { get; set; }

        [BsonElement("salary")]
        public int Salary { get; set; }

        [BsonElement("LecturerName")]
        public string LecturerName { get; set; }

        [BsonElement("LecturerDateOfBirth")]
        public DateTime LecturerDateOfBirth { get; set; }

        [BsonElement("LecturerPhoneNumber")]
        public string LecturerPhoneNumber { get; set; }

        [BsonElement("LecturerAddress")]
        public string LecturerAddress { get; set; }

        [BsonElement("LecturerPostalCode")]
        public int LecturerPostalCode { get; set; }
        
        [BsonElement("isDeleted")]
        public bool IsDeleted{ get; set; }
    }
}
