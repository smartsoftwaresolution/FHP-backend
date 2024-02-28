using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.UserManagement
{
    public interface  IStateRepository
    {
        Task AddAsync(State entity);
        Task<State> GetAsync(int id);
        void Edit(State entity);
        Task<(List<StateDetailDto> state,int totalCount)> GetAllAsync(int page ,int pageSize, string search);
        Task<StateDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<List<StateDetailDto>> GetByCountryIdAsync(int countryId);

    }
}
