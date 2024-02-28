using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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

        public async Task<(List<EmployeeDetailDto>employee,int totalCount)> GetAllAsync(int page,int pageSize,int userId, string? search)
        {

            var query =  from s in _dataContext.EmployeeDetails
                              where s.Status != Constants.RecordStatus.Deleted
                              select new { employee = s };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where( s=> s.employee.Gender.Contains(search) ||
                                         s.employee.Hobby.Contains(search) ||
                                         s.employee.Mobile.Contains(search) || 
                                         s.employee.AlternateEmail.Contains(search) ||
                                         s.employee.Mobile.Contains(search));
            }
            var totalCount = await query.CountAsync(x => x.employee.Status != Constants.RecordStatus.Deleted);
            if(userId > 0)
            {
                query = query.Where(s => s.employee.UserId == userId);
            }
            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            query = query.OrderByDescending(s=> s.employee.Id);   // orderbyDescending
            
            var data = await query.Select( s => new EmployeeDetailDto {
                                                                    Id = s.employee.Id,
                                                                    UserId = s.employee.UserId,
                                                                    MaritalStatus = s.employee.MaritalStatus,
                                                                    Gender = s.employee.Gender,
                                                                    DOB = s.employee.DOB,
                                                                    CountryId = s.employee.CountryId,
                                                                    StateId = s.employee.StateId,
                                                                    CityId = s.employee.CityId,
                                                                    ResumeURL = s.employee.ResumeURL,
                                                                    ProfileImgURL = s.employee.ProfileImgURL,
                                                                    IsAvailable = s.employee.IsAvailable,
                                                                    Hobby = s.employee.Hobby,
                                                                    PermanentAddress = s.employee.PermanentAddress,
                                                                    AlternateAddress = s.employee.AlternateAddress,
                                                                    Mobile = s.employee.Mobile,
                                                                    Phone = s.employee.Phone,
                                                                    AlternateEmail = s.employee.AlternateEmail,
                                                                    AlternatePhone = s.employee.AlternatePhone,
                                                                    EmergencyContactName = s.employee.EmergencyContactName,
                                                                    EmergencyContactNumber = s.employee.EmergencyContactNumber,
                                                                    CreatedOn = s.employee.CreatedOn,
                                                                    UpdatedOn = s.employee.UpdatedOn,
                                                                    Status = s.employee.Status,
                                                                }).AsNoTracking().ToListAsync();

            return (data, totalCount);
        }

        public async Task<EmployeeDetailDto> GetByIdAsync(int id)
        {
          return  await (from s in _dataContext.EmployeeDetails where s.Status !=utilities.Constants.RecordStatus.Deleted &&
                  s.Id == id

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
