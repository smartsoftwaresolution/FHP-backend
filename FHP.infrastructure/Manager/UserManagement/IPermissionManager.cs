using FHP.dtos.UserManagement.Permission;
using FHP.models.UserManagement.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IPermissionManager
    {
        Task AddAsync(AddPermissionModel model);
        Task EditAsync(AddPermissionModel model);
        Task<(List<PermissionDetailDto> permission, int totalCount)> GetAllAsync(int page ,int pageSize,string search);
        Task<PermissionDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
