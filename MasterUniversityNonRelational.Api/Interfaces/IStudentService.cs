using MasterUniversityNonRelational.Api.Models;

namespace MasterUniversityNonRelational.Api.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(Guid id);
        Task<Student> Save(Student studentData);
        Task<Student> Update(String Id, Student studentData);
        Task<bool> Delete(Guid id);
        Task <string> TestCase(Student studentData, int testCases);
        Task<List<Student>> TestStudentInsert(int testCases, List<UniversityData> universities);
        Task<List<Student>> TestStudentUpdate(int testCase);
    }
}
