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
    
    public class ReportRepository :IReportRepository
    {
        private readonly DataContext _dataContext;

        public ReportRepository(DataContext dataContext)
        {
            _dataContext = dataContext; 
        }

        public async Task<int> GetAllEmployeeAsync(int id)
        {
            var data = await _dataContext.EmployeeDetails.CountAsync(s => s.Status != Constants.RecordStatus.Deleted && s.Id == id);
            return data;
        }
    }
}
