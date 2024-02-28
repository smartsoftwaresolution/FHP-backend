using FHP.dtos.FHP;
using FHP.models.FHP;
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
        Task Edit(AddJobPostingModel model);
        Task<(List<JobPostingDetailDto> jobPosting, int totalCount)> GetAllAsync(int page, int pageSize, string? search);
        Task<JobPostingDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
