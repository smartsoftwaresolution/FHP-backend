using FHP.dtos.FHP.Contract;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IContractRepository
    {
        Task AddAsync(Contract entity);
        void Edit(Contract entity);
        Task<Contract> GetAsync(int id);
        Task<(List<ContractDetailDto> contract, int totalCount)> GetAllAsync(int page, int pageSize, string? search);

        Task<ContractDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
