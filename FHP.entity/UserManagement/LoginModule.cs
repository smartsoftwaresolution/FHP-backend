﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.entity.UserManagement
{
    public class LoginModule
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? RoleId { get; set; }
      
        public DateTime CreatedOn { get; set; }
    }
}
