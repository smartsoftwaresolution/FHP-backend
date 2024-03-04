﻿using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP
{
    public class EmployerContractConfirmationDetailDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public int EmployerId { get; set; }
        public bool IsSelected { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
