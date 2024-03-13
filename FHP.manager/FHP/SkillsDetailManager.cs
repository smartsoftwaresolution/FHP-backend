using FHP.dtos.FHP.Skill;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.SkillDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.FHP
{
    public class SkillsDetailManager : ISkillsDetailManager
    {
        private readonly ISkillsDetailRepository _repository;

        public SkillsDetailManager(ISkillsDetailRepository repository)
        {
            _repository=repository;
        }
        public async Task AddAsync(AddSkillsDetailModel model)
        {
           await _repository.AddAsync(SkillsDetailFactory.Create(model)); 
        }

        public async Task Edit(AddSkillsDetailModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            SkillsDetailFactory.Update(data,model);
            _repository.Edit(data);
        }

        public async Task<(List<SkillsDetailDto> skill,int totalCount)> GetAllAsync(int page ,int pageSize,string? search)
        {
           return await _repository.GetAllAsync(page, pageSize, search);
        }

        public async Task<SkillsDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
