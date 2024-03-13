using FHP.dtos.FHP;
using FHP.dtos.FHP.EmployerDetail;
using FHP.entity.FHP;
using FHP.models.FHP;
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
        Task<(List<EmployerDetailDto> employerDetail, int totalCount)> GetAllAsync(int page, int pageSize,int userId, string? search);
        Task<EmployerDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
