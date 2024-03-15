using FHP.dtos.FHP.EmployeeSkill;
using FHP.models.FHP.EmployeeSkillDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployeeSkillDetailManager
    {
        Task AddAsync(AddEmployeeSkillDetailModel model);
        Task Edit(AddEmployeeSkillDetailModel model);
        Task<(List<EmployeeSkillDetailDto> employeeSkillDetail , int totalCount)> GetAllAsync(int page, int pageSize,int userId, string? search, string? skillName);
        Task<EmployeeSkillDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
