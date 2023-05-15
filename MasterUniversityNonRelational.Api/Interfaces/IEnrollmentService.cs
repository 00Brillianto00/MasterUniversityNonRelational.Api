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
    }
}
