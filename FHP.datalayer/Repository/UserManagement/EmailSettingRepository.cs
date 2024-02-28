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
    public class EmailSettingRepository:IEmailSettingRepository
    {
        private readonly DataContext _dataContext;
        public EmailSettingRepository(DataContext dataContext)
        {
            _dataContext = dataContext;

        }

        public async Task AddAsync(EmailSetting entity)
        {
            await _dataContext.EmailSetting.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<EmailSetting> GetAsync(int id)
        {
             return await _dataContext.EmailSetting.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public void  Edit(EmailSetting entity)
        {
            _dataContext.EmailSetting.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<(List<EmailSettingDetailDto> emailSetting,int totalCount)> GetAllAsync(int page,int pageSize,string search)
        {
            var query = from s in _dataContext.EmailSetting
                        where s.Status != Constants.RecordStatus.Deleted
                        select new { emailSetting = s };


            var totalCount = await _dataContext.EmailSetting.CountAsync(s => s.Status != Constants.RecordStatus.Deleted);


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.emailSetting.Email.Contains(search));
            }

            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            query = query.OrderByDescending(s => s.emailSetting.Id);

            var data = await query.Select(s => new EmailSettingDetailDto
            {
                Id = s.emailSetting.Id,
                Email = s.emailSetting.Email,
                Password = s.emailSetting.Password,
                AppPassword = s.emailSetting.AppPassword,
                IMapHost = s.emailSetting.IMapHost,
                IMapPort = s.emailSetting.IMapPort,
                SmtpHost = s.emailSetting.SmtpHost,
                SmtpPort = s.emailSetting.SmtpPort,
                Status = s.emailSetting.Status,
                CreatedOn = s.emailSetting.CreatedOn,
                UpdatedOn = s.emailSetting.UpdatedOn,
            }).AsNoTracking().ToListAsync();


            return (data, totalCount);

        }

        public async Task<EmailSettingDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.EmailSetting
                   where
                   s.Status != utilities.Constants.RecordStatus.Deleted &&
                   s.Id == id 
                   select new EmailSettingDetailDto
                   {
                       Id = s.Id,
                       Email = s.Email,
                       Password = s.Password,
                       AppPassword = s.AppPassword,
                       IMapHost = s.IMapHost,
                       IMapPort = s.IMapPort,
                       SmtpHost = s.SmtpHost,
                       SmtpPort = s.SmtpPort,
                       Status = s.Status,
                       CreatedOn = s.CreatedOn,
                       UpdatedOn = s.UpdatedOn,

                   }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.EmailSetting.Where(s => s.Id == id ).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
