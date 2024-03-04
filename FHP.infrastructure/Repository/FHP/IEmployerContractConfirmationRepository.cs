using FHP.dtos.FHP;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployerContractConfirmationRepository
    {
        Task AddAsync(EmployerContractConfirmation entity);
        void Edit(EmployerContractConfirmation entity);
        Task<EmployerContractConfirmation> GetAsync(int id);
        Task<List<EmployerContractConfirmationDetailDto>> GetAllAsync();
        Task<EmployerContractConfirmationDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
