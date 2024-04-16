using FHP.dtos.FHP.JobPosting;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.JobPosting;
using FHP.utilities;

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
            if(data.JobStatus == Constants.JobPosting.Submitted)
            {
                return "job is submitted ,hence can't be updated";
            }

            if(data.JobStatus != Constants.JobPosting.Draft)
            {
                return "updated Successfully";
            }

            JobPostingFactory.Update(data,model);
            data.JobStatus = Constants.JobPosting.Submitted;
            _repoitory.Edit(data);
            return "updated successfully";
        }



        public async Task<(List<JobPostingDetailDto> jobPosting, int totalCount )> GetAllAsync(int page, int pageSize, string? search,int userId, bool? IsAdmin)
        {
           return  await _repoitory.GetAllAsync(page, pageSize, search,userId,IsAdmin);
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
