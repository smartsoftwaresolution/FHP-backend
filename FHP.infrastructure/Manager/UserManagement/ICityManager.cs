using FHP.dtos.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface ICityManager
    {
        Task AddAsync(AddCityModel model);
        Task Edit(AddCityModel model);
        Task<List<CityDetailDto>> GetAllAsync();
        Task<CityDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);   
    }
}
