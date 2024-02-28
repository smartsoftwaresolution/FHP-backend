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
        Task AddAsync(AddScreenModel model);
        Task EditAsync(AddScreenModel model);
        Task<(List<ScreenDetailDto> screen,int totalCount)> GetAllAsync(int page ,int pageSize,string search);
        Task<ScreenDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
