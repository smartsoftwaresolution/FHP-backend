using FHP.dtos.FHP;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.FHP
{
    public class EmployeeSkillDetailManager : IEmployeeSkillDetailManager
    {
        private readonly IEmployeeSkillDetailRepository _repository;

        public EmployeeSkillDetailManager(IEmployeeSkillDetailRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddEmployeeSkillDetailModel model)
        {
            await _repository.AddAsync(model);   
        }

        public async Task Edit(AddEmployeeSkillDetailModel model)
        {
          var data =  await _repository.GetAsync(model.Id);
            EmployeeSkillDetailFactory.Update(data, model);
            _repository.Edit(data);
        }

        public async Task<(List<EmployeeSkillDetailDto> employeeSkillDetail, int totalCount)> GetAllAsync(int page, int pageSize, int userId, string? search)
        {
            return await _repository.GetAllAsync(page, pageSize, userId, search);
        }

        public async Task<EmployeeSkillDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
