using MasterUniversityNonRelational.Api.Models;

namespace MasterUniversityNonRelational.Api.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<Enrollment> GetByIdAsync(Guid id);
        Task<Enrollment> Save(Enrollment branchData);
        Task<Enrollment> Update(String Id, Enrollment branchData);
        Task<bool> Delete(Guid id);
        Task<List<Enrollment>> TestEnrollmentInsert(int testCases, List<UniversityData> universities, List<Lecturer> lecturers, List<Courses> courses, List<Student> students);
        Task<List<Enrollment>> TestEnrollmentUpdate(int testCases, List<UniversityData> universities, List<Lecturer> lecturers, List<Courses> courses, List<Student> students);
        Task<List<StudentEnrollmentDataModel>> TestEnrollmentGet(int testCases, List<Student> students);
        Task<bool> TestEnrollmentDelete(List<Student> students);
    }
}
