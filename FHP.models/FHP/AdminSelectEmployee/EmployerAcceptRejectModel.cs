using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.AdminSelectEmployee
{
    public class EmployerAcceptRejectModel
    {
        public int JobId { get; set; }
        public int EmployeeId { get; set; }
        public Constants.AcceptRejectStatus IsSelected { get; set; }
    }
}
