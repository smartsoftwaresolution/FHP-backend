using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP.EmployeeAvailability
{
    public class EmployeeAvailabilityDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public Constants.EmployeeAvailability IsAvailable { get; set; }
        public DateTime CreatedOn { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public string AdminjobTitle { get; set; }
        public string AdminJobDescription { get; set; }
    }
}
