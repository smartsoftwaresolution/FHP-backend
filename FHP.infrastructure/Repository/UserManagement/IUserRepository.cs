using FHP.dtos.UserManagement.User;
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
        Task<int> AddAsync(User entity,string roleName);
        Task<User> GetAsync(int id);
        void Edit(User entity);
        Task<(List<UserDetailDto> user, int totalCount)> GetAllAsync(int page,int pageSize,string? search,string? roleName,bool isAscending, string? skills, string? employmentStatus, string? experience, string? jobTitle, string? rolesAndResponsibilities);
        Task<UserDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);   
        Task<UserDetailDto> GetUserByEmail(string Email);
        Task<UserDetailDto> GetUserByGovernmentId(string governmentId);
        Task UserLogIn(LoginModule entity);
        Task UserLogOut(int userId);
        Task VerifyUser(int userId);
        Task AddUserPic(int userId, string picUrl);
        Task<string> EnableDisableUser(int userId, string roleName);
        Task ChangePassword(int userId, string password);
        Task VerifyEmployerByAdmin(int userId);
        Task<bool> SaveOtp(string email, int otp);
        Task AddFCMToken(FCMToken entity);

        Task RemoveFCMToken(int userId, string fcmToken);
        Task<double> CalculateUserPercentage(int userId);
        Task<double> CalculateEmployeeDetailPercentage(int userId);
        Task<double> CalculateEmployeeEducationalDetailPercentage(int userId);
        Task<double> CalculateEmployeeProfessionalDetailPercentage(int userId);
        Task<double> CalculateEmployeeSkillDetailPercentage(int userId);
    }
}
