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
        Task Edit(AddEmployeeDetailModel model);
        Task<List<EmployeeDetailDto>> GetAllAsync();
        Task<EmployeeDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
