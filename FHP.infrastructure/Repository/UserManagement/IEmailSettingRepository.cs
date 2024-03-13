using FHP.dtos.UserManagement.EmailSetting;
using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.UserManagement
{
    public interface IEmailSettingRepository
    {
        Task AddAsync(EmailSetting entity);
        Task<EmailSetting> GetAsync(int id);
        void Edit(EmailSetting entity);
        Task<(List<EmailSettingDetailDto> emailSetting, int totalCount)> GetAllAsync(int page, int pageSize, string search);
        Task<EmailSettingDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
