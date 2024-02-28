using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP
{
    public class EmployeeSkillDetailDto
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int SkillId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
