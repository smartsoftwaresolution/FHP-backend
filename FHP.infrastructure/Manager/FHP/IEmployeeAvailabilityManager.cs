using FHP.dtos.FHP;
using FHP.models.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployeeAvailabilityManager
    {
        Task AddAsync(AddEmployeeAvailabilityModel model);
        Task Edit(AddEmployeeAvailabilityModel model);
        Task<(List<EmployeeAvailabilityDetailDto> employeeAval, int totalCount)> GetAllAsync(int page ,int pageSize,string? search);
        Task<EmployeeAvailabilityDetailDto> GetByIdAsync(int id);
        Task<List<EmployeeAvailabilityDetailDto>> GetAllAvalibility(int JobId);
        Task DeleteAsync(int id);
    }
}
