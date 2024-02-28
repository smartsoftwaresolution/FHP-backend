using FHP.dtos.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IEmailSettingManager
    {
        Task AddAsync(AddEmailSettingModel model);
        Task EditAsync(AddEmailSettingModel model);
        Task<(List<EmailSettingDetailDto> emailSetting,int totalCount)> GetAllAsync(int page ,int pageSize ,string search);
        Task<EmailSettingDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
