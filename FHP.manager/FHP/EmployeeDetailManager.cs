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
    public class EmployeeDetailManager:IEmployeeDetailManager
    {
        private readonly IEmployeeDetailRepository _repository;

        public EmployeeDetailManager(IEmployeeDetailRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddEmployeeDetailModel model)
        {
           await _repository.AddAsync(EmployeeDetailFactory.Create(model));
        }

  
        public async Task Edit(AddEmployeeDetailModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            EmployeeDetailFactory.Update(data,model);
            _repository.Edit(data);
        }

        public async Task<List<EmployeeDetailDto>> GetAllAsync()
        {
          return  await _repository.GetAllAsync();
        }

        public async Task<EmployeeDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}
