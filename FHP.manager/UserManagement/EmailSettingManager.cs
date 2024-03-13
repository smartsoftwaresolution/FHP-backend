using FHP.dtos.UserManagement.EmailSetting;
using FHP.factories.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.models.UserManagement.EmailSetting;
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
        public async Task AddAsync(AddEmailSettingModel model)
        {
            await _repository.AddAsync(EmailSettingFactory.Create(model));
        }

        public async Task EditAsync(AddEmailSettingModel model)
        {
          var data=  await _repository.GetAsync(model.Id);
            EmailSettingFactory.Update(data, model);
            _repository.Edit(data);
        }
        public async Task<(List<EmailSettingDetailDto> emailSetting,int totalCount)> GetAllAsync(int page,int pageSize,string search)
        {
           return  await _repository.GetAllAsync(page,pageSize,search);
        }

        public async Task<EmailSettingDetailDto> GetByIdAsync(int id)
        {
          return  await _repository.GetByIdAsync(id);
        }

        public async  Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
