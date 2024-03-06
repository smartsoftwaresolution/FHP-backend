using FHP.dtos.FHP;
using FHP.entity.FHP;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IJobPostingRepoitory
    {
        Task AddAsync(JobPosting entity);
        Task<JobPosting> GetAsync(int id);
        void Edit(JobPosting entity);
        Task<(List<JobPostingDetailDto> jobPosting, int totalCount)> GetAllAsync(int page, int pageSize, string? search,int userId);
        Task<JobPostingDetailDto> GetByIdAsync(int id); 
        Task DeleteAsync(int id);
        Task<string> ActiveDeactiveAsync(int jobId);
        Task SubmitJobAsync(int jobId);
        Task CancelJobAsync(int jobId, string cancelReason);
        Task SetJobProcessingStatus(int jobId, Constants.JobProcessingStatus jobProcessingStatus);

    }
}
