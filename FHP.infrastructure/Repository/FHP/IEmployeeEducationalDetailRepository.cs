using FHP.dtos.FHP;
using FHP.dtos.FHP.EmployeeEducationalDetail;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployeeEducationalDetailRepository
    {
        Task AddAsync(EmployeeEducationalDetail entity);
        Task<EmployeeEducationalDetail> GetAsync(int id);
        void Edit(EmployeeEducationalDetail entity);
        Task<(List<EmployeeEducationalDetailDto> employeeeducationaldetail, int totalCount)> GetAllAsync(int page, int pageSize,int userId, string? search);
        Task<EmployeeEducationalDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
