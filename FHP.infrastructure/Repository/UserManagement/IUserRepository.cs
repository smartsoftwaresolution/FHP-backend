using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.UserManagement
{
    public interface IUserRepository
    {
        Task AddAsync(User entity,string roleName);
        Task<User> GetAsync(int id);
        void Edit(User entity);
        Task<(List<UserDetailDto> user, int totalCount)> GetAllAsync(int page,int pageSize,string? search,string? roleName);
        Task<UserDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);   
        Task<UserDetailDto> GetUserByEmail(string Email);
        Task<UserDetailDto> GetUserByGovernmentId(string governmentId);
        Task UserLogIn(LoginModule entity);
        Task UserLogOut(int userId);
    }
}
