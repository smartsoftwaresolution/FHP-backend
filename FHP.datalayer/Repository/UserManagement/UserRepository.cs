using FHP.entity.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using Microsoft.EntityFrameworkCore;
using FHP.utilities;
using FHP.dtos.FHP;
using FHP.dtos.UserManagement.User;
using FHP.dtos.FHP.EmployeeDetail;

namespace FHP.datalayer.Repository.UserManagement
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> AddAsync(User entity, string roleName)
        {
            int roleId = await _dataContext.UserRole.Where(s => s.RoleName.ToUpper() == roleName.ToUpper()).Select(s => s.Id).FirstOrDefaultAsync();
            entity.RoleId = roleId;
            await _dataContext.User.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<User> GetAsync(int id)
        {
            return await _dataContext.User.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public void Edit(User entity)
        {
            _dataContext.User.Update(entity);
            _dataContext.SaveChanges();
        }



        public async Task<(List<UserDetailDto> user, int totalCount)> GetAllAsync(int page, int pageSize, string? search, string? roleName, bool isAscending, string? skills, string? employmentStatus, string? experience, string? jobTitle, string? rolesAndResponsibilities)
        {
            var query = from s in _dataContext.User
                        join t in _dataContext.UserRole on s.RoleId equals t.Id
                        join e in _dataContext.EmployeeProfessionalDetails on s.Id equals e.Id
                        join j in _dataContext.JobPostings on s.Id equals j.Id

                        where s.Status != Constants.RecordStatus.Deleted
                        select new { user = s, t, employeedetail = e, job = j };



            

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.user.FirstName.Contains(search) ||
                                      s.user.LastName.Contains(search) ||
                                      s.user.Email.Contains(search) ||
                                      s.user.GovernmentId.Contains(search));
            }



            if (!string.IsNullOrEmpty(roleName))
            {
                if (roleName.ToLower().Contains("admin"))
                {
                    query = query.Where(s => s.t.RoleName != "employer" && s.t.RoleName != "employee");
                }
                else
                {
                    query = query.Where(s => s.t.RoleName == roleName);
                }

            }

            if (skills != null)
            {
                query = query.Where(s => s.job.Skills == skills);
            }

            if (employmentStatus != null)
            {
                query = query.Where(s => s.employeedetail.EmploymentStatus == employmentStatus);
            }

            if (experience != null)
            {
                query = query.Where(s => s.job.Experience == experience);
            }

            if (jobTitle != null)
            {
                query = query.Where(s => s.job.JobTitle == jobTitle);
            }

            if (rolesAndResponsibilities != null)
            {
                query = query.Where(s => s.job.RolesAndResponsibilities == rolesAndResponsibilities);
            }

            var totalCount = await query.CountAsync();

            if (isAscending == true)
            {
                query = query.OrderBy(s => s.user.Id);

            }

            else
            {
                query = query.OrderByDescending(s => s.user.Id);
            }

            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var data = await query.Select(s => new UserDetailDto
            {
                Id = s.user.Id,
                RoleId = s.user.RoleId,
                RoleName = s.t.RoleName,
                FirstName = s.user.FirstName,
                LastName = s.user.LastName,
                Email = s.user.Email,
                Password = s.user.Password,
                GovernmentId = s.user.GovernmentId,
                CompanyName = s.user.CompanyName,
                ContactName = s.user.ContactName,
                Status = s.user.Status,
                CreatedOn = s.user.CreatedOn,
                IsVerify = s.user.IsVerify,
                UpdatedOn = s.user.UpdatedOn,
                ProfileImg = s.user.ProfileImg,
                MobileNumber = s.user.MobileNumber,
                IsVerifyByAdmin = s.user.IsVerifyByAdmin,
                EmploymentType = s.user.EmploymentType,
                Skills = s.job.Skills,
                EmploymentStatus = s.employeedetail.EmploymentStatus,
                JobTitle = s.job.JobTitle,
                Experience = s.job.Experience,
                RolesAndResponsibilities = s.job.RolesAndResponsibilities,


                EmployeeDetails = (from e in _dataContext.EmployeeDetails
                                   where e.UserId == s.user.Id
                                   select new EmployeeDetailDto
                                   {
                                       Id = e.Id,
                                       UserId = e.UserId,
                                       MaritalStatus = e.MaritalStatus,
                                       Gender = e.Gender,
                                       DOB = e.DOB,
                                       CountryId = e.CountryId,
                                       StateId = e.StateId,
                                       CityId = e.CityId,
                                       ResumeURL = e.ResumeURL,
                                       ProfileImgURL = e.ProfileImgURL,
                                       IsAvailable = e.IsAvailable,
                                       Hobby = e.Hobby,
                                       PermanentAddress = e.PermanentAddress,
                                       AlternateAddress = e.AlternateAddress,
                                       Mobile = e.Mobile,
                                       Phone = e.Phone,
                                       AlternateEmail = e.AlternateEmail,
                                       AlternatePhone = e.AlternatePhone,
                                       EmergencyContactName = e.EmergencyContactName,
                                       EmergencyContactNumber = e.EmergencyContactNumber,
                                       CreatedOn = e.CreatedOn,
                                       UpdatedOn = e.UpdatedOn,
                                       Status = e.Status,
                                   })
                                  .AsNoTracking()
                                  .ToList(),
            })
            .AsNoTracking()
            .ToListAsync();


            return (data, totalCount);
        }

        public async Task<UserDetailDto> GetByIdAsync(int id)
        {

            return await (from s in _dataContext.User
                          join t in _dataContext.UserRole
                         on s.RoleId equals t.Id
                          where s.Status != Constants.RecordStatus.Deleted && s.Id == id

                          select new UserDetailDto
                          {
                              Id = s.Id,
                              RoleId = s.RoleId,
                              RoleName = t.RoleName,
                              FirstName = s.FirstName,
                              LastName = s.LastName,
                              Email = s.Email,
                              Password = s.Password,
                              GovernmentId = s.GovernmentId,
                              CompanyName = s.CompanyName,
                              ContactName = s.ContactName,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              IsVerify = s.IsVerify,
                              UpdatedOn = s.UpdatedOn,
                              ProfileImg = s.ProfileImg,
                              MobileNumber =  s.MobileNumber,
                              IsVerifyByAdmin = s.IsVerifyByAdmin,
                              EmploymentType = s.EmploymentType,
                          }).AsNoTracking().FirstOrDefaultAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.User.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.User.Update(data);
            await _dataContext.SaveChangesAsync();

        }

        public async Task<UserDetailDto> GetUserByEmail(string email)
        {

            return await (from s in _dataContext.User
                          join t in _dataContext.UserRole
                             on s.RoleId equals t.Id
                          where s.Status != Constants.RecordStatus.Deleted 
                          && s.Email == email


                          select new UserDetailDto
                          {
                              Id = s.Id,
                              RoleId = s.RoleId,
                              RoleName = t.RoleName,
                              FirstName = s.FirstName,
                              LastName = s.LastName,
                              Email = s.Email,
                              Password = s.Password,
                              GovernmentId = s.GovernmentId,
                              CompanyName = s.CompanyName,
                              ContactName = s.ContactName,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              IsVerify = s.IsVerify,
                              UpdatedOn = s.UpdatedOn,
                              ProfileImg = s.ProfileImg,
                              MobileNumber =  s.MobileNumber,
                              IsVerifyByAdmin = s.IsVerifyByAdmin,
                              Otp = s.Otp,
                              EmploymentType = s.EmploymentType,
                          }).AsNoTracking().FirstOrDefaultAsync();

        }


        public async Task<UserDetailDto> GetUserByGovernmentId(string governmentId)
        {
            return await (from s in _dataContext.User
                          join t in _dataContext.UserRole on s.RoleId equals t.Id
                          where s.Status != Constants.RecordStatus.Deleted
                          && s.GovernmentId == governmentId

                          select new UserDetailDto
                          {
                              Id = s.Id,
                              RoleId = s.RoleId,
                              RoleName = t.RoleName,
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
                              MobileNumber =  s.MobileNumber,
                              IsVerifyByAdmin = s.IsVerifyByAdmin,
                              EmploymentType = s.EmploymentType,
                          }).AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task UserLogIn(LoginModule entity)
        {
            var data = await _dataContext.User.Where(s => s.Id == entity.UserId).FirstOrDefaultAsync();
            if (data != null)
            {
                data.LastLogInTime = DateTime.Now;
                _dataContext.User.Update(data);
                await _dataContext.SaveChangesAsync();
            }


            await _dataContext.LoginModule.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UserLogOut(int userId)
        {
            var data = await _dataContext.LoginModule.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (data != null)
            {
                _dataContext.LoginModule.Remove(data);
                await _dataContext.SaveChangesAsync();
            }

            var user = await _dataContext.User.Where(s => s.Id == userId &&  s.Status != Constants.RecordStatus.Deleted).FirstOrDefaultAsync();
            if (user != null)
            {
                user.LastLogOutTime = DateTime.Now;
                _dataContext.User.Update(user);
                await _dataContext.SaveChangesAsync();
            }

        }

        public async Task VerifyUser(int userId)
        {
            var data = await _dataContext.User
                                         .Where(s => s.Id == userId)
                                         .FirstOrDefaultAsync();
            data.IsVerify = true;
            _dataContext.User.Update(data);
            await _dataContext.SaveChangesAsync();

        }

        public async Task AddUserPic(int userId, string picUrl)
        {
            
                var data = await _dataContext.User.Where(s => s.Id == userId).FirstOrDefaultAsync();
                data.ProfileImg = picUrl;
                _dataContext.User.Update(data);
                await _dataContext.SaveChangesAsync();
            
            
        }


        public async Task<string> EnableDisableUser(int userId, string roleName)
        {
            string res = string.Empty;
            var data = await _dataContext.User.Where(s => s.Id == userId).FirstOrDefaultAsync();
            if(data.Status == Constants.RecordStatus.Active)
            {
                data.Status = Constants.RecordStatus.Inactive;
                res = "DeActivated";
            }
            else
            {
                data.Status = Constants.RecordStatus.Active;
                res = "Activated";

            }
            _dataContext.User.Update(data);
            await _dataContext.SaveChangesAsync();

            
            return res;
        }


        public async Task ChangePassword(int userId,string password)
        {
            var data = await _dataContext.User.Where(s => s.Id == userId).FirstOrDefaultAsync();
            data.Password = Utility.Encrypt(password);
            _dataContext.User.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task VerifyEmployerByAdmin(int userId)
        {
            var data = await _dataContext.User.Where(s => s.Id == userId).FirstOrDefaultAsync();
            data.IsVerifyByAdmin = true;
            _dataContext.User.Update(data);
            await _dataContext.SaveChangesAsync();
        }


        public async  Task<bool> SaveOtp(string email, int otp)
        {
            var data = await _dataContext.User.Where(s => s.Email == email).FirstOrDefaultAsync(); 
            if(data == null)
            {
                return false;
            }

            data.Otp = otp;
            _dataContext.User.Update(data);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task AddFCMToken(FCMToken entity)
        {
            await _dataContext.FCMTokens.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task RemoveFCMToken(int userId, string fcmToken)
        {
            var data = await _dataContext.FCMTokens.Where(s => s.UserId == userId && s.TokenFCM == fcmToken).FirstOrDefaultAsync();
            _dataContext.FCMTokens.Remove(data);
            await _dataContext.SaveChangesAsync();
        }

       
    }
}
