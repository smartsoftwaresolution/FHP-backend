using FHP.dtos.FHP.EmployeeAvailability;
using FHP.entity.FHP;
using FHP.models.FHP.EmployeeAvailability;
using FHP.utilities;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployeeAvailabilityRepository
    {
        Task AddAsync(AddEmployeeAvailabilityModel model);
        void Edit(EmployeeAvailability entity);
        Task<EmployeeAvailability> GetAsync(int id);
        Task<(List<EmployeeAvailabilityDetailDto> employeeAval, int totalCount)> GetAllAsync(int page, int pageSize, string? search);
        Task<EmployeeAvailabilityDetailDto> GetByIdAsync(int id);
        Task<List<EmployeeAvailabilityDetailDto>> GetAllAvalibility(int JobId, Constants.EmployeeAvailability? employeeAvailability);
        Task<List<EmployeeAvailabilityDetailDto>> GetByEmployeeIdAsync(int employeeId);
        Task<string> SetEmployeeAvalibility(int EmployeeId, int JobId);
        Task DeleteAsync(int id);

    }
}
