using MasterUniversityNonRelational.Api.Models;

namespace MasterUniversityNonRelational.Api.Interfaces
{
    public interface IUniversityService
    {
        Task<IEnumerable<UniversityData>> GetAllAsync();
        Task<UniversityData> GetByIdAsync(Guid id);
        Task<UniversityData> Save(UniversityData branchData);
        Task<UniversityData> Update(String Id, UniversityData branchData);
        Task<bool> Delete(Guid id);
    }
}
