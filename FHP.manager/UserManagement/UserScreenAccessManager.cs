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
    public class UserScreenAccessManager: IUserScreenAccessManager
    {
        private readonly IUserScreenAccessRepository _repository;

        public UserScreenAccessManager(IUserScreenAccessRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddUserScreenAccessModel model)
        {
           await _repository.AddAsync(UserScreenAccessFactory.Create(model));
        }

        public async Task Edit(AddUserScreenAccessModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            UserScreenAccessFactory.Update(data, model);
            _repository.Edit(data);
        }

        public async Task<(List<UserScreenAccessDto> userScreenAccess, int totalCount)> GetAllAsync(int page, int pageSize, int roleId)
        {
            return await _repository.GetAllAsync( page,pageSize,roleId);
        }
    }
}
