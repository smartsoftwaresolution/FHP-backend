using FHP.dtos.FHP.EmployeeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public  interface IReportManager
    {
        Task<int> GetAllEmployeeAsync(int id);
    }
}
