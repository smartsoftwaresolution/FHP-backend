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
        Task<int> AddAsync(AddContractModel model);
        Task Edit(AddContractModel model);
        Task<(List<ContractDetailDto> contract,int totalCount)> GetAllAsync(int page, int pageSize, string? search,int employeeId,int employerId);
        Task<ContractDetailDto> GetByIdAsync(int id);   
        Task DeleteAsync(int id);

        Task AddPdfFile(int id, string file);
        
        Task<string> GetPdfUrlByContractIdAsync(int id);
    }
}
