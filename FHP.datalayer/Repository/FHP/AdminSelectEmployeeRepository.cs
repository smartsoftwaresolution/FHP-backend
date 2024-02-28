using FHP.dtos.FHP;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        select new { adminSelect = s };

            var totalCount = await query.CountAsync(s => s.adminSelect.IsSelected != false);

            if(jobId > 0)
            {
                query = query.Where(s=> s.adminSelect.JobId == jobId);
            }

            if (!String.IsNullOrEmpty(search))
            {
                query =query.Where(s=>s.adminSelect.JobId.ToString().Contains(search) || 
                                       s.adminSelect.EmployeeId.ToString().Contains(search) || 
                                       s.adminSelect.InProbationCancel.ToString().Contains(search));
            }


            if(page > 0 && pageSize > 0)
            {
                query =query.Skip((page - 1) * pageSize).Take(pageSize);    
            }

            query = query.OrderByDescending(s => s.adminSelect.Id);

            var data = await query.Select(s => new AdminSelectEmployeeDetailDto
                                                         {
                                                            Id = s.adminSelect.Id,
                                                            JobId = s.adminSelect.JobId,
                                                            EmployeeId = s.adminSelect.EmployeeId,
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
                   where  s.Id == id

                   select new AdminSelectEmployeeDetailDto
                   {
                       Id=s.Id,
                       JobId=s.JobId,
                       EmployeeId=s.EmployeeId,
                       InProbationCancel=s.InProbationCancel,
                       IsSelected=s.IsSelected,
                   }).AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.AdminSelectEmployees.Where(s => s.Id == id).FirstOrDefaultAsync();
           
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        
    }
}
