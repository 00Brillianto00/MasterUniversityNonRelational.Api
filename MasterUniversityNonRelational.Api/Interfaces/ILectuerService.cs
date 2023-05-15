using MasterUniversityNonRelational.Api.Models;

namespace MasterUniversityNonRelational.Api.Interfaces
{
    public interface ILectuerService
    {
        Task<IEnumerable<Lecturer>> GetAllAsync();
        Task<Lecturer> GetByIdAsync(Guid id);
        Task<Lecturer> Save(Lecturer studentData);
        Task<Lecturer> Update(String Id, Lecturer studentData);
        Task<bool> Delete(Guid id);
    }
}
