using FHP.dtos.FHP;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP;
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

        public async Task Edit(AddJobPostingModel model)
        {
           var data = await _repoitory.GetAsync(model.Id);
            JobPostingFactory.Update(data,model);
            _repoitory.Edit(data);
        }



        public async Task<(List<JobPostingDetailDto> jobPosting, int totalCount )> GetAllAsync(int page, int pageSize, string? search,int userId)
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

    }
}
