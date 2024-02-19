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
    public class EmailSettingManager : IEmailSettingManager
    {
        private readonly IEmailSettingRepository _repository;

        public EmailSettingManager(IEmailSettingRepository repository)
        {
            _repository= repository;
        }
        public async Task AddAsync(AddEmailSettingModel model, int companyId)
        {
            await _repository.AddAsync(EmailSettingFactory.Create(model, companyId));
        }

        public async Task EditAsync(AddEmailSettingModel model,int companyId)
        {
          var data=  await _repository.GetAsync(model.Id);
            EmailSettingFactory.Update(data, model, companyId);
            _repository.Edit(data);
        }
        public async Task<List<EmailSettingDetailDto>> GetAllAsync(int companyId)
        {
           return  await _repository.GetAllAsync(companyId);
        }

        public async Task<EmailSettingDetailDto> GetByIdAsync(int id,int companyId)
        {
          return  await _repository.GetByIdAsync(id, companyId);
        }

        public async  Task DeleteAsync(int id,int companyId)
        {
            await _repository.DeleteAsync(id,companyId);
        }
    }
}
