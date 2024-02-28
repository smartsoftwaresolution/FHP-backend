using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.entity.UserManagement
{
    public class UserRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set;}
        public DateTime? UpdatedOn { get; set; }
        
    }
}
