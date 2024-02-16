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
        Task<List<UserDetailDto>> GetAllAsync( int companyId);
        Task<UserDetailDto> GetByIdAsync(int id, int companyId);
        Task DeleteAsync(int id, int companyId);   
        Task<UserDetailDto> GetUserByEmail(string Email,int  companyId);
        Task<UserDetailDto> GetUserByGovernmentId(string governmentId, int companyId);
        Task UserLogIn(LoginModule entity);
        Task UserLogOut(int userId,int companyId);
    }
}
