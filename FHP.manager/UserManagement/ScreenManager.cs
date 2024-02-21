using FHP.dtos.UserManagement;
using FHP.factories.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.UserManagement
{
    public class ScreenManager : IScreenManager
    {
        private readonly IScreenRepository _repository;

        public ScreenManager(IScreenRepository repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(AddScreenModel model, int CompanyId)
        {
            await _repository.AddAsync(ScreenFactory.Create(model, CompanyId));
        }

        public async Task EditAsync(AddScreenModel model, int CompanyId)
        {
            var data = await _repository.GetAsync(model.Id);
            ScreenFactory.Update(data, model, CompanyId);
            _repository.Edit(data);
        }
        public async Task<List<ScreenDetailDto>> GetAllAsync(int CompanyId)
        {
            return await _repository.GetAllAsync(CompanyId);
        }

        public async Task<ScreenDetailDto> GetByIdAsync(int id, int CompanyId)
        {
           return await _repository.GetByIdAsync(id, CompanyId);
        }

        public async Task DeleteAsync(int id,int CompanyId)
        {
            await _repository.DeleteAsync(id, CompanyId);
        }
    }
}
