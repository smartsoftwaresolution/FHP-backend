using FHP.dtos.UserManagement.Screen;
using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.UserManagement
{
    public interface IScreenRepository
    {
        Task AddAsync(Screen entity);
        Task<Screen> GetAsync(int id);
        void Edit(Screen entity);
        Task<(List<ScreenDetailDto> screen, int totalCount)> GetAllAsync(int page, int pageSize, string search);
        Task<ScreenDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
