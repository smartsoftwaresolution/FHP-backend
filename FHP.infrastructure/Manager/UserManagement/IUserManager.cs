using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using FHP.models.UserManagement;
using FHP.models.UserManagement.UserLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IUserManager
    {
        Task AddAsync(AddUserModel model);
        Task EditAsync(AddUserModel model);
        Task<(List<UserDetailDto> user,int totalCount)> GetAllAsync(int page,int pageSize,string? search,string? roleName);
        Task<UserDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<UserDetailDto> GetUserByEmail(string Email);
        Task<UserDetailDto> GetUserByGovernmentId(string governmentId);
        Task UserLogIn(LoginModule entity);
        Task UserLogOut(int userId);
    }
}
