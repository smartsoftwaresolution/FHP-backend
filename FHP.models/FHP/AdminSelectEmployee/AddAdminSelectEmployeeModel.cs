using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.AdminSelectEmployee
{
    public class AddAdminSelectEmployeeModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int EmployeeId { get; set; }
        public bool InProbationCancel { get; set; }
        public bool IsSelected { get; set; }

    }
}
