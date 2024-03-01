using FHP.dtos.FHP;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployeeSkillDetailRepository
    {
        Task AddAsync(EmployeeSkillDetail entity);
        void Edit(EmployeeSkillDetail entity);
        Task<EmployeeSkillDetail> GetAsync(int id);
        Task<(List<EmployeeSkillDetailDto> employeeSkillDetail, int totalCount)> GetAllAsync(int page, int pageSize, string? search);
        Task<EmployeeSkillDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
