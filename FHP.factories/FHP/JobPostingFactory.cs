using FHP.entity.FHP;
using FHP.models.FHP.JobPosting;
using FHP.utilities;


namespace FHP.factories.FHP
{
    public class JobPostingFactory
    {
        public static JobPosting Create(AddJobPostingModel model)
        {
            var data = new JobPosting
            {
                UserId= model.UserId,
                JobTitle= model.JobTitle,
                Description=model.Description,
                Experience=model.Experience,
                RolesAndResponsibilities=model.RolesAndResponsibilities,
                ContractDuration=Utility.GetDateTime(),
                ContractStartTime=Utility.GetDateTime(),
                Skills=model.Skills,
                Address=model.Address,
                Payout=model.Payout,
                InProbationCancel=model.InProbationCancel,
                CreatedOn=Utility.GetDateTime(),
                Status=Constants.RecordStatus.Active,
                JobStatus = model.JobPosting,
                EmploymentType = model.EmploymentType,
                CancelReason = "",
                JobProcessingStatus = Constants.JobProcessingStatus.InProcess,

                JobSkillDetails = model.SkillId?.Select(s => new JobSkillDetail
                {
                    SkillId = s,
                    Status = Constants.RecordStatus.Active,
                    CreatedOn = Utility.GetDateTime()
                }).ToList(),
            };

            return data;
        }

        public static void Update(JobPosting entity,AddJobPostingModel model)
        {
            entity.UserId = model.UserId;
            entity.JobTitle = model.JobTitle;
            entity.Description = model.Description;
            entity.Experience = model.Experience;
            entity.RolesAndResponsibilities = model.RolesAndResponsibilities;
            entity.ContractDuration = Utility.GetDateTime();
            entity.ContractStartTime = Utility.GetDateTime();
            entity.Skills = model.Skills;
            entity.Address = model.Address;
            entity.Payout = model.Payout;
            entity.EmploymentType = model.EmploymentType;
            entity.InProbationCancel = model.InProbationCancel;
            entity.UpdatedOn=Utility.GetDateTime();
        }
    }
}
