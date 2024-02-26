using FHP.dtos.FHP;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface ISkillsDetailRepository
    {
        Task AddAsync(SkillsDetail entity);
        Task<SkillsDetail> GetAsync(int id);
        void Edit(SkillsDetail entity);
        Task<(List<SkillsDetailDto> skill ,int totalCount)> GetAllAsync(int page ,int pageSize,string? search);
        Task<SkillsDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
