using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.UserManagement
{
    public interface IUserScreenAccessRepository
    {
        Task AddAsync(UserScreenAccess entity);
        void Edit(UserScreenAccess entity);
        Task<UserScreenAccess> GetAsync(int id);
        Task<(List<UserScreenAccessDto> userScreenAccess, int totalCount)> GetAllAsync(int page, int pageSize, int roleId);
        Task<UserScreenAccessDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
