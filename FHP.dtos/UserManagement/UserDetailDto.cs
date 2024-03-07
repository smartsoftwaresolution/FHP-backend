﻿
using FHP.dtos.FHP;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.UserManagement
{
    public class UserDetailDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? GovernmentId { get; set; }
        public string RoleName { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public bool? IsVerify { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public List<EmployeeDetailDto>? EmployeeDetails { get; set; }
        public string ProfileImg { get; set; }
        public string MobileNumber { get; set; }
        public bool? IsVerifyByAdmin { get; set; }
        public int? Otp { get; set; }
    }
}
