using FHP.infrastructure.Repository.FHP;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;

namespace FHP.datalayer.Repository.FHP
{
    public class EmployerDashboardRepository : IEmployerDashboardRepository
    {
        private readonly DataContext _dataContext;
        public EmployerDashboardRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

       
        public async Task<int> GetAllJobPost()
        {
          return  await _dataContext.JobPostings.CountAsync();
        }

        public async Task<int> GetAllDraftPost()
        {
            var draft = await _dataContext.JobPostings.CountAsync(s => s.JobStatus == Constants.JobPosting.Draft);
            return draft;
        }

        public async Task<int> GetAllContract(int id)
        {
           return await _dataContext.Contracts.CountAsync(x => x.EmployerId == id);
        }

        public async Task<int> TotalJobReq(int employeeId)
        {
            var totalJobReq = await _dataContext.EmployeeAvailabilities.CountAsync(x => x.IsAvailable == Constants.EmployeeAvailability.Pending && x.EmployeeId == employeeId);
            return totalJobReq;
        }
    }
}
