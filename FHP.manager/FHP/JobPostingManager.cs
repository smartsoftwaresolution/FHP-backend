using FHP.dtos.FHP;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.FHP
{
    public class JobPostingManager : IJobPostingManager
    {
        private readonly IJobPostingRepoitory _repoitory;

        public JobPostingManager(IJobPostingRepoitory repoitory)
        {
            _repoitory=repoitory;
        }
        public async Task AddAsync(AddJobPostingModel model)
        {
           await _repoitory.AddAsync(JobPostingFactory.Create(model));
        }

        public async Task<string> Edit(AddJobPostingModel model)
        {
           var data = await _repoitory.GetAsync(model.Id);
            if(data.JobStatus == utilities.Constants.JobPosting.Submitted)
            {
                return "job is submitted ,hence can't be updated";
            }
            JobPostingFactory.Update(data,model);
            _repoitory.Edit(data);
            return "updated successfully";
        }



        public async Task<(List<JobPostingDetailDto> jobPosting, int totalCount ,int totalPage )> GetAllAsync(int page, int pageSize, string? search,int userId)
        {
           return  await _repoitory.GetAllAsync(page, pageSize, search,userId);
        }


        public async Task<JobPostingDetailDto> GetByIdAsync(int id)
        {
          return  await _repoitory.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repoitory.DeleteAsync(id);
        }

        public async Task<string> ActiveDeactiveAsync(int jobId)
        {
            return await _repoitory.ActiveDeactiveAsync(jobId);
        }

        public async Task SubmitJobAsync(int jobId)
        {
             await _repoitory.SubmitJobAsync(jobId);
        }

        public async Task CancelJobAsync(int jobId, string cancelReason)
        {
            await _repoitory.CancelJobAsync(jobId, cancelReason);
        }

        public async Task SetJobProcessingStatus(int jobId, Constants.JobProcessingStatus jobProcessingStatus)
        {
            await _repoitory.SetJobProcessingStatus(jobId,jobProcessingStatus);
        }

    }
}
