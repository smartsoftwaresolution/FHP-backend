using FHP.dtos.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IUserRoleManager
    {
        Task AddAsync(AddUserRoleModel model);
        Task EditAsync(AddUserRoleModel model);
        Task<List<UserRoleDetailDto>> GetAllAsync();
        Task<UserRoleDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
