using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.AdminSelectEmployee
{
    public class SetAdminSelectEmployeeModel
    {
        public int JobId { get; set; }
        public int EmployeeId { get; set; } 
        public Constants.ProcessingStatus ProcessingStatus { get; set; }
    }
}
