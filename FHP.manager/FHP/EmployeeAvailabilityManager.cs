using FHP.dtos.FHP.EmployeeAvailability;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.EmployeeAvailability;
using FHP.utilities;

namespace FHP.manager.FHP
{
    public class EmployeeAvailabilityManager : IEmployeeAvailabilityManager
    {
        private readonly IEmployeeAvailabilityRepository _repository;

        public EmployeeAvailabilityManager(IEmployeeAvailabilityRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddEmployeeAvailabilityModel model)
        {
            await _repository.AddAsync(model);  
        }

        public async Task Edit(AddEmployeeAvailabilityModel model)
        {
             var data = await _repository.GetAsync(model.Id);
             EmployeeAvailabilityFactory.Update(data,model);
            _repository.Edit(data);
        }

        public async Task<(List<EmployeeAvailabilityDetailDto> employeeAval, int totalCount)> GetAllAsync(int page, int pageSize, string? search, int employeeId, Constants.EmployeeAvailability? employeeAvailability)
        {
            return await _repository.GetAllAsync(page, pageSize, search, employeeId, employeeAvailability);
        }

        public async Task<EmployeeAvailabilityDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<(List<EmployeeAvailabilityDetailDto> getallAval ,int totalCount)> GetAllAvalibility(int page, int pageSize, string? search,int JobId, Constants.EmployeeAvailability? employeeAvailability)
        {
            return await _repository.GetAllAvalibility(page,pageSize,search,JobId,employeeAvailability);
        }

        public async Task<List<EmployeeAvailabilityDetailDto>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _repository.GetByEmployeeIdAsync(employeeId);
        }
        
        public async Task<string> SetEmployeeAvalibility(SetEmployeeAvailabilityModel model)
        {
            return await _repository.SetEmployeeAvalibility(model);
        }
    }
}
