using FHP.dtos.FHP.EmployerContractConfirmation;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.EmployerContractConfirmation;

namespace FHP.manager.FHP
{
    public class EmployerContractConfirmationManager : IEmployerContractConfirmationManager
    {
        private readonly IEmployerContractConfirmationRepository _repository;

        public EmployerContractConfirmationManager(IEmployerContractConfirmationRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(AddEmployerContractConfirmationModel model)
        {
           await _repository.AddAsync(EmployerContractConfirmationFactory.Create(model));
        }

 

        public async Task Edit(AddEmployerContractConfirmationModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            EmployerContractConfirmationFactory.Update(data, model);
            _repository.Edit(data);
        }

        public async Task<(List<EmployerContractConfirmationDetailDto>employerContract , int totalCount)> GetAllAsync(int page,int pageSize,string? search)
        {
            return await _repository.GetAllAsync(page,pageSize,search);
        }

        public async Task<EmployerContractConfirmationDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}
