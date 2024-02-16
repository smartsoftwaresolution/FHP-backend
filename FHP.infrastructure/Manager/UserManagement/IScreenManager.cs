using FHP.dtos.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IScreenManager
    {
        Task AddAsync(AddScreenModel model, int CompanyId);
        Task EditAsync(AddScreenModel model, int CompanyId);
        Task<List<ScreenDetailDto>> GetAllAsync(int CompanyId);
        Task<ScreenDetailDto> GetByIdAsync(int id,int CompanyId);
        Task DeleteAsync(int id, int CompanyId);
    }
}
