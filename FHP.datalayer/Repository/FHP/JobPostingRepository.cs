
using FHP.dtos.FHP;
using FHP.dtos.FHP.JobPosting;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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


        public async Task<(List<JobPostingDetailDto> jobPosting, int totalCount)> GetAllAsync(int page, int pageSize, string? search,int userId)
        {
            string rolename = string.Empty;

            if (userId > 0)
            {


                rolename  = await (from s in _dataContext.User
                                   join t in _dataContext.UserRole on s.RoleId equals t.Id
                                   where s.Id == userId
                                   select t.RoleName).FirstOrDefaultAsync();
            }
            var query = from s in _dataContext.JobPostings
                        join u in _dataContext.User on s.UserId equals u.Id
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { jobPosting = s , employer = u};


            if(!string.IsNullOrEmpty(search))
            {
                query =query.Where(s=> s.jobPosting.JobTitle.Contains(search) ||
                                       s.jobPosting.Experience.Contains(search) ||
                                       s.jobPosting.Address.Contains(search) ||
                                       s.jobPosting.Skills.Contains(search));
            }

            
            var totalCount = await query.CountAsync();

            if (rolename.ToLower() != "admin")
            {
                if (userId > 0)
                {
                    query = query.Where(s => s.jobPosting.UserId == userId);
                    totalCount = await query.CountAsync(s => s.jobPosting.Status != Constants.RecordStatus.Deleted && s.jobPosting.UserId == userId);        
                }
            }

            query = query.OrderByDescending(s => s.jobPosting.Id);


            if (page > 0 && pageSize > 0 )
            {
                query =query.Skip((page - 1 ) * pageSize).Take(pageSize);   
            }



            var data = await query.Select(s => new JobPostingDetailDto
<<<<<<< HEAD
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
                                           JobStatus = s.jobPosting.JobStatus
                                  })
                                  .AsNoTracking()
                                  .ToListAsync();
=======
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
                JobStatus = s.jobPosting.JobStatus,
                EmployerName = s.employer.FirstName + " " + s.employer.LastName
            }).AsNoTracking().ToListAsync();
>>>>>>> 019f144ebf3a78f43d62b9b5d37e4530e25a4562

            return (data, totalCount);
        }



        public async Task<JobPostingDetailDto> GetByIdAsync(int id)
        {
            return  await (from s in _dataContext.JobPostings
                           join e in _dataContext.User on s.UserId equals e.Id
                          where s.Status != Constants.RecordStatus.Deleted
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
                              JobStatus = s.JobStatus,
                              EmployerName = e.FirstName + " " + e.LastName

                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.JobPostings.Where(s=>s.Id ==id).FirstOrDefaultAsync();
            data.Status=Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<string> ActiveDeactiveAsync(int jobId)
        {
            string result = string.Empty;
            var data = await _dataContext.JobPostings.Where(s => s.Id == jobId).FirstOrDefaultAsync();
            if(data.Status == Constants.RecordStatus.Active)
            {
                data.Status = Constants.RecordStatus.Inactive;
                result = "DeActivated";
            }

            else
            {
                data.Status = Constants.RecordStatus.Active;
                result = "Activated";
            }

            _dataContext.JobPostings.Update(data);
            await _dataContext.SaveChangesAsync();
            return result;
        }

        public async Task SubmitJobAsync(int jobId)
        {
            var data = await _dataContext.JobPostings.Where(s => s.Id == jobId).FirstOrDefaultAsync();
            data.JobStatus = Constants.JobPosting.Submitted;
            _dataContext.JobPostings.Update(data);
            await _dataContext.SaveChangesAsync();
        }
        public async Task CancelJobAsync(int jobId, string cancelReason)
        {
            var data = await _dataContext.JobPostings.Where(s => s.Id == jobId).FirstOrDefaultAsync();
            data.CancelReason = cancelReason;
            _dataContext.JobPostings.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task SetJobProcessingStatus(int jobId, Constants.JobProcessingStatus jobProcessingStatus)
        {
            var data = await _dataContext.JobPostings.Where(s => s.Id == jobId).FirstOrDefaultAsync();
            data.JobProcessingStatus = jobProcessingStatus;
            _dataContext.JobPostings.Update(data);
            await _dataContext.SaveChangesAsync();
        }


        

    }
}
