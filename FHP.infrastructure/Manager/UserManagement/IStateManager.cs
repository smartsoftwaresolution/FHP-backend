using FHP.dtos.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IStateManager
    {
        Task AddAsync(AddStateModel model);
        Task Edit(AddStateModel model);
        Task<List<StateDetailDto>> GetAllAsync();
        Task<StateDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);   
    }
}
