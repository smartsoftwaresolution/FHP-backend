using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.EmployeeAvailability
{
    public class SetEmployeeAvailabilityModel
    {
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public Constants.EmployeeAvailability EmployeeAvailability { get; set; }
        public string CancelReason { get; set; }
    }
}
