using FHP.dtos.FHP.EmployeeAvailability;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.EmployeeAvailability;
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

        public async Task AddAsync(AddEmployeeAvailabilityModel model)
        {
            var employeeAvailability = model.EmployeeId.Select(employeeId => new EmployeeAvailability
            {
                UserId = model.UserId,
                JobId = model.JobId,
                EmployeeId = employeeId,
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active,
                AdminJobTitle = model.AdminjobTitle,
                AdminJobDescription = model.AdminJobDescription,
            }).ToList();

            await _dataContext.EmployeeAvailabilities.AddRangeAsync(employeeAvailability);
            await _dataContext.SaveChangesAsync();

           /* var data = await _dataContext.JobPostings.Where(s => s.Id == model.JobId).FirstOrDefaultAsync();
            data.AdminJobTitle = model.AdminjobTitle;
            data.AdminJobDescription = model.AdminJobDescription;
            _dataContext.JobPostings.Update(data);
            await _dataContext.SaveChangesAsync();*/
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

        public async Task<(List<EmployeeAvailabilityDetailDto>  employeeAval, int totalCount)> GetAllAsync(int page, int pageSize, string? search,int employeeId, Constants.EmployeeAvailability? employeeAvailability)
        {
            var query = from s in _dataContext.EmployeeAvailabilities
                        join t in _dataContext.User on s.EmployeeId equals t.Id

                        where s.Status != Constants.RecordStatus.Deleted 
                        select new { employeeAval = s, UserDetail = t};

            
            if(employeeId > 0)
            {
                query = query.Where(s => s.employeeAval.EmployeeId == employeeId);
            }

            if(employeeAvailability != null)
            {
                query = query.Where(s => s.employeeAval.IsAvailable == employeeAvailability);
            }
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
                FirstName = s.UserDetail.FirstName,
                LastName= s.UserDetail.LastName,
                Email = s.UserDetail.Email,
                MobileNumber = s.UserDetail.MobileNumber,
                IsAvailable= s.employeeAval.IsAvailable,
                CreatedOn = s.employeeAval.CreatedOn,
                Status= s.employeeAval.Status,
                AdminjobTitle = s.employeeAval.AdminJobTitle,
                AdminJobDescription = s.employeeAval.AdminJobDescription,
                UpdatedOn = s.employeeAval.UpdatedOn,
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
                             AdminJobDescription = s.AdminJobDescription,
                             AdminjobTitle = s.AdminJobTitle
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<EmployeeAvailabilityDetailDto>> GetAllAvalibility(int JobId, Constants.EmployeeAvailability? employeeAvailability)
        {
            return await (from s in _dataContext.EmployeeAvailabilities
                          join u in _dataContext.User on s.EmployeeId equals u.Id
                          where s.Status != Constants.RecordStatus.Deleted && 
                          s.JobId == JobId && (s.IsAvailable == employeeAvailability || employeeAvailability == null)

                          select new EmployeeAvailabilityDetailDto
                          {
                              Id = s.Id,
                              UserId = s.UserId,
                              JobId = s.JobId,
                              EmployeeId = s.EmployeeId,
                              IsAvailable = s.IsAvailable,
                              CreatedOn = s.CreatedOn,
                              Status = s.Status,
                              AdminjobTitle = s.AdminJobTitle,
                              AdminJobDescription = s.AdminJobDescription,
                              UpdatedOn = s.UpdatedOn,

                              FirstName = u.FirstName,
                              LastName = u.LastName,
                              Email = u.Email,
                              MobileNumber = u.MobileNumber,
                              FullName = u.FirstName + " " + u.LastName,
                          }).AsNoTracking().ToListAsync();
        }

        public async Task<List<EmployeeAvailabilityDetailDto>> GetByEmployeeIdAsync(int employeeId)
        {
            return await (from s in _dataContext.EmployeeAvailabilities
                          where s.Status != Constants.RecordStatus.Deleted &&
                          s.EmployeeId == employeeId

                          select new EmployeeAvailabilityDetailDto
                          {
                              Id = s.Id,
                              UserId = s.UserId,
                              JobId = s.JobId,
                              EmployeeId = s.EmployeeId,
                              IsAvailable = s.IsAvailable,
                              CreatedOn = s.CreatedOn,
                              Status = s.Status,
                              AdminjobTitle = s.AdminJobTitle,
                              AdminJobDescription = s.AdminJobDescription,
                              UpdatedOn = s.UpdatedOn,
                              
                          }).AsNoTracking().ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.EmployeeAvailabilities.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = utilities.Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<string> SetEmployeeAvalibility(SetEmployeeAvailabilityModel model)
        {
            string result = string.Empty;

            var data = await _dataContext.EmployeeAvailabilities.Where(s=> s.EmployeeId == model.EmployeeId && s.JobId == model.JobId).FirstOrDefaultAsync();

            if(model.EmployeeAvailability == Constants.EmployeeAvailability.NotAvailable)
            {
                data.IsAvailable = Constants.EmployeeAvailability.NotAvailable;
                data.CancelReasons = model.CancelReason;
                data.UpdatedOn = Utility.GetDateTime();
                result = "UnAvailable";
            }
            else 
            {
                data.IsAvailable = Constants.EmployeeAvailability.Available;
                data.CancelReasons = model.CancelReason;
                data.UpdatedOn = Utility.GetDateTime();
                result = "Available";
            }
            _dataContext.EmployeeAvailabilities.Update(data);
            await _dataContext.SaveChangesAsync();
            return result;
        }
    }
}
