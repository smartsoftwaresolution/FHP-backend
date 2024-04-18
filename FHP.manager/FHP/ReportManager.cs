using FHP.datalayer.Repository.FHP;
using FHP.dtos.FHP.EmployeeDetail;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.FHP
{
    public class ReportManager : IReportManager
    {
        private readonly IReportRepository _reportRepository;

        public ReportManager(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;

        }

        public async Task<int> GetAllEmployeeCountAsync()
        {
            return await _reportRepository.GetAllEmployeeCountAsync();
        }


    }
}
