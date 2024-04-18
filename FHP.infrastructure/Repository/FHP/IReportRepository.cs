using FHP.dtos.FHP.EmployeeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    
    public interface IReportRepository
    {
        Task<int> GetAllEmployeeCountAsync();
        Task<int> GetAllEmployerCountAsync();
        Task<int> GetAllTeamCountAsync();
        Task<int> GetAllJobCountAsync();
        Task<int> GetAllJobCountByUserIdAsync(int userId);
        Task<int> GetAllContractCountAsync();
    }
}
