using FHP.entity.FHP;
using FHP.models.FHP;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Status=Constants.RecordStatus.Created,
                JobStatus = model.JobPosting,
                CancelReason = "",
                JobProcessingStatus = Constants.JobProcessingStatus.None,
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
            entity.InProbationCancel = model.InProbationCancel;
            entity.UpdatedOn=Utility.GetDateTime();
        }
    }
}
