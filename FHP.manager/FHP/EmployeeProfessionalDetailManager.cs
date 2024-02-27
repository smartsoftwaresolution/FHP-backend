using FHP.datalayer;
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
    public class EmployeeProfessionalDetailManager: IEmployeeProfessionalDetailManager
    {
        private readonly IEmployeeProfessionalDetailRepository _repository;

        public EmployeeProfessionalDetailManager(IEmployeeProfessionalDetailRepository repository )
        {
            _repository=repository;
        }

        public async Task AddAsync(AddEmployeeProfessionalDetailModel model)
        {
            await _repository.AddAsync(EmployeeProfessionalDetailFactory.Create(model));
        }

        public async Task Edit(AddEmployeeProfessionalDetailModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            EmployeeProfessionalDetailFactory.Update(data, model);
            _repository.Edit(data);
        }

        public async Task<(List<EmployeeProfessionalDetailDto> employeeProfessionalDetail, int totalCount)> GetAllAsync(int page, int pageSize, string? search)
        {
         return  await _repository.GetAllAsync(page, pageSize, search);
        }

        public async Task<EmployeeProfessionalDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
