using FHP.dtos.FHP.EmployeeAvailability;
using FHP.models.FHP.EmployeeAvailability;
using FHP.utilities;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployeeAvailabilityManager
    {
        Task AddAsync(AddEmployeeAvailabilityModel model);
        Task Edit(AddEmployeeAvailabilityModel model);
        Task<(List<EmployeeAvailabilityDetailDto> employeeAval, int totalCount)> GetAllAsync(int page ,int pageSize,string? search);
        Task<EmployeeAvailabilityDetailDto> GetByIdAsync(int id);
        Task<List<EmployeeAvailabilityDetailDto>> GetAllAvalibility(int JobId, Constants.EmployeeAvailability? employeeAvailability);
        Task<List<EmployeeAvailabilityDetailDto>> GetByEmployeeIdAsync(int employeeId);
        Task<string> SetEmployeeAvalibility(int EmployeeId,int JobId);
        Task DeleteAsync(int id);
       // Task GetByJobId(int jobId);
    }
}
