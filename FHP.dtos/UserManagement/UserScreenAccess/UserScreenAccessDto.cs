using FHP.entity.UserManagement;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.UserManagement.UserScreenAccess
{
    public class UserScreenAccessDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int ScreenId { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserRoleName { get; set; }
        public string ScreenName { get; set; }

    }
}
