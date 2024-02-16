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
        Task AddAsync(AddUserModel model,int companyId);
        Task EditAsync(AddUserModel model,int companyId);
        Task<List<UserDetailDto>> GetAllAsync(int companyId);
        Task<UserDetailDto> GetByIdAsync(int id, int companyId);
        Task DeleteAsync(int id, int companyId);
        Task<UserDetailDto> GetUserByEmail(string Email,int companyId);
        Task<UserDetailDto> GetUserByGovernmentId(string governmentId, int companyId);
        Task UserLogIn(LoginModule entity);
        Task UserLogOut(int userId,int companyId);
    }
}
