using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.UserManagement
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRole entity);
        Task<UserRole> GetAsync(int id);
        void Edit(UserRole entity);
        Task<List<UserRoleDetailDto>> GetAllAsync(int CreatedBy);
        Task<UserRoleDetailDto> GetByIdAsync(int id);

        Task DeleteAsync(int id);
    }
}
