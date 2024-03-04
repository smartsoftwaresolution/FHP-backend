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
    public class EmployeeAvailabilityManager : IEmployeeAvailabilityManager
    {
        private readonly IEmployeeAvailabilityRepository _repository;

        public EmployeeAvailabilityManager(IEmployeeAvailabilityRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddEmployeeAvailabilityModel model)
        {
            await _repository.AddAsync(EmployeeAvailabilityFactory.Create(model));  
        }

        public async Task Edit(AddEmployeeAvailabilityModel model)
        {
             var data = await _repository.GetAsync(model.Id);
            EmployeeAvailabilityFactory.Update(data,model);
            _repository.Edit(data);
        }

        public async Task<(List<EmployeeAvailabilityDetailDto> employeeAval, int totalCount)> GetAllAsync(int page, int pageSize, string? search)
        {
            return await _repository.GetAllAsync(page, pageSize, search);
        }

        public async Task<EmployeeAvailabilityDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<EmployeeAvailabilityDetailDto>> GetAllAvalibility(int JobId)
        {
            return await _repository.GetAllAvalibility(JobId);
        }
    }
}
