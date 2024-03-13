using FHP.dtos.FHP;
using FHP.dtos.FHP.EmployeeDetail;
using FHP.models.FHP.EmployeeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployeeDetailManager
    {
        Task AddAsync(AddEmployeeDetailModel model,string profileResume);
        Task Edit(AddEmployeeDetailModel model,string resumeUrl);
        Task<(List<EmployeeDetailDto>employee, int totalCount)> GetAllAsync(int page,int pagesize,int userId, string? search);
        Task<EmployeeDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<string> SetAvailabilityAsync(int id);
        Task<CompleteEmployeeDetailDto> GetAllByIdAsync(int id);

    }
}
