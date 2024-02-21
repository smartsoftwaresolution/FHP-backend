using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using FHP.factories.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.models.UserManagement;
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

        public async Task AddAsync(AddPermissionModel model, int companyId)
        {
            await _repository.AddAsync(PermissionFactory.Create(model, companyId));
        }

        public async Task EditAsync(AddPermissionModel model, int companyId)
        {
            var data = await _repository.GetAsync(model.Id);
            PermissionFactory.Update(data,model,companyId);
            _repository.Edit(data);
        }

        public async Task<List<PermissionDetailDto>> GetAllAsync(int companyId)
        {
           return  await _repository.GetAllAsync(companyId);
        }
        public  async Task<PermissionDetailDto> GetByIdAsync(int id, int companyId)
        {
           return  await _repository.GetByIdAsync(id,companyId);

        }

        public async Task DeleteAsync(int id,int companyId)
        {
            await _repository.DeleteAsync(id,companyId);
        }

        
    }
}
