using MasterUniversityNonRelational.Api.Models;

namespace MasterUniversityNonRelational.Api.Interfaces
{
    public interface ILecturerService
    {
        Task<IEnumerable<Lecturer>> GetAllAsync();
        Task<Lecturer> GetByIdAsync(Guid id);
        Task<Lecturer> Save(Lecturer lecturerData);
        Task<Lecturer> Update(String Id, Lecturer lecturerData);
        Task<bool> Delete(Guid id);
    }
}
