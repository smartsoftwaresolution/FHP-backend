﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP.AdminSelectEmployee
{
    public class AdminSelectEmployeeDetailDto
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool InProbationCancel { get; set; }
        public bool IsSelected { get; set; }
        public string ProfileImg { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Mobilenumber { get; set; }
        public string Email { get; set; }

    }
}
