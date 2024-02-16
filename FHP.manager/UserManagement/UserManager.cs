using DocumentFormat.OpenXml.Bibliography;
using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using FHP.factories.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.models.UserManagement;
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
        public async Task AddAsync(AddUserModel model, int companyId)
        {
           await _repository.AddAsync(UserFactory.Create(model,companyId),model.RoleName);
        }
        public async Task EditAsync(AddUserModel model, int companyId)
        {
            var data = await _repository.GetAsync(model.Id);
            UserFactory.Update(data,model,companyId);
            _repository.Edit(data);
        }

        public async Task<List<UserDetailDto>> GetAllAsync( int companyId)
        {
            return await _repository.GetAllAsync(companyId);
        }

        public async Task<UserDetailDto> GetByIdAsync(int id, int companyId)
        {
            return await _repository.GetByIdAsync(id,companyId);
        }

        public async Task DeleteAsync(int id, int companyId)
        {

             await _repository.DeleteAsync(id,companyId);
        }


        public async Task<UserDetailDto> GetUserByEmail(string Email,int companyId)
        {
           return  await _repository.GetUserByEmail(Email,companyId);
        }

        public async Task<UserDetailDto> GetUserByGovernmentId(string governmentId,int companyId)
        {
          return   await _repository.GetUserByGovernmentId(governmentId,companyId);
        }

        public async Task UserLogIn(LoginModule entity)
        {
            await _repository.UserLogIn(entity);
        }

        public async Task UserLogOut(int userId,int companyId)
        {
            await _repository.UserLogOut(userId,companyId);
        }
    }
}
