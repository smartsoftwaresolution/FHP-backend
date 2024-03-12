using FHP.dtos.FHP;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployeeAvailabilityRepository
    {
        Task AddAsync(EmployeeAvailability entity);
        void Edit(EmployeeAvailability entity);
        Task<EmployeeAvailability> GetAsync(int id);
        Task<(List<EmployeeAvailabilityDetailDto> employeeAval, int totalCount)> GetAllAsync(int page, int pageSize, string? search);
        Task<EmployeeAvailabilityDetailDto> GetByIdAsync(int id);
        Task<List<EmployeeAvailabilityDetailDto>> GetAllAvalibility(int JobId);
        Task<List<EmployeeAvailabilityDetailDto>> GetByEmployeeIdAsync(int employeeId);
        Task<string> SetEmployeeAvalibility(int EmployeeId);
        Task DeleteAsync(int id);

    }
}
