﻿using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.UserManagement.Screen
{
    public class ScreenDetailDto
    {
        public int Id { get; set; }
        public string ScreenName { get; set; }
        public string ScreenCode { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
