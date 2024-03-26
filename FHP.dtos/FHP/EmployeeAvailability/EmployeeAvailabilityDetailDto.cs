using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.dtos.FHP.EmployeeAvailability
{
    public class EmployeeAvailabilityDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber {  get; set; }
        public Constants.EmployeeAvailability IsAvailable { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public string AdminjobTitle { get; set; }
        public string AdminJobDescription { get; set; }

       
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string Experience { get; set; }
        public string RolesAndResponsibilities { get; set; }
        public string Skills { get; set; }
        public string Address { get; set; }
        public string Payout { get; set; }
        public string EmploymentType { get; set; }
        public bool InProbationCancel { get; set; }
        public Constants.JobPosting JobStatus { get; set; }
    }
}
