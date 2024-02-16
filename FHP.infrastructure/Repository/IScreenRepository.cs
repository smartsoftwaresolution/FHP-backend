using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository
{
    public  interface IScreenRepository
    {
        Task AddAsync(Screen entity);
        Task<Screen> GetAsync(int id);
        void Edit(Screen entity);
        Task<List<ScreenDetailDto>> GetAllAsync(int CompanyId);
        Task<ScreenDetailDto> GetByIdAsync(int id,int CompanyId);
        Task DeleteAsync(int id, int CompanyId);
    }
}
