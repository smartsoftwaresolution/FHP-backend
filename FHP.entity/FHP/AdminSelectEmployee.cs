using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FHP.entity.FHP
{
    public class AdminSelectEmployee
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int EmployeeId { get; set; }
        public bool InProbationCancel { get; set; }
        public bool IsSelected { get; set; }
        public Constants.ProcessingStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }

    }

}
