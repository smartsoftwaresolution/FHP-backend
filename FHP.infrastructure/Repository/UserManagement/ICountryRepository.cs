using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.UserManagement
{
    public interface ICountryRepository
    {
        Task AddAsync(Country entity);
        Task<Country> GetAsync(int id);
        void Edit(Country entity);
        Task<(List<CountryDetailDto> country,int totalCount)> GetAllAsync(int page,int pageSize,string? search);
        Task<CountryDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
