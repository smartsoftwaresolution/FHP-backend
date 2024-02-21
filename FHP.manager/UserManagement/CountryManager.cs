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
    public class CountryManager:ICountryManager
    {
        private readonly ICountryRepository _repository;

        public CountryManager(ICountryRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddCountryModel model)
        {
            await _repository.AddAsync(CountryFactory.Create(model));
        }


        public async Task Edit(AddCountryModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            CountryFactory.Update(data,model);
            _repository.Edit(data);
        }

        public async Task<List<CountryDetailDto>> GetAllAsync()
        {
           return  await _repository.GetAllAsync();
        }
        public async Task<CountryDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
