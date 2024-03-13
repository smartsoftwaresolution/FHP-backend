using FHP.dtos.FHP.AdminSelectEmployee;
using FHP.dtos.FHP.JobPosting;
using FHP.dtos.UserManagement.User;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
