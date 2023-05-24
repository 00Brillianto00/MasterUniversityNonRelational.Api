using MasterUniversityNonRelational.Api.Models;

namespace MasterUniversityNonRelational.Api.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Courses>> GetAllAsync();
        Task<Courses> GetByIdAsync(Guid id);
        Task<Courses> Save(Courses courseData);
        Task<Courses> Update(String Id, Courses courseData);
        Task<bool> Delete(Guid id);
        Task<Courses> GetByIdStringAsync(string ID);
    }
}
