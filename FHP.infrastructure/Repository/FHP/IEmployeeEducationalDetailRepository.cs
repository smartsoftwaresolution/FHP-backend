using FHP.dtos.FHP;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployeeEducationalDetailRepository
    {
        Task AddAsync(EmployeeEducationalDetail entity);
        Task<EmployeeEducationalDetail> GetAsync(int id);
        void Edit(EmployeeEducationalDetail entity);
        Task<(List<EmployeeEducationalDetailDetailDto> employeeeducationaldetail, int totalCount)> GetAllAsync(int page, int pageSize, string? search);
        Task<EmployeeEducationalDetailDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
