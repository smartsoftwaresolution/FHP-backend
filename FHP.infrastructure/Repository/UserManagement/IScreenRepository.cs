using FHP.dtos.UserManagement;
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
        Task<List<ScreenDetailDto>> GetAllAsync();
        Task<ScreenDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
