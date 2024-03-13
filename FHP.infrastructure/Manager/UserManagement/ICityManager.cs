using DocumentFormat.OpenXml.Wordprocessing;
using FHP.dtos.UserManagement.City;
using FHP.models.UserManagement.City;
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
        Task<(List<CityDetailDto>city,int totalCount)> GetAllAsync(int page,int pageSize,string? search);
        Task<CityDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<List<CityDetailDto>> GetByStateIdAsync(int stateId);

    }
}
