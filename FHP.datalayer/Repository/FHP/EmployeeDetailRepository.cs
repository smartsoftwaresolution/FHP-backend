using FHP.dtos.FHP;
using FHP.dtos.FHP.EmployeeDetail;
using FHP.dtos.FHP.EmployeeEducationalDetail;
using FHP.dtos.FHP.EmployeeProfessionalDetail;
using FHP.entity.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;

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
            return await _dataContext.EmployeeDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(List<EmployeeDetailDto>employee,int totalCount)> GetAllAsync(int page,int pageSize,int userId, string? search)
        {

            var query =  from s in _dataContext.EmployeeDetails
                              where s.Status != Constants.RecordStatus.Deleted
                              select new { employee = s };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where( s =>s.employee.Gender.Contains(search) ||
                                         s.employee.Hobby.Contains(search) ||
                                         s.employee.Mobile.Contains(search) || 
                                         s.employee.AlternateEmail.Contains(search) ||
                                         s.employee.Mobile.Contains(search));
            }

            var totalCount = await query.CountAsync(); 

            if(userId > 0)
            {
                query = query.Where(s => s.employee.UserId == userId);
            }

            query = query.OrderByDescending(s => s.employee.Id);   // orderbyDescending

            if(page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize);
            }

            
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

        public async Task<string> SetAvailabilityAsync(int id)
        {
            string result = string.Empty;
            var data = await _dataContext.EmployeeDetails.Where(s => s.Id == id).FirstOrDefaultAsync();
            if(data.IsAvailable == false)
            {
                data.IsAvailable = true;
                result = "Available";
            }
            else
            {
                data.IsAvailable = false;
                result = "UnAvailable";
            }
            _dataContext.EmployeeDetails.Update(data);
            await _dataContext.SaveChangesAsync();
            return result;
        }


        public async Task<CompleteEmployeeDetailDto> GetAllByIdAsync(int id)
        {
            var data = await (from s in _dataContext.User
                              join emp in _dataContext.EmployeeDetails on s.Id equals emp.UserId
                              where s.Id == id && s.Status != Constants.RecordStatus.Deleted
                              select new CompleteEmployeeDetailDto
                              {
                                  Id = s.Id,
                                  RoleId = s.RoleId,
                                // RoleName = t.RoleName,
                                  FirstName = s.FirstName,
                                  LastName = s.LastName,
                                  Email = s.Email,
                                  Password = s.Password,
                                  GovernmentId = s.GovernmentId,
                                  CompanyName = s.CompanyName,
                                  ContactName = s.ContactName,
                                  Status = s.Status,
                                  CreatedOn = s.CreatedOn,
                                  UpdatedOn = s.UpdatedOn,
                                  IsVerify = s.IsVerify,
                                  ProfileImg = s.ProfileImg,
                                  MobileNumber = s.MobileNumber,
                                  IsVerifyByAdmin = s.IsVerifyByAdmin,
                                  EmployeeDetail = new EmployeeDetailDto
                                  {
                                      Id = emp.Id,
                                      UserId = emp.UserId,
                                      MaritalStatus = emp.MaritalStatus,
                                      Gender = emp.Gender,
                                      DOB = emp.DOB,
                                      CountryId = emp.CountryId,
                                      StateId = emp.StateId,
                                      CityId = emp.CityId,
                                      ResumeURL = emp.ResumeURL,
                                      ProfileImgURL = emp.ProfileImgURL,
                                      IsAvailable = emp.IsAvailable,
                                      Hobby = emp.Hobby,
                                      PermanentAddress = emp.PermanentAddress,
                                      AlternateAddress = emp.AlternateAddress,
                                      Mobile = emp.Mobile,
                                      Phone = emp.Phone,
                                      AlternateEmail = emp.AlternateEmail,
                                      AlternatePhone = emp.AlternatePhone,
                                      EmergencyContactName = emp.EmergencyContactName,
                                      EmergencyContactNumber = emp.EmergencyContactNumber,
                                      CreatedOn = emp.CreatedOn,
                                      UpdatedOn = emp.UpdatedOn,
                                      Status = emp.Status,

                                  },
                                  Professional = (from p in _dataContext.EmployeeProfessionalDetails
                                                  where p.UserId == s.Id
                                                  select new EmployeeProfessionalDetailDto
                                                  {
                                                      Id = p.Id,
                                                      UserId = p.UserId,
                                                      JobDescription = p.JobDescription,
                                                      StartDate = p.StartDate,
                                                      EndDate = p.EndDate,
                                                      CompanyName = p.CompanyName,
                                                      CompanyLocation = p.CompanyLocation,
                                                      Designation = p.Designation,
                                                      EmploymentStatus = p.EmploymentStatus,
                                                      YearsOfExperience = p.YearsOfExperience,
                                                      CreatedOn = p.CreatedOn,
                                                      UpdatedOn = p.UpdatedOn,
                                                      Status = p.Status,
                                                  }).ToList(),
                                  Education = (from e in _dataContext.EmployeeEducationalDetails
                                               where e.UserId == s.Id
                                               select new EmployeeEducationalDetailDto
                                               {
                                                   Id = e.Id,
                                                   UserId = e.UserId,
                                                   Education = e.Education,
                                                   NameOfBoardOrUniversity = e.NameOfBoardOrUniversity,
                                                   YearOfCompletion = e.YearOfCompletion,
                                                   MarksObtained = e.MarksObtained,
                                                   GPA = e.GPA,
                                                   CreatedOn = e.CreatedOn,
                                                   UpdatedOn = e.UpdatedOn,
                                                   Status = e.Status,
                                               }).ToList()

                              }).AsNoTracking().FirstOrDefaultAsync();

            return data;
        }

    }
}
