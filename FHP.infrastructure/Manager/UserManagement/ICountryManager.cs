using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface ICountryManager
    {
        Task AddAsync(AddCountryModel model);
        Task Edit(AddCountryModel model);
        Task<List<CountryDetailDto>> GetAllAsync();
        Task<CountryDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
