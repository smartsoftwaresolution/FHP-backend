using FHP.dtos.UserManagement.Permission;
using FHP.entity.UserManagement;
using FHP.factories.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.models.UserManagement.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.UserManagement
{
    public class PermissionManager:IPermissionManager
    {
        private readonly IPermissionRepository _repository;

        public PermissionManager(IPermissionRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddPermissionModel model)
        {
            await _repository.AddAsync(PermissionFactory.Create(model));
        }

        public async Task EditAsync(AddPermissionModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            PermissionFactory.Update(data,model);
            _repository.Edit(data);
        }

        public async Task<(List<PermissionDetailDto> permission, int totalCount)> GetAllAsync(int page,int pageSize,string search)
        {
           return  await _repository.GetAllAsync(page,pageSize,search);
        }
        public  async Task<PermissionDetailDto> GetByIdAsync(int id)
        {
           return  await _repository.GetByIdAsync(id);

        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        
    }
}
