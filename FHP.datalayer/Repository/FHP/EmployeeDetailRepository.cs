using FHP.dtos.FHP;
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
    public class EmployeeDetailRepository:IEmployeeDetailRepository
    {
        private readonly DataContext _dataContext;

        public EmployeeDetailRepository(DataContext dataContext)
        {
            _dataContext=dataContext;
        }

        public async Task AddAsync(EmployeeDetail entity)
        {
            await _dataContext.EmployeeDetails.AddAsync(entity);    
           await _dataContext.SaveChangesAsync();
        }

        public void Edit(EmployeeDetail entity)
        {
           _dataContext.EmployeeDetails.Update(entity);
            _dataContext.SaveChanges();
        }


        public async Task<EmployeeDetail> GetAsync(int id)
        {
          return   await _dataContext.EmployeeDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<EmployeeDetailDto>> GetAllAsync()
        {
          return   await (from s in _dataContext.EmployeeDetails where s.Status!=utilities.Constants.RecordStatus.Deleted

                   select new EmployeeDetailDto
                   {
                       Id=s.Id,
                       UserId=s.UserId,
                       MaritalStatus=s.MaritalStatus,
                       Gender=s.Gender, 
                       DOB=s.DOB,
                       CountryId=s.CountryId,
                       StateId=s.StateId,   
                       CityId=s.CityId,
                       ResumeURL=s.ResumeURL,
                       ProfileImgURL=s.ProfileImgURL,
                       IsAvailable=s.IsAvailable,
                       Hobby=s.Hobby,
                       PermanentAddress=s.PermanentAddress,
                       AlternateAddress=s.AlternateAddress,
                       Mobile = s.Mobile,
                       Phone = s.Phone,
                       AlternateEmail=s.AlternateEmail,
                       AlternatePhone=s.AlternatePhone,
                       EmergencyContactName=s.EmergencyContactName, 
                       EmergencyContactNumber=s.EmergencyContactNumber,
                       CreatedOn=s.CreatedOn,   
                       UpdatedOn=s.UpdatedOn,
                       Status=s.Status,
                   }).AsNoTracking().ToListAsync();
        }

        public async Task<EmployeeDetailDto> GetByIdAsync(int id)
        {
          return  await (from s in _dataContext.EmployeeDetails where s.Status !=utilities.Constants.RecordStatus.Deleted &&
                  (s.Id == id)

                  select new EmployeeDetailDto
                  {
                      Id = s.Id,
                      UserId = s.UserId,
                      MaritalStatus = s.MaritalStatus,
                      Gender = s.Gender,
                      DOB = s.DOB,
                      CountryId = s.CountryId,
                      StateId = s.StateId,
                      CityId = s.CityId,
                      ResumeURL = s.ResumeURL,
                      ProfileImgURL = s.ProfileImgURL,
                      IsAvailable = s.IsAvailable,
                      Hobby = s.Hobby,
                      PermanentAddress = s.PermanentAddress,
                      AlternateAddress = s.AlternateAddress,
                      Mobile = s.Mobile,
                      Phone = s.Phone,
                      AlternateEmail = s.AlternateEmail,
                      AlternatePhone = s.AlternatePhone,
                      EmergencyContactName = s.EmergencyContactName,
                      EmergencyContactNumber = s.EmergencyContactNumber,
                      CreatedOn = s.CreatedOn,
                      UpdatedOn = s.UpdatedOn,
                      Status = s.Status,
                  }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.EmployeeDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();  
        }
    }
}
