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
    public class StateManager:IStateManager
    {
        private readonly IStateRepository _repository;

        public StateManager(IStateRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddStateModel model)
        {
            await _repository.AddAsync(StateFactory.Create(model));
        }

        public async Task Edit(AddStateModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            StateFactory.Update(data,model);
            _repository.Edit(data);
        }

        public async Task<List<StateDetailDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<StateDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
             await _repository.DeleteAsync(id);
        }
    }
}
