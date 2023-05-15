using MasterUniversityNonRelational.Api.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MasterUniversityNonRelational.Api.Models
{
    [BsonIgnoreExtraElements]
    public class UniversityData
    {
        [BsonId]
        public string? Id { get; set; }
        [BsonElement("branchCode")]
        public string BranchCode { get; set; }

        [BsonElement("branchName")]
        public string BranchName { get; set; }

        [BsonElement("branchLocation")]
        public int BranchLocation { get; set; }

        [BsonElement("branchPostalCode")]
        public int BranchPostalCode { get; set; }

        [BsonElement("facultyCode")]
        public string FacultyCode { get; set; }

        [BsonElement("facultyName")]
        public string FacultyName { get; set; }

        [BsonElement("departmentCode")]
        public string DepartmentCode { get; set; }

        [BsonElement("departmentName")]
        public string DepartmentName { get; set; } 
        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
