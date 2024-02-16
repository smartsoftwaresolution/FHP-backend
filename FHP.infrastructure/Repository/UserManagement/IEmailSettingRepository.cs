using FHP.dtos.UserManagement;
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
        Task<List<EmailSettingDetailDto>> GetAllAsync(int CompanyId);
        Task<EmailSettingDetailDto> GetByIdAsync(int id,int CompanyId);
        Task DeleteAsync(int id,int CompanyId);
    }
}
