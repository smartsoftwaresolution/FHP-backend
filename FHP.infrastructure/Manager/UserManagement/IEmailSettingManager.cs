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
        Task AddAsync(AddEmailSettingModel model, int companyId);
        Task EditAsync(AddEmailSettingModel model, int companyId);
        Task<List<EmailSettingDetailDto>> GetAllAsync(int companyId);
        Task<EmailSettingDetailDto> GetByIdAsync(int id,int companyId);
        Task DeleteAsync(int id, int companyId);
    }
}
