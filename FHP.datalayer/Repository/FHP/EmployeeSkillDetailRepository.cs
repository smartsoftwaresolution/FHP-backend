using FHP.dtos.FHP.EmployeeSkill;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.EmployeeSkillDetail;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;

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
        public async Task AddAsync(AddEmployeeSkillDetailModel entity)
        {
            var employeeSkillDetails = entity.SkillId.Select(skillId => new EmployeeSkillDetail
            {
                UserId = entity.UserId,
                SkillId = skillId,
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active,
            }).ToList();

            await _dataContext.EmployeeSkillDetails.AddRangeAsync(employeeSkillDetails);
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


        public async Task<(List<EmployeeSkillDetailDto> employeeSkillDetail, int totalCount)> GetAllAsync(int page, int pageSize,int userId, string? search, string? skillName)
        {
            var query = from s in _dataContext.EmployeeSkillDetails 
                        join t in _dataContext.SkillsDetails on s.SkillId equals t.Id
                        
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { employeeSkillDetail = s , SkillsDetail= t };

            

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.employeeSkillDetail.UserId.ToString().Contains(search) ||
                                         s.employeeSkillDetail.SkillId.ToString().Contains(search));
            }

            if(userId > 0)
            {
                query = query.Where(s => s.employeeSkillDetail.UserId == userId);
            }

            var totalCount = await query.CountAsync();
            
            query = query.OrderByDescending(s => s.employeeSkillDetail.Id);

            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }


            var data = await query.Select(s => new EmployeeSkillDetailDto
            {
                Id = s.employeeSkillDetail.Id,
                UserId = s.employeeSkillDetail.UserId,
                SkillId = s.employeeSkillDetail.SkillId,
                SkillName = s.SkillsDetail.SkillName,
                CreatedOn = s.employeeSkillDetail.CreatedOn,
                UpdatedOn = s.employeeSkillDetail.UpdatedOn,
                Status = s.employeeSkillDetail.Status,
            }).AsNoTracking().ToListAsync();

            return (data, totalCount);
        }

        public async Task<EmployeeSkillDetailDto> GetByIdAsync(int id)
        {
           return await (from s in _dataContext.EmployeeSkillDetails
                   where s.Status != Constants.RecordStatus.Deleted &&
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
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteByIdsAsync(List<int> ids)
        {
            var data = await _dataContext.EmployeeSkillDetails.Where(s => ids.Contains(s.Id)).ToListAsync();

            foreach(var item in data)
            {
                item.Status = Constants.RecordStatus.Deleted;
                _dataContext.Update(item);
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}
