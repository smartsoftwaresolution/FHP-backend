using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.EmployeeAvailability
{
    public class AddEmployeeAvailabilityModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<int> EmployeeId { get; set; }
        public int JobId { get; set; }
        public Constants.EmployeeAvailability IsAvailable { get; set; }
        public string AdminjobTitle { get; set; }
        public string AdminJobDescription { get; set; }
    }
}
