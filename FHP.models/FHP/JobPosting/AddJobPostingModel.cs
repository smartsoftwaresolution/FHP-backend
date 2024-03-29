﻿using FHP.utilities;

namespace FHP.models.FHP.JobPosting
{
    public class AddJobPostingModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string Experience { get; set; }
        public string RolesAndResponsibilities { get; set; }
        public string Skills { get; set; }
        public List<int> SkillId { get; set; }
        public string Address { get; set; }
        public string Payout { get; set; }

        public string EmploymentType { get; set; }
        public bool InProbationCancel { get; set; }
        public Constants.JobPosting JobPosting { get; set; }
    }
}
