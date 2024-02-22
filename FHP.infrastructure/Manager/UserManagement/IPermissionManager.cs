using FHP.dtos.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IPermissionManager
    {
        Task AddAsync(AddPermissionModel model, int companyId);
        Task EditAsync(AddPermissionModel model, int companyId);
        Task<List<PermissionDetailDto>> GetAllAsync(int companyId);
        Task<PermissionDetailDto> GetByIdAsync(int id,int companyId);
        Task DeleteAsync(int id, int companyId);
    }
}
