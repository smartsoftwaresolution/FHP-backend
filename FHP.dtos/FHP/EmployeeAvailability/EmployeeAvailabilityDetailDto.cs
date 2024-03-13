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
        public bool IsAvailable { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
