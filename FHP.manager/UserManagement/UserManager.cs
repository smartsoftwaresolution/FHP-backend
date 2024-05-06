using FHP.dtos.UserManagement.User;
using FHP.entity.FHP;
using FHP.entity.UserManagement;
using FHP.factories.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.models.UserManagement.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.UserManagement
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _repository;

        public UserManager(IUserRepository repository)
        {
            _repository=repository;
        }
        public async Task<int> AddAsync(AddUserModel model)
        {
          return await _repository.AddAsync(UserFactory.Create(model),model.RoleName);
        }
        public async Task EditAsync(AddUserModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            UserFactory.Update(data,model);
            _repository.Edit(data);
        }

        public async Task<(List<UserDetailDto> user,int totalCount)> GetAllAsync(int page ,int pageSize,string? search,string? roleName,bool isAscending, List<int> ids, string? employmentStatus, string? experience, string? jobTitle, string? rolesAndResponsibilities, string? employmentType)
        {
            return await _repository.GetAllAsync(page,pageSize,search,roleName,isAscending,ids,experience,employmentStatus,jobTitle,rolesAndResponsibilities,employmentType);
        }

        public async Task<UserDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {

             await _repository.DeleteAsync(id);
        }


        public async Task<UserDetailDto> GetUserByEmail(string Email)
        {
           return  await _repository.GetUserByEmail(Email);
        }

        public async Task<UserDetailDto> GetUserByGovernmentId(string governmentId)
        {
          return   await _repository.GetUserByGovernmentId(governmentId);
        }

        public async Task UserLogIn(LoginModule entity)
        {
            await _repository.UserLogIn(entity);
        }

        public async Task UserLogOut(int userId)
        {
            await _repository.UserLogOut(userId);
        }

        public async Task VerifyUser(int userId)
        {
            await _repository.VerifyUser(userId);
        }

        public async Task AddUserPic(int userId,string picUrl)
        {
            await _repository.AddUserPic(userId, picUrl);
        }

        public async Task<string> EnableDisableUser(int userId, string roleName)
        {
            return await _repository.EnableDisableUser(userId, roleName);
        }

        public async Task ChangePassword(int userId, string password)
        {
             await _repository.ChangePassword(userId, password);
        }

        public async Task VerifyEmployerByAdmin(int userId)
        {
            await _repository.VerifyEmployerByAdmin(userId);
        }

        public async  Task<bool> SaveOtp(string email, int otp)
        {
          return  await _repository.SaveOtp(email, otp);
        }

        public async Task AddFCMToken(FCMToken entity)
        {
            await _repository.AddFCMToken(entity);
        }
        public async Task RemoveFCMToken(int userId, string fcmToken)
        {
            await _repository.RemoveFCMToken(userId, fcmToken);
        }


        public async Task<double> ProfilePercentage(int userId)
        {
            // Define weightage for each detail type
            double weightageUserDetail = 0.2;
            double weightageEmployeeDetail = 0.3;
            double weightageEmployeeEducationalDetail = 0.2;
            double weightageEmployeeProfessionalDetail = 0.2;
            double weightageEmployeeSkillDetail = 0.1;

            // Calculate detail fulfillment percentage for each detail type
            double fulfillmentPercentageEmployeeDetail = await _repository.CalculateUserPercentage(userId);
            double fulfillmentPercentageEmployeeDetail2 = await _repository.CalculateEmployeeDetailPercentage(userId);
            double fulfillmentPercentageEmployeeEducationalDetail = await _repository.CalculateEmployeeEducationalDetailPercentage(userId);
            double fulfillmentPercentageEmployeeProfessionalDetail = await _repository.CalculateEmployeeProfessionalDetailPercentage(userId);
            double fulfillmentPercentageEmployeeSkillDetail = await _repository.CalculateEmployeeSkillDetailPercentage(userId);


            double overallPercentage =
           (fulfillmentPercentageEmployeeDetail * weightageUserDetail) +
           (fulfillmentPercentageEmployeeDetail2 * weightageEmployeeDetail) +
           (fulfillmentPercentageEmployeeEducationalDetail * weightageEmployeeEducationalDetail) +
           (fulfillmentPercentageEmployeeProfessionalDetail * weightageEmployeeProfessionalDetail) +
           (fulfillmentPercentageEmployeeSkillDetail * weightageEmployeeSkillDetail);

            return overallPercentage;
        }
    }
}
