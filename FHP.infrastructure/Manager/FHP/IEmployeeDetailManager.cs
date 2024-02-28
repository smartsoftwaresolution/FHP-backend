using FHP.dtos.FHP;
using FHP.models.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployeeDetailManager
    {
        Task AddAsync(AddEmployeeDetailModel model);
        Task Edit(AddEmployeeDetailModel model,string resumeUrl);
        Task<(List<EmployeeDetailDto>employee, int totalCount)> GetAllAsync(int page,int pagesize,int userId, string? search);
        Task<EmployeeDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
