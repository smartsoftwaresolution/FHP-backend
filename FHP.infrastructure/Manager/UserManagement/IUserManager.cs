using FHP.dtos.UserManagement.User;
using FHP.entity.UserManagement;
using FHP.models.UserManagement.User;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IUserManager
    {
        Task<int> AddAsync(AddUserModel model);
        Task EditAsync(AddUserModel model);
        Task<(List<UserDetailDto> user,int totalCount)> GetAllAsync(int page,int pageSize,string? search,string? roleName,bool isAscending);
        Task<UserDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<UserDetailDto> GetUserByEmail(string Email);
        Task<UserDetailDto> GetUserByGovernmentId(string governmentId);
        Task UserLogIn(LoginModule entity);
        Task UserLogOut(int userId);
        Task VerifyUser(int userId);
        Task AddUserPic(int userId, string picUrl);
        Task<string> EnableDisableUser(int userId,string roleName);
        Task ChangePassword(int userId, string password);
        Task VerifyEmployerByAdmin(int userId);
        Task<bool> SaveOtp(string email, int otp);
        Task AddFCMToken(FCMToken entity);
        Task RemoveFCMToken(int userId,string fcmToken);
    }
}
