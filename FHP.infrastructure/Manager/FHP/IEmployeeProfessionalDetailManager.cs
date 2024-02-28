using FHP.dtos.FHP;
using FHP.models.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployeeProfessionalDetailManager
    {
        Task AddAsync(AddEmployeeProfessionalDetailModel model);
        Task Edit(AddEmployeeProfessionalDetailModel model);
        Task<(List<EmployeeProfessionalDetailDto> employeeProfessionalDetail, int totalCount)> GetAllAsync(int page, int pageSize,int userId, string? search);
        Task<EmployeeProfessionalDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
