
using FHP.dtos.FHP;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;

namespace FHP.datalayer.Repository.FHP
{
    public class EmployeeAvailabilityRepository : IEmployeeAvailabilityRepository
    {
        private readonly DataContext _dataContext;

        public EmployeeAvailabilityRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }

        public async Task AddAsync(EmployeeAvailability entity)
        {
            await _dataContext.EmployeeAvailabilities.AddAsync(entity); 
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(EmployeeAvailability entity)
        {
           _dataContext.EmployeeAvailabilities.Update(entity);
            _dataContext.SaveChanges();
        }


        public async Task<EmployeeAvailability> GetAsync(int id)
        {
            return await _dataContext.EmployeeAvailabilities.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(List<EmployeeAvailabilityDetailDto> employeeAval, int totalCount)> GetAllAsync(int page, int pageSize, string? search)
        {
            var query = from s in _dataContext.EmployeeAvailabilities
                        where s.Status != utilities.Constants.RecordStatus.Deleted
                        select new { employeeAval = s};

            


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.employeeAval.JobId.ToString().Contains(search) ||
                                       s.employeeAval.EmployeeId.ToString().Contains(search));
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(s => s.employeeAval.Id);

            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }


            var data = await query.Select(s => new EmployeeAvailabilityDetailDto
            {
                Id= s.employeeAval.Id,
                UserId= s.employeeAval.UserId,
                JobId= s.employeeAval.JobId,    
                EmployeeId= s.employeeAval.EmployeeId,
                IsAvailable= s.employeeAval.IsAvailable,
                CreatedOn = s.employeeAval.CreatedOn,
                Status= s.employeeAval.Status,
            }).ToListAsync();

            return (data, totalCount);
        }

        public async Task<EmployeeAvailabilityDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.EmployeeAvailabilities
                          join j in _dataContext.JobPostings on s.JobId equals j.Id
                          where s.Status != Constants.RecordStatus.Deleted &&
                          s.Id == id

                          select new EmployeeAvailabilityDetailDto
                          {
                              Id=s.Id,
                              UserId=s.UserId,
                              JobId=s.JobId,
                              EmployeeId=s.EmployeeId,
                              IsAvailable=s.IsAvailable,
                              CreatedOn=s.CreatedOn,
                             Status=s.Status,
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<EmployeeAvailabilityDetailDto>> GetAllAvalibility(int JobId)
        {
            return await (from s in _dataContext.EmployeeAvailabilities
                          where s.Status != Constants.RecordStatus.Deleted && 
                          s.JobId == JobId && (s.IsAvailable == true)

                          select new EmployeeAvailabilityDetailDto
                          {
                              Id = s.Id,
                              UserId = s.UserId,
                              JobId = s.JobId,
                              EmployeeId = s.EmployeeId,
                              IsAvailable = s.IsAvailable,
                              CreatedOn = s.CreatedOn,
                              Status = s.Status,
                          }).AsNoTracking().ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.EmployeeAvailabilities.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = utilities.Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
