using FHP.dtos.FHP.EmployeeSkill;
using FHP.entity.FHP;
using FHP.models.FHP.EmployeeSkillDetail;
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
        Task AddAsync(AddEmployeeSkillDetailModel entity);
        void Edit(EmployeeSkillDetail entity);
        Task<EmployeeSkillDetail> GetAsync(int id);
        Task<(List<EmployeeSkillDetailDto> employeeSkillDetail, int totalCount)> GetAllAsync(int page, int pageSize, int userId, string? search, string? skillName);
        Task<EmployeeSkillDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task DeleteByIdsAsync(List<int> ids);
    }
}
