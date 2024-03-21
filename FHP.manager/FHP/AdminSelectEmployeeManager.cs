using FHP.dtos.FHP.AdminSelectEmployee;
using FHP.dtos.UserManagement.User;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.AdminSelectEmployee;

namespace FHP.manager.FHP
{
    public class AdminSelectEmployeeManager : IAdminSelectEmployeeManager
    {
        private readonly IAdminSelectEmployeeRepository _repository;

        public AdminSelectEmployeeManager(IAdminSelectEmployeeRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddAdminSelectEmployeeModel model)
        {
            await _repository.AddAsync(model);
        }

        public async Task Edit(AddAdminSelectEmployeeModel model)
        {
         var data=   await _repository.GetAsync(model.Id);
            AdminSelectEmployeeFactory.Update(data, model);
            _repository.Edit(data);
        }

        public async Task<(List<AdminSelectEmployeeDetailDto> adminSelect, int totalCount)> GetAllAsync(int page, int pageSize,int jobId, string? search)
        {
           return await _repository.GetAllAsync(page, pageSize,jobId, search);
        }

        public async Task<AdminSelectEmployeeDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<(List<UserDetailDto> adminSelect, int totalCount)> GetAllJobEmployeeAsync(int jobId)
        {
            return await _repository.GetAllJobEmployeeAsync(jobId);
        }

        public async Task<string> AcceptRejectAsync(int jobId, int employeeId)
        {
            return await _repository.AcceptRejectAsync(jobId, employeeId);
        }
    }
}
