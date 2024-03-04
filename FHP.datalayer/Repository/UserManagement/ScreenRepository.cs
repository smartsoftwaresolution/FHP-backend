using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.UserManagement
{
    public class ScreenRepository: IScreenRepository
    {
        private readonly DataContext _dataContext;

        public ScreenRepository(DataContext dataContext)
        {
                _dataContext=dataContext;
        }
        public async Task AddAsync(Screen entity)
        {
            await _dataContext.Screen.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(Screen entity)
        {
            _dataContext.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<Screen> GetAsync(int id)
        {
           return await _dataContext.Screen.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(List<ScreenDetailDto> screen,int totalCount)> GetAllAsync(int page ,int pageSize,string search)
        {

            var query = from s in _dataContext.Screen
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { screen = s };


            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.screen.ScreenName.Contains(search) ||
                                    s.screen.ScreenCode.Contains(search));
            }

            var totalCount = await _dataContext.Screen.CountAsync(s => s.Status != Constants.RecordStatus.Deleted);


            if (page > 0 && pageSize >0)
            {
                query =query.Skip((page - 1 ) * pageSize).Take(pageSize);
            }


            query = query.OrderByDescending(s => s.screen.Id);

            var data = await query.Select(s => new ScreenDetailDto
            {
                Id = s.screen.Id,
                ScreenName = s.screen.ScreenName,
                ScreenCode = s.screen.ScreenCode,
                Status = s.screen.Status,
                CreatedOn = s.screen.CreatedOn,
                UpdatedOn = s.screen.UpdatedOn,
            }).AsNoTracking().ToListAsync();


            return (data,totalCount);

        }

        public async Task<ScreenDetailDto> GetByIdAsync(int id)
        {
           return  await (from s in _dataContext.Screen
                   where
                   s.Status != utilities.Constants.RecordStatus.Deleted &&
                   s.Id == id
                   select new ScreenDetailDto
                   {
                       Id = s.Id,
                       ScreenName = s.ScreenName,
                       ScreenCode = s.ScreenCode,
                       Status = s.Status,
                       CreatedOn = s.CreatedOn,
                       UpdatedOn = s.UpdatedOn,
                   }).FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.Screen.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
