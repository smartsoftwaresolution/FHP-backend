﻿using FHP.dtos.FHP;
using FHP.models.FHP;
using FHP.utilities;
 
namespace FHP.infrastructure.Manager.FHP
{
    public interface IJobPostingManager
    {
        Task AddAsync(AddJobPostingModel model);

        Task<string> Edit(AddJobPostingModel model);
        Task<(List<JobPostingDetailDto> jobPosting, int totalCount)> GetAllAsync(int page, int pageSize, string? search,int userId);

        Task<JobPostingDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<string> ActiveDeactiveAsync(int jobId);
        Task SubmitJobAsync(int jobId);
        Task CancelJobAsync(int jobId, string cancelReason);
        Task SetJobProcessingStatus(int jobId,Constants.JobProcessingStatus jobProcessingStatus);   
    }
}
