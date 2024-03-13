using FHP.dtos.FHP.EmployerContractConfirmation;
using FHP.entity.FHP;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployerContractConfirmationRepository
    {
        Task AddAsync(EmployerContractConfirmation entity);
        void Edit(EmployerContractConfirmation entity);
        Task<EmployerContractConfirmation> GetAsync(int id);
        Task<(List<EmployerContractConfirmationDetailDto> employerContract , int totalCount)> GetAllAsync(int page,int pageSize,string? search);
        Task<EmployerContractConfirmationDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
