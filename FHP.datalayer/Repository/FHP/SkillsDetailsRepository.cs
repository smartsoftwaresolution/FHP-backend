using FHP.dtos.FHP;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.FHP
{
    public class SkillsDetailsRepository : ISkillsDetailRepository
    {
        private readonly DataContext _dataContext;

        public SkillsDetailsRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }
        public async Task AddAsync(SkillsDetail entity)
        {
           await _dataContext.SkillsDetails.AddAsync(entity);
           await _dataContext.SaveChangesAsync();
        }

        public void Edit(SkillsDetail entity)
        {
            _dataContext.SkillsDetails.Update(entity);
            _dataContext.SaveChanges();
        }

  
        public async Task<SkillsDetail> GetAsync(int id)
        {
           return  await _dataContext.SkillsDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(List<SkillsDetailDto> skill ,int totalCount)> GetAllAsync(int page, int pageSize,string? search)
        {
            var query = from s in _dataContext.SkillsDetails
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { skill = s };

            var totalCount = await _dataContext.Cities.CountAsync(s => s.Status != Constants.RecordStatus.Deleted);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.skill.SkillName.Contains(search));
            }

            if(page >0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            query = query.OrderByDescending(s => s.skill.Id);

            var data = await query.Select(s => new SkillsDetailDto
                                               {
                                                   Id = s.skill.Id,
                                                   UserId = s.skill.UserId,
                                                   SkillName = s.skill.SkillName,
                                                   CreatedOn = s.skill.CreatedOn,
                                                   UpdatedOn = s.skill.UpdatedOn,
                                                   Status = s.skill.Status,
                                               })
                                               .AsNoTracking()
                                               .ToListAsync();


            return (data, totalCount);
        }


        public async Task<SkillsDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.SkillsDetails
                          where s.Status != utilities.Constants.RecordStatus.Deleted
                          && (s.Id == id)

                          select new SkillsDetailDto
                          {
                              Id = s.Id,
                              UserId = s.UserId,
                              SkillName = s.SkillName,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                              Status = s.Status,
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data =await _dataContext.SkillsDetails.Where(s=>s.Id== id).FirstOrDefaultAsync();
            data.Status=Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
