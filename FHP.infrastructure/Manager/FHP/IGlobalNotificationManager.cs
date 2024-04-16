using FHP.dtos.FHP.GlobalNotification;
using FHP.dtos.FHP.Skill;
using FHP.models.FHP.GlobalNotification;
using FHP.models.FHP.SkillDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IGlobalNotificationManager
    {
        Task AddAsync(AddGlobalNotificationModel model);
        Task Edit(AddGlobalNotificationModel model);
        Task<(List<GlobalNotificationDetailDto> notification, int totalCount)> GetAllAsync(int page, int pageSize, string? search,int userId);
        Task<GlobalNotificationDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<List<int>> UnreadAsync();
    }
}
