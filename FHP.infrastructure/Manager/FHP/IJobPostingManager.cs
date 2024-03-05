using FHP.dtos.FHP;
using FHP.models.FHP;
using FHP.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IJobPostingManager
    {
        Task AddAsync(AddJobPostingModel model);
<<<<<<< HEAD
        Task Edit(AddJobPostingModel model);
        Task<(List<JobPostingDetailDto> jobPosting, int totalCount)> GetAllAsync(int page, int pageSize, string? search,int userId);
=======
        Task<string> Edit(AddJobPostingModel model);
        Task<(List<JobPostingDetailDto> jobPosting, int totalCount,int totalPage)> GetAllAsync(int page, int pageSize, string? search,int userId);
>>>>>>> 7118752b95599043a9b9ea4fa4c115301f16bb2e
        Task<JobPostingDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<string> ActiveDeactiveAsync(int jobId);
        Task SubmitJobAsync(int jobId);
        Task CancelJobAsync(int jobId, string cancelReason);
        Task SetJobProcessingStatus(int jobId,Constants.JobProcessingStatus jobProcessingStatus);   
    }
}
