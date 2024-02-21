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
        Task<List<PermissionDetailDto>> GetAllAsync(int companyId);
       
        Task<PermissionDetailDto> GetByIdAsync(int id, int companyId);
        Task DeleteAsync(int id, int companyId);
    }
}
