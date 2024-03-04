using FHP.datalayer.Migrations;
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
    public class EmployeeSkillDetailRepository: IEmployeeSkillDetailRepository
    {
        private readonly DataContext _dataContext;

        public EmployeeSkillDetailRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }

        public async Task AddAsync(EmployeeSkillDetail entity)
        {
           await _dataContext.EmployeeSkillDetails.AddAsync(entity);
           await _dataContext.SaveChangesAsync();
        }

        public void Edit(EmployeeSkillDetail entity)
        {
            _dataContext.EmployeeSkillDetails.Update(entity);
            _dataContext.SaveChanges();
        }

       

        public async Task<EmployeeSkillDetail> GetAsync(int id)
        {
            return await _dataContext.EmployeeSkillDetails.Where(s => s.Id == id).FirstOrDefaultAsync(); 
        }


        public async Task<(List<EmployeeSkillDetailDto> employeeSkillDetail, int totalCount)> GetAllAsync(int page, int pageSize, string? search)
        {
            var query = from s in _dataContext.EmployeeSkillDetails
                        where s.Status != utilities.Constants.RecordStatus.Deleted
                        select new { employeeSkillDetail = s };

            

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.employeeSkillDetail.UserId.ToString().Contains(search) ||
                                       s.employeeSkillDetail.SkillId.ToString().Contains(search));
            }

            var totalCount = await query.CountAsync(s => s.employeeSkillDetail.Status != utilities.Constants.RecordStatus.Deleted);

            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            query = query.OrderByDescending(s => s.employeeSkillDetail.Id);

            var data = await query.Select(s => new EmployeeSkillDetailDto
            {
                Id = s.employeeSkillDetail.Id,
                UserId = s.employeeSkillDetail.UserId,
                SkillId = s.employeeSkillDetail.SkillId,
                CreatedOn = s.employeeSkillDetail.CreatedOn,
                UpdatedOn = s.employeeSkillDetail.UpdatedOn,
                Status = s.employeeSkillDetail.Status,

            }).AsNoTracking().ToListAsync();

            return (data, totalCount);
        }

        public async Task<EmployeeSkillDetailDto> GetByIdAsync(int id)
        {
           return await (from s in _dataContext.EmployeeSkillDetails
                   where s.Status != utilities.Constants.RecordStatus.Deleted &&
                   s.Id == id

                   select new EmployeeSkillDetailDto
                   {
                       Id=s.Id,
                       UserId=s.UserId,
                       SkillId=s.SkillId,
                       CreatedOn=s.CreatedOn,
                       UpdatedOn=s.UpdatedOn,
                       Status=s.Status,

                   }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.EmployeeSkillDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = utilities.Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
