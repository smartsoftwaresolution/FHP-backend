using FHP.dtos.FHP.Skill;
using FHP.models.FHP.SkillDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface ISkillsDetailManager
    {
        Task AddAsync(AddSkillsDetailModel model);
        Task Edit(AddSkillsDetailModel model);
        Task<(List<SkillsDetailDto> skill,int totalCount)> GetAllAsync(int page,int pageSize,string? search);
        Task<SkillsDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
