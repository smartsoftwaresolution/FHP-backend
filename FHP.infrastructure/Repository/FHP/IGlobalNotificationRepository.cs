using FHP.dtos.FHP.GlobalNotification;
using FHP.dtos.FHP.Skill;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IGlobalNotificationRepository
    {
        Task AddAsync(GlobalNotification entity);
        Task<GlobalNotification> GetAsync(int id);
        void Edit(GlobalNotification entity);
        Task<(List<GlobalNotificationDetailDto> notification, int totalCount)> GetAllAsync(int page, int pageSize, string? search, int userId);
        Task<GlobalNotificationDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<List<int>> UnreadAsync();

    }
}
