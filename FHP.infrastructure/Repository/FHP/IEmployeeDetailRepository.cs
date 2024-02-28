using FHP.dtos.FHP;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployeeDetailRepository
    {
        Task AddAsync(EmployeeDetail entity);
        Task<EmployeeDetail> GetAsync(int id);
        void Edit(EmployeeDetail entity);
        Task<(List<EmployeeDetailDto>employee, int totalCount)> GetAllAsync(int page,int pageSize,int userId, string? search);    
        Task<EmployeeDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
