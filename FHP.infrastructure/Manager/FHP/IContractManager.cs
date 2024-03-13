using FHP.dtos.FHP.Contract;
using FHP.models.FHP.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IContractManager
    {
        Task AddAsync(AddContractModel model);
        Task Edit(AddContractModel model);
        Task<(List<ContractDetailDto> contract,int totalCount)> GetAllAsync(int page, int pageSize, string? search);
        Task<ContractDetailDto> GetByIdAsync(int id);   
        Task DeleteAsync(int id);   
    }
}
