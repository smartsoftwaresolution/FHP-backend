using FHP.dtos.FHP.GlobalNotification;
using FHP.dtos.FHP.Skill;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.GlobalNotification;
using FHP.models.FHP.SkillDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.FHP
{
    public class GlobalNotificationManager : IGlobalNotificationManager
    {
        private readonly IGlobalNotificationRepository _repository;
        public GlobalNotificationManager(IGlobalNotificationRepository repository) 
        { 
            _repository = repository;
        }

        public async Task AddAsync(AddGlobalNotificationModel model)
        {
            await _repository.AddAsync(GlobalNotificationFactory.Create(model));
        }

        public async Task Edit(AddGlobalNotificationModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            GlobalNotificationFactory.Update(data, model);
            _repository.Edit(data);
        }

        public async Task<(List<GlobalNotificationDetailDto> notification, int totalCount)> GetAllAsync(int page, int pageSize, string? search, int userId)
        {
            return await _repository.GetAllAsync(page, pageSize, search, userId);
        }

        public async Task<GlobalNotificationDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<int>> UnreadAsync()
        {
            await _repository.UnreadAsync();
        }

    }
}
