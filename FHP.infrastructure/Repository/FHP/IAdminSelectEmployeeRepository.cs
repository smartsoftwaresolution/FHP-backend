using FHP.dtos.FHP.AdminSelectEmployee;
using FHP.dtos.UserManagement.User;
using FHP.entity.FHP;
using FHP.models.FHP.AdminSelectEmployee;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IAdminSelectEmployeeRepository
    {
        Task AddAsync(AdminSelectEmployee entity);
        void Edit(AdminSelectEmployee entity);
        Task<AdminSelectEmployee> GetAsync(int id);
        Task<(List<AdminSelectEmployeeDetailDto> adminSelect, int totalCount)> GetAllAsync(int page, int pageSize,int jobId, string? search);

        Task<AdminSelectEmployeeDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<(List<UserDetailDto> adminSelect, int totalCount)> GetAllJobEmployeeAsync(int jobId);
        Task AddAsync(AddAdminSelectEmployeeModel model);
        Task<string> AcceptRejectAsync(int jobId,int employeeId);
        Task<string> SetStatus(SetAdminSelectEmployeeModel model);
    }
}
