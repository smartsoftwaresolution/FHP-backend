using FHP.dtos.UserManagement;
using FHP.models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface ICompanyManager
    {
        Task AddAsync(AddCompanyModel model);
        Task EditAsync(AddCompanyModel model);
        Task<List<CompanyDetailDto>> GetAllAsync(int id);
        Task<CompanyDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
