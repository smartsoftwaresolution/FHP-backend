using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.models.FHP
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
        public string Address { get; set; }
        public string Payout { get; set; }
        public bool InProbationCancel { get; set; }
        public Constants.JobPosting JobPosting { get; set; }
    }
}
