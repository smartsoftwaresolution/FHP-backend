using FHP.dtos.FHP.EmployerContractConfirmation;
using FHP.models.FHP.EmployerContractConfirmation;
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
        Task<(List<EmployerContractConfirmationDetailDto> employerContract , int totalCount)> GetAllAsync(int page,int pageSize,string? search);
        Task<EmployerContractConfirmationDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
