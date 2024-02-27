using FHP.dtos.FHP;
using FHP.models.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployerDetailManager
    {
        Task AddAsync(AddEmployerDetailModel model);
        Task Edit(AddEmployerDetailModel model);
        Task<(List<EmployerDetailDetailDto> employerDetail, int totalCount)> GetAllAsync(int page, int pageSize, string? search);
        Task<EmployerDetailDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
