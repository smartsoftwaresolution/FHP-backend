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
    public class CityManager : ICityManager
    {
        private readonly ICityRepository _repository;

        public CityManager(ICityRepository repository)
        {
            _repository=repository;
        }
        public async Task AddAsync(AddCityModel model)
        {
            await _repository.AddAsync(CityFactory.Create(model));    
        }

        public async Task Edit(AddCityModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            CityFactory.Update(data, model);
            _repository.Edit(data);
        }

        public async Task<(List<CityDetailDto>city,int totalCount)> GetAllAsync(int page,int pageSize,string? search)
        {
           return await _repository.GetAllAsync(page,pageSize,search);

        }

        public async Task<CityDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
