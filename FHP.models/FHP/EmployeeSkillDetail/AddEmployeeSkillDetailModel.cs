﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP.EmployeeSkillDetail
{
    public class AddEmployeeSkillDetailModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<int> SkillId { get; set; }
    }
}
