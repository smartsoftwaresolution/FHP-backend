using FHP.dtos.FHP;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.FHP
{
    public class JobPostingRepository : IJobPostingRepoitory
    {
        private readonly DataContext _dataContext;

        public JobPostingRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }
        public async Task AddAsync(JobPosting entity)
        {
           await _dataContext.JobPostings.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(JobPosting entity)
        {
            _dataContext.JobPostings.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<JobPosting> GetAsync(int id)
        {
           return await _dataContext.JobPostings.Where(s => s.Id == id).FirstOrDefaultAsync();
        }


        public async Task<(List<JobPostingDetailDto> jobPosting, int totalCount)> GetAllAsync(int page, int pageSize, string? search)
        {
            var query = from s in _dataContext.JobPostings
                        where s.Status != utilities.Constants.RecordStatus.Deleted
                        select new { jobPosting = s };

            var totalCount = await query.CountAsync(s => s.jobPosting.Status != utilities.Constants.RecordStatus.Deleted);

            if(!string.IsNullOrEmpty(search))
            {
                query =query.Where(s=> s.jobPosting.JobTitle.Contains(search) ||
                                       s.jobPosting.Experience.Contains(search) ||
                                       s.jobPosting.Address.Contains(search) ||
                                       s.jobPosting.Skills.Contains(search));
            }

            if(page > 0 && pageSize > 0)
            {
                query =query.Skip((page -1 )*pageSize).Take(pageSize);   
            }


            query = query.OrderByDescending(s => s.jobPosting.Id);

            var data = await query.Select(s => new JobPostingDetailDto
            {
                Id = s.jobPosting.Id,
                UserId = s.jobPosting.UserId,
                JobTitle = s.jobPosting.JobTitle,
                Description = s.jobPosting.Description,
                Experience = s.jobPosting.Experience,
                RolesAndResponsibilities = s.jobPosting.RolesAndResponsibilities,
                ContractDuration = s.jobPosting.ContractDuration,
                ContractStartTime = s.jobPosting.ContractStartTime,
                Skills = s.jobPosting.Skills,
                Address = s.jobPosting.Address,
                Payout = s.jobPosting.Payout,
                InProbationCancel = s.jobPosting.InProbationCancel,
                CreatedOn = s.jobPosting.CreatedOn,
                UpdatedOn = s.jobPosting.UpdatedOn,
                Status = s.jobPosting.Status,
            }).AsNoTracking().ToListAsync();

            return (data, totalCount);
        }



        public async Task<JobPostingDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.JobPostings
                          where s.Status != utilities.Constants.RecordStatus.Deleted
                          && s.Id == id
                          select new JobPostingDetailDto
                          {
                              Id=s.Id,
                              UserId=s.UserId,
                              JobTitle=s.JobTitle,
                              Description=s.Description,
                              Experience=s.Experience,
                              RolesAndResponsibilities=s.RolesAndResponsibilities,
                              ContractDuration=s.ContractDuration,
                              ContractStartTime=s.ContractStartTime,
                              Skills=s.Skills,
                              Address=s.Address,
                              Payout=s.Payout,
                              InProbationCancel=s.InProbationCancel,
                              CreatedOn=s.CreatedOn,
                              UpdatedOn=s.UpdatedOn,
                              Status=s.Status,
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.JobPostings.Where(s=>s.Id ==id).FirstOrDefaultAsync();
            data.Status=utilities.Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        
    }
}
