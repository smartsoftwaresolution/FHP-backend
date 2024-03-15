using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.entity.FHP
{
    public class EmployeeAvailability
    {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int EmployeeId { get; set; }
            public int JobId { get; set; }
            public Constants.EmployeeAvailability IsAvailable { get; set; }
            public Constants.RecordStatus Status { get; set; }
            public DateTime CreatedOn { get; set; }
            public string AdminJobTitle { get; set; }
            public string AdminJobDescription { get; set; }
            public string CancelReasons { get; set; }
            public DateTime? UpdatedOn { get; set; }


    }
}
