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
        Task<(List<EmployeeAvailabilityDetailDto> employeeAval, int totalCount)> GetAllAsync(int page, int pageSize, string? search, int employeeId, Constants.EmployeeAvailability? employeeAvailability);
        Task<EmployeeAvailabilityDetailDto> GetByIdAsync(int id);
        Task<(List<EmployeeAvailabilityDetailDto> getallAval , int totalCount)> GetAllAvalibility(int page, int pageSize, string? search, int JobId, Constants.EmployeeAvailability? employeeAvailability);
        Task<List<EmployeeAvailabilityDetailDto>> GetByEmployeeIdAsync(int employeeId);
        Task<string> SetEmployeeAvalibility(SetEmployeeAvailabilityModel models);
        Task DeleteAsync(int id);

    }
}
