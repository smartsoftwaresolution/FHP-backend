using FHP.dtos.FHP;
using FHP.models.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployerContractConfirmationManager
    {
        Task AddAsync(AddEmployerContractConfirmationModel model);
        Task Edit(AddEmployerContractConfirmationModel model);
        Task<List<EmployerContractConfirmationDetailDto>> GetAllAsync();
        Task<EmployerContractConfirmationDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
