using FHP.dtos.FHP.AdminSelectEmployee;
using FHP.dtos.UserManagement.User;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.AdminSelectEmployee;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;

namespace FHP.datalayer.Repository.FHP
{
    public class AdminSelectEmployeeRepository : IAdminSelectEmployeeRepository
    {
        private readonly DataContext _dataContext;

        public AdminSelectEmployeeRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }

        public async Task AddAsync(AdminSelectEmployee entity)
        {
            await _dataContext.AdminSelectEmployees.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }


        public async Task AddAsync(AddAdminSelectEmployeeModel entity)
        {
            var adminSelectEmployee = entity.EmployeeId.Select(employeeId => new AdminSelectEmployee
            {
                JobId = entity.JobId,
                EmployeeId = employeeId,
                InProbationCancel = entity.InProbationCancel,
                IsSelected = entity.IsSelected,
                CreatedOn = Utility.GetDateTime(),
            }).ToList();

            await _dataContext.AdminSelectEmployees.AddRangeAsync(adminSelectEmployee);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(AdminSelectEmployee entity)
        {
            _dataContext.AdminSelectEmployees.Update(entity);   
            _dataContext.SaveChanges();
        }

        public async Task<AdminSelectEmployee> GetAsync(int id)
        {
            return await _dataContext.AdminSelectEmployees.Where(s => s.Id == id).FirstOrDefaultAsync(); 
        }


        public async Task<(List<AdminSelectEmployeeDetailDto> adminSelect, int totalCount)> GetAllAsync(int page, int pageSize,int jobId, string? search)
        {
            var query = from s in _dataContext.AdminSelectEmployees
                        join e in _dataContext.User on s.EmployeeId equals e.Id
                        select new { adminSelect = s ,employee = e};

            

            if(jobId > 0 )
            {
                query = query.Where(s=> s.adminSelect.JobId == jobId);
            }
             
           
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(s => s.adminSelect.JobId.ToString().Contains(search) ||
                                           s.adminSelect.EmployeeId.ToString().Contains(search) ||
                                           s.adminSelect.InProbationCancel.ToString().Contains(search));
                }
            
            

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(s => s.adminSelect.Id);

            if (page > 0 && pageSize > 0)
            {
                query =query.Skip((page - 1) * pageSize).Take(pageSize);    
            }


            var data = await query.Select(s => new AdminSelectEmployeeDetailDto
                                                         {
                                                            Id = s.adminSelect.Id,
                                                            JobId = s.adminSelect.JobId,
                                                            EmployeeId = s.adminSelect.EmployeeId,
                                                            EmployeeName = s.employee.FirstName + " " + s.employee.LastName,
                                                            InProbationCancel = s.adminSelect.InProbationCancel,
                                                            IsSelected = s.adminSelect.IsSelected,
                                                         }) 
                                                         .AsNoTracking()
                                                         .ToListAsync();


            return (data,totalCount);
        }

        public async Task<AdminSelectEmployeeDetailDto> GetByIdAsync(int id)
        {
          return  await (from s in _dataContext.AdminSelectEmployees
                         join u in _dataContext.User on s.EmployeeId equals u.Id
                   where  s.Id == id

                   select new AdminSelectEmployeeDetailDto
                   {
                       Id=s.Id,
                       JobId=s.JobId,
                       EmployeeId=s.EmployeeId,
                       InProbationCancel=s.InProbationCancel,
                       IsSelected=s.IsSelected,
                       EmployeeName = u.FirstName + " " + u.LastName
                   }).AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.AdminSelectEmployees.Where(s => s.Id == id).FirstOrDefaultAsync();
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<(List<UserDetailDto> adminSelect, int totalCount)> GetAllJobEmployeeAsync(int jobId)
        {
            var query = from s in _dataContext.AdminSelectEmployees
                        join t in _dataContext.User on s.EmployeeId equals t.Id
                        join j in _dataContext.JobPostings on s.JobId equals j.Id
                        where s.JobId == jobId
                        select new { adminSelect = s,employee = t ,job = j};

            int totalcount = query.Count();

            var data = await query.Select(s => new UserDetailDto
            {
                Id = s.employee.Id,
                RoleId = s.employee.RoleId,
               
                FirstName = s.employee.FirstName,
                LastName = s.employee.LastName,
                Email = s.employee.Email,
                Password = s.employee.Password,
                GovernmentId = s.employee.GovernmentId,
                CompanyName = s.employee.CompanyName,
                ContactName = s.employee.ContactName,
                Status = s.employee.Status,
                CreatedOn = s.employee.CreatedOn,
                IsVerify = s.employee.IsVerify,
                UpdatedOn = s.employee.UpdatedOn,
                ProfileImg = s.employee.ProfileImg,
                MobileNumber = s.employee.MobileNumber,
                IsVerifyByAdmin = s.employee.IsVerifyByAdmin,

            }).AsNoTracking().ToListAsync();

            return (data, totalcount);
        }
    }
}
