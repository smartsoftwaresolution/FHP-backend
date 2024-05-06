using DocumentFormat.OpenXml.Office2010.ExcelAc;
using FHP.dtos.FHP.GlobalNotification;
using FHP.dtos.FHP.Skill;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.FHP
{
    public class GlobalNotificationRepository : IGlobalNotificationRepository
    {
        private readonly DataContext _dataContext;
        public GlobalNotificationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(GlobalNotification entity)
        {
            await _dataContext.GlobalNotifications.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(GlobalNotification entity)
        {
            _dataContext.GlobalNotifications.Update(entity);
            _dataContext.SaveChanges();
        }


        public async Task<GlobalNotification> GetAsync(int id)
        {
            return await _dataContext.GlobalNotifications.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(List<GlobalNotificationDetailDto> notification, int totalCount)> GetAllAsync(int page, int pageSize, string? search, int userId)
        {
            var query = from s in _dataContext.GlobalNotifications
                        join u in _dataContext.User on s.UserId equals u.Id
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { notification = s,user = u };


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.notification.Name.Contains(search));
            }

            if(userId > 0)
            {
                query = query.Where(s => s.notification.UserId == userId);  
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(s => s.notification.Id);

            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }


            var data = await query.Select(s => new GlobalNotificationDetailDto
            {
                Id = s.notification.Id,
                UserId = s.notification.UserId,
                Name = s.notification.Name,
                Description = s.notification.Description,
                CreatedOn = s.notification.CreatedOn,
                UpdatedOn = s.notification.UpdatedOn,
                Status = s.notification.Status,
                ProfileUrl = s.user.ProfileImg,
                IsRead = s.notification.IsRead
            })
                                               .AsNoTracking()
                                               .ToListAsync();


            return (data, totalCount);
        }


        public async Task<GlobalNotificationDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.GlobalNotifications
                          join u in _dataContext.User on s.UserId equals u.Id
                          where s.Status != utilities.Constants.RecordStatus.Deleted
                          && (s.Id == id)

                          select new GlobalNotificationDetailDto
                          {
                              Id = s.Id,
                              UserId = s.UserId,
                              Name = s.Name,
                              Description = s.Description,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                              Status = s.Status,
                              ProfileUrl = u.ProfileImg,
                              IsRead = s.IsRead,
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.GlobalNotifications.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<int>> UnreadAsync()
        {
            return await _dataContext.GlobalNotifications.Where(s => s.IsRead == false).Select(s => s.Id).ToListAsync();
        }

    }
}
