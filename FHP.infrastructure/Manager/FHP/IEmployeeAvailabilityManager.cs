using FHP.dtos.FHP.EmployeeAvailability;
using FHP.models.FHP.EmployeeAvailability;
using FHP.utilities;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployeeAvailabilityManager
    {
        Task AddAsync(AddEmployeeAvailabilityModel model);
        Task Edit(AddEmployeeAvailabilityModel model);
        Task<(List<EmployeeAvailabilityDetailDto> employeeAval, int totalCount)> GetAllAsync(int page ,int pageSize,string? search,int employeeId, Constants.EmployeeAvailability? employeeAvailability);
        Task<EmployeeAvailabilityDetailDto> GetByIdAsync(int id);
        Task<(List<EmployeeAvailabilityDetailDto> getallAval , int totalCount)> GetAllAvalibility(int page, int pageSize, string? search,int JobId, Constants.EmployeeAvailability? employeeAvailability);
        Task<List<EmployeeAvailabilityDetailDto>> GetByEmployeeIdAsync(int employeeId);
        Task<string> SetEmployeeAvalibility(SetEmployeeAvailabilityModel model);
        Task DeleteAsync(int id);
       // Task GetByJobId(int jobId);
    }
}
