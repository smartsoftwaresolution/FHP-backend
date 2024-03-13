using FHP.dtos.UserManagement.State;
using FHP.factories.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.models.UserManagement.State;
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

        public async Task<(List<StateDetailDto>state,int totalCount)> GetAllAsync(int page ,int pageSize,string search)
        {
            return await _repository.GetAllAsync(page,pageSize,search);
        }

        public async Task<StateDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
             await _repository.DeleteAsync(id);
        }

        public async Task<List<StateDetailDto>> GetByCountryIdAsync(int countryId)
        {
            return await _repository.GetByCountryIdAsync(countryId);
        }

    }
}
