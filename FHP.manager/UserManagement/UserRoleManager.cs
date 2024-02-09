using FHP.dtos.UserManagement;
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
    public class UserRoleManager:IUserRoleManager
    {
        private readonly IUserRoleRepository _repository;

        public UserRoleManager(IUserRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(AddUserRoleModel model)
        {
            await _repository.AddAsync(UserRoleFactory.Create(model));
        }

 
        public async Task EditAsync(AddUserRoleModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            UserRoleFactory.Update(data, model);
            _repository.Edit(data);

        }

        public async Task<List<UserRoleDetailDto>> GetAllAsync(int id)
        {
          return  await _repository.GetAllAsync(id);
        }

        public async Task<UserRoleDetailDto> GetByIdAsync(int id)
        {
           return  await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}
