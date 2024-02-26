using FHP.dtos.FHP;
using FHP.entity.FHP;
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
    public class EmployeeEducationalDetailManager : IEmployeeEducationalDetailManager
    {
        private readonly IEmployeeEducationalDetailRepository _repository;

        public EmployeeEducationalDetailManager(IEmployeeEducationalDetailRepository repository)
        {
            _repository=repository;
        }
        public async Task AddAsync(AddEmployeeEducationalDetailModel model)
        {
            await _repository.AddAsync(EmployeeEducationalDetailFactory.Create(model));    
        }


        public async Task Edit(AddEmployeeEducationalDetailModel model)
        {
          var data =  await _repository.GetAsync(model.Id);
            EmployeeEducationalDetailFactory.Update(data,model);
            _repository.Edit(data);
        }


        public async Task<(List<EmployeeEducationalDetailDetailDto> employeeeducationaldetail, int totalCount)> GetAllAsync(int page, int pageSize, string? search)
        {
          return await _repository.GetAllAsync(page,pageSize, search);  
        }

        public async Task<EmployeeEducationalDetailDetailDto> GetByIdAsync(int id)
        {
           return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

      
    }
}
