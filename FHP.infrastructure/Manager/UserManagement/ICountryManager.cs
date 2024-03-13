using FHP.dtos.UserManagement.Country;
using FHP.entity.UserManagement;
using FHP.models.UserManagement.Country;
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
        Task<(List<CountryDetailDto>country,int totalCount)> GetAllAsync(int page,int pageSize,string search);
        Task<CountryDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
