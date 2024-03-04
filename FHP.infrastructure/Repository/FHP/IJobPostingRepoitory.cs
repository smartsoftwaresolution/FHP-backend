using FHP.dtos.FHP;
using FHP.entity.FHP;
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
        Task<(List<JobPostingDetailDto> jobPosting, int totalCount,int totalPage)> GetAllAsync(int page, int pageSize, string? search,int userId);
        Task<JobPostingDetailDto> GetByIdAsync(int id); 
        Task DeleteAsync(int id);
        Task<string> ActiveDeactiveAsync(int jobId);

    }
}
