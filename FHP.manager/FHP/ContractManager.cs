using FHP.dtos.FHP.Contract;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.FHP
{
    public class ContractManager : IContractManager
    {
        private readonly IContractRepository _repository;

        public ContractManager(IContractRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddContractModel model)
        {
            await _repository.AddAsync(ContractFactory.Create(model));
        }

  

        public async Task Edit(AddContractModel model)
        {
           var data = await _repository.GetAsync(model.Id);
            ContractFactory.Update(data, model);
            _repository.Edit(data);
        }

        public async Task<(List<ContractDetailDto> contract, int totalCount)> GetAllAsync(int page, int pageSize, string? search, int employeeId)
        {
            return await _repository.GetAllAsync(page, pageSize, search,employeeId);
        }

        public async Task<ContractDetailDto> GetByIdAsync(int id)
        {
          return   await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

      
    }
}
