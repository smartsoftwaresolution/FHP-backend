using FHP.entity.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using Microsoft.EntityFrameworkCore;
using FHP.utilities;
using FHP.dtos.UserManagement.User;
using FHP.dtos.FHP.EmployeeDetail;
using DocumentFormat.OpenXml.Office2010.Excel;

using Castle.Core.Internal;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using FHP.dtos.FHP.JobPosting;
using System.Linq.Dynamic.Core;

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

                        /*join e in _dataContext.EmployeeProfessionalDetails on s.Id equals e.UserId into empDetails
                        from ed in empDetails.DefaultIfEmpty()*/
                      /*  join j in _dataContext.JobPostings on s.Id equals j.UserId into jobPosting
                        from jd in jobPosting.DefaultIfEmpty()*/

                        where s.Status != Constants.RecordStatus.Deleted
                        select new { user = s, t,job = s.JobPosts,professional = s.ProfessionalDetails/*, employeedetail = ed*/ };

                        


            
            

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
                query = query.Where(s => s.user.JobPosts.Any( r=> r.Skills == skills));
            }


            if (employmentStatus != null)
            {
                query = query.Where(s => s.user.ProfessionalDetails.Any(y=> y.EmploymentStatus == employmentStatus));
            }

            


            if (experience != null)
            {
                query = query.Where(s => /*s.job.Experience == experience */  s.user.JobPosts.Any(t => t.Experience == experience));
            }

            /*if (employmentStatus != null)
            {
                query = query.Where(s => s.employeedetail.EmploymentStatus == employmentStatus);
            }*/

            if (jobTitle != null)
            {
                query = query.Where(s => /*s.job.JobTitle == jobTitle &&*/ s.user.JobPosts.Any(t => t.JobTitle == jobTitle));
            }

            if (rolesAndResponsibilities != null)
            {
                query = query.Where(s => /*s.job.RolesAndResponsibilities == rolesAndResponsibilities &&*/ s.user.JobPosts.Any(t => t.RolesAndResponsibilities == rolesAndResponsibilities));
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
                EmploymentStatus =  _dataContext.EmployeeProfessionalDetails.Where( r=> r.UserId == s.user.Id).Select( n => n.EmploymentStatus).ToString() ?? "",
                // EmploymentStatus = s.employeedetail.EmploymentStatus,
                /* Skills = s.job.Skills,
                 JobTitle = s.job.JobTitle,
                 Experience = s.job.Experience,
                 RolesAndResponsibilities = s.job.RolesAndResponsibilities,*/

                JobPostingDetail = (from j in _dataContext.JobPostings
                                    where j.UserId == s.user.Id
                                    select new JobPostingDto
                                    {
                                        Skills = j.Skills,
                                        JobTitle = j.JobTitle,
                                        RolesAndResponsibilities = j.RolesAndResponsibilities,
                                        Experience = j.Experience,
                                    }).ToList(),

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

        public async Task<double> CalculateUserPercentage(int userId)
        {
            int totalProperties = 6;
            int fulfilledProperties = 0;
            var data = await _dataContext.User.Where(s => s.Id == userId).FirstOrDefaultAsync();
            var role = await _dataContext.UserRole.Where(s => s.Id == data.RoleId).FirstOrDefaultAsync();

            if (role.RoleName.ToLower().Contains("employee"))
            {
                totalProperties--;
                if (!string.IsNullOrEmpty(data.GovernmentId)) fulfilledProperties++;

            }

            if (!string.IsNullOrEmpty(data.FirstName)) fulfilledProperties++;
            if (!string.IsNullOrEmpty(data.LastName)) fulfilledProperties++;
            if (!string.IsNullOrEmpty(data.Email)) fulfilledProperties++;
            if (!string.IsNullOrEmpty(data.MobileNumber)) fulfilledProperties++;


            if (role.RoleName.ToLower().Contains("employer"))
            {
                if (!string.IsNullOrEmpty(data.GovernmentId)) fulfilledProperties++;
            }

            // Calculate percentage based on the actual profile detail
            double percentage = (double)fulfilledProperties / totalProperties * 100;

          
            return Math.Round(percentage, 2); // Round to two decimal places

           
        }


        public async Task<double> CalculateEmployeeDetailPercentage(int userId)
        {
            int totalProperties = 17;
            int fulfilledProperties = 0;
            var data = await _dataContext.EmployeeDetails.Where(s => s.UserId == userId).FirstOrDefaultAsync();

            if (data != null)
            {



                if (!string.IsNullOrEmpty(data.MaritalStatus)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.Gender)) fulfilledProperties++;
                if (data.DOB != null) fulfilledProperties++;
                if (data.CountryId != null) fulfilledProperties++;
                if (data.StateId != null) fulfilledProperties++;
                if (data.CityId != null) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.ResumeURL)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.ProfileImgURL)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.Hobby)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.PermanentAddress)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.AlternateAddress)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.Mobile)) fulfilledProperties++;
                if (data.Phone != null) fulfilledProperties++;
                if (data.AlternatePhone != null) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.AlternateEmail)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.EmergencyContactName)) fulfilledProperties++;
                if (data.EmergencyContactNumber != null) fulfilledProperties++;



                // Calculate percentage based on the actual profile detail
                double percentage = (double)fulfilledProperties / totalProperties * 100;


                return Math.Round(percentage, 2); // Round to two decimal places
            }
            return 0.0;

        }


        public async Task<double> CalculateEmployeeEducationalDetailPercentage(int userId)
        {
            int totalProperties = 5;
            int fulfilledProperties = 0;
            var data = await _dataContext.EmployeeEducationalDetails.Where(s => s.UserId == userId).FirstOrDefaultAsync();

            if (data != null)
            {



                if (!string.IsNullOrEmpty(data.Education)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.NameOfBoardOrUniversity)) fulfilledProperties++;
                if (data.YearOfCompletion != 0) fulfilledProperties++;
                if (data.MarksObtained != null) fulfilledProperties++;
                if (data.GPA != 0.0) fulfilledProperties++;



                // Calculate percentage based on the actual profile detail
                double percentage = (double)fulfilledProperties / totalProperties * 100;


                return Math.Round(percentage, 2); // Round to two decimal places

            }
            return 0.0;
        }

        public async Task<double> CalculateEmployeeProfessionalDetailPercentage(int userId)
        {
            int totalProperties = 8;
            int fulfilledProperties = 0;
            var data = await _dataContext.EmployeeProfessionalDetails.Where(s => s.UserId == userId).FirstOrDefaultAsync();

            if (data != null)
            {


                if (!string.IsNullOrEmpty(data.JobDescription)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.StartDate)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.EndDate)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.CompanyName)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.CompanyLocation)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.Designation)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.EmploymentStatus)) fulfilledProperties++;
                if (!string.IsNullOrEmpty(data.YearsOfExperience)) fulfilledProperties++;



                // Calculate percentage based on the actual profile detail
                double percentage = (double)fulfilledProperties / totalProperties * 100;


                return Math.Round(percentage, 2); // Round to two decimal places
            }
            return 0.0;

        }
        public async Task<double> CalculateEmployeeSkillDetailPercentage(int userId)
        {
            int totalProperties = 1;
            int fulfilledProperties = 0;
            var data = await _dataContext.EmployeeSkillDetails.Where(s => s.UserId == userId).FirstOrDefaultAsync();

            if (data != null)
            {


                if (data.SkillId != 0) fulfilledProperties++;

                // Calculate percentage based on the actual profile detail
                double percentage = (double)fulfilledProperties / totalProperties * 100;


                return Math.Round(percentage, 2); // Round to two decimal places
            }
            return 0.0;

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
