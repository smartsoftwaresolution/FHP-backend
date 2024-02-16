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

        public async Task<List<EmailSettingDetailDto>> GetAllAsync(int CompnayId)
        {
           return await (from s in _dataContext.EmailSetting where 
                   s.Status!=utilities.Constants.RecordStatus.Deleted && 
                   (s.CompanyId==CompnayId || CompnayId==0) 
                   select new EmailSettingDetailDto
                   {
                       Id=s.Id,
                       CompanyId=s.CompanyId,
                       Email=s.Email,
                       Password=s.Password,
                       AppPassword=s.AppPassword,
                       IMapHost=s.IMapHost,
                       IMapPort=s.IMapPort,
                       SmtpHost=s.SmtpHost,
                       SmtpPort=s.SmtpPort,
                       Status=s.Status,
                       CreatedOn=s.CreatedOn,
                       UpdatedOn=s.UpdatedOn,
                   }).ToListAsync();
        }

        public async Task<EmailSettingDetailDto> GetByIdAsync(int id,int CompanyId)
        {
            return await (from s in _dataContext.EmailSetting
                   where
                   s.Status != utilities.Constants.RecordStatus.Deleted &&
                   s.Id == id && s.CompanyId == CompanyId
                   select new EmailSettingDetailDto
                   {
                       Id = s.Id,
                       CompanyId = s.CompanyId,
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

                   }).FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id ,int CompanyId)
        {
            var data = await _dataContext.EmailSetting.Where(s => s.Id == id && s.CompanyId == CompanyId).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
