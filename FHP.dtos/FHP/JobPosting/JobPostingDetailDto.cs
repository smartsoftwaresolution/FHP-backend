﻿using FHP.dtos.FHP.JobSkillDetail;
using FHP.entity.FHP;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP.JobPosting
{
    public class JobPostingDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string Experience { get; set; }
        public string RolesAndResponsibilities { get; set; }
        public DateTime ContractDuration { get; set; }
        public DateTime? ContractStartTime { get; set; }
        public string Skills { get; set; }
        public string Address { get; set; }
        public string Payout { get; set; }
        public bool InProbationCancel { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Constants.JobPosting JobStatus { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public int EmployeeId { get; set; }
        public string EmployerName { get; set; }
        public string EmploymentType { get; set; }
        public Constants.JobProcessingStatus JobProcessingStatus { get; set; }
        public List<JobSkillDetailDto>? JobSkillDetails { get; set; }
    }
}
