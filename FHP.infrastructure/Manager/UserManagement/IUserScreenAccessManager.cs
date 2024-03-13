using FHP.dtos.UserManagement.UserScreenAccess;
using FHP.models.UserManagement.UserScreenAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IUserScreenAccessManager
    {
        Task AddAsync(AddUserScreenAccessModel model);
        Task Edit(AddUserScreenAccessModel model);
        Task<(List<UserScreenAccessDto> userScreenAccess, int totalCount)> GetAllAsync(int page, int pageSize, int roleId);
        Task<UserScreenAccessDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
