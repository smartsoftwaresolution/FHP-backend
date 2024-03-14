using FHP.utilities;

namespace FHP.entity.FHP
{
    public class JobPosting
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
        public Constants.RecordStatus Status { get; set; }
        public Constants.JobPosting JobStatus { get; set; }
        public string CancelReason { get; set; }
        public Constants.JobProcessingStatus JobProcessingStatus { get; set; }
        public string AdminJobTitle { get; set; }
        public string AdminJobDescription { get; set; }

    }
}
