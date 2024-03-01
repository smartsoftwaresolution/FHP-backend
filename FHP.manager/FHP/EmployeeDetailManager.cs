using FHP.dtos.FHP;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP;
using Microsoft.AspNetCore.Mvc.Filters;
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

        public async Task AddAsync(AddEmployeeDetailModel model,string resumeUrl)
        {
           await _repository.AddAsync(EmployeeDetailFactory.Create(model,resumeUrl));
        }

  
        public async Task Edit(AddEmployeeDetailModel model,string resumeUrl)
        {
            var data = await _repository.GetAsync(model.Id);
            EmployeeDetailFactory.Update(data,model,resumeUrl);
            _repository.Edit(data);
        }

        public async Task<(List<EmployeeDetailDto>employee, int totalCount)> GetAllAsync(int page,int pageSize,int userId, string? search)
        {
          return  await _repository.GetAllAsync(page,pageSize,userId,search);
        }

        public async Task<EmployeeDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<string> SetAvailabilityAsync(int id)
        {
            return await _repository.SetAvailabilityAsync(id);
        }

    }
}
