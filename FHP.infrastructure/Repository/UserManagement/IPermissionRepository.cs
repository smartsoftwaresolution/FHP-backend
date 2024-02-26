using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.UserManagement
{
    public interface IPermissionRepository
    {
        Task AddAsync(Permission entity);
        Task<Permission> GetAsync(int id);
        void Edit(Permission entity);
        Task<(List<PermissionDetailDto> permission, int totalCount)> GetAllAsync(int page, int pageSize, string search);
        Task<PermissionDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
