using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployerDashboardRepository
    {
        Task<int> GetAllJobPost();
        Task<int> GetAllDraftPost();
        Task<int> GetAllContract(int id);
        Task<int> TotalJobReq(int employeeId);
    }
}
