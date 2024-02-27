using FHP.dtos.FHP;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployerDetailRepository
    {
        Task AddAsync(EmployerDetail entity);
        void Edit(EmployerDetail entity);
        Task<EmployerDetail> GetAsync(int id);
        Task<(List<EmployerDetailDetailDto> employerDetail, int totalCount)> GetAllAsync(int page, int pageSize, string? search);
        Task<EmployerDetailDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
