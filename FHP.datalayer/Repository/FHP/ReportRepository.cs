using FHP.dtos.FHP.DashBoard;
using FHP.dtos.FHP.EmployeeDetail;
using FHP.infrastructure.Repository.FHP;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.FHP
{

    public class ReportRepository : IReportRepository
    {
        private readonly DataContext _dataContext;

        public ReportRepository(DataContext dataContext)
        {
            _dataContext = dataContext; 
        }


        public async Task<int> GetAllEmployeeCountAsync()
        {
            var r = await _dataContext.UserRole.Where(s => s.RoleName.ToLower().Contains("employee")).Select(s => s.Id).FirstOrDefaultAsync();

            return await _dataContext.User.CountAsync(s => s.RoleId == r && s.Status != Constants.RecordStatus.Deleted);
        }
        
        public async Task<int> GetAllEmployerCountAsync()
        {
           
            var r = await _dataContext.UserRole.Where(s => s.RoleName.ToLower().Contains("employer")).Select(s => s.Id).FirstOrDefaultAsync();
            return await _dataContext.User.CountAsync(s => s.RoleId == r && s.Status != Constants.RecordStatus.Deleted);
        }

        public async Task<int> GetAllTeamCountAsync()
        {
            return await _dataContext.User.CountAsync(s   => s.Status != Constants.RecordStatus.Deleted);
        }

        public async Task<int> GetAllJobCountAsync()
        {
            return await _dataContext.JobPostings.CountAsync(s => s.Status != Constants.RecordStatus.Deleted);
        }

        public async Task<int> GetAllJobCountByUserIdAsync(int userId)
        {
            return await _dataContext.JobPostings.CountAsync(s => s.Status != Constants.RecordStatus.Deleted && s.UserId == userId);
        }

        public async Task<int> GetAllContractCountAsync()
        {
            return await _dataContext.Contracts.CountAsync(s => s.Status != Constants.RecordStatus.Deleted);
        }

        public async Task<DashBoardDto> GetAllCount(string rolename)
        {

            var counts = new DashBoardDto();

            if(rolename.ToLower().Contains("admin"))
            {
                counts.TotalEmployee = await GetAllEmployeeCountAsync();
                counts.TotalEmployer = await GetAllEmployerCountAsync();
                counts.TotalJobPost = await GetAllJobCountAsync();
                counts.TotalUser = await GetAllTeamCountAsync();
                counts.TotalContract = await GetAllContractCountAsync();    
            }

            else if (rolename.ToLower().Contains("employee"))
            {
                counts.TotalEmployee = await GetAllEmployeeCountAsync();
            }

            else if (rolename.ToLower().Contains("employer"))
            {
                counts.TotalEmployer = await GetAllEmployerCountAsync();
            }

            /*else if(rolename != "employee" && rolename != "employer" && rolename != "admin")
            {
                counts.TotalJobPost = await GetAllJobCountAsync();
                counts.TotalUser = await GetAllTeamCountAsync();
                counts.TotalContract = await GetAllContractCountAsync();
            }*/

            return counts;
        }
    }
}

