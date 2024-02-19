using FHP.datalayer.Migrations;
using FHP.entity.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHP.dtos.UserManagement;
using FHP.utilities;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace FHP.datalayer.Repository.UserManagement
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(User entity, string roleName)
        {
            int roleId = await _dataContext.UserRole.Where(s => s.RoleName.ToUpper() == roleName.ToUpper()).Select(s => s.Id).FirstOrDefaultAsync();
            entity.RoleId = roleId;
            await _dataContext.User.AddAsync(entity);
            await _dataContext.SaveChangesAsync();



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

        public async Task<List<UserDetailDto>> GetAllAsync(int companyId)
        {
            return await (from s in _dataContext.User
                          join t in _dataContext.UserRole
                          on s.RoleId equals t.Id
                          where s.Status != Constants.RecordStatus.Deleted
                          && (companyId == 0 || s.CompanyId == companyId)

                          select new UserDetailDto
                          {
                              Id = s.Id,
                              CompanyId = s.CompanyId,
                              //CompanyName = _dataContext.Companies.Where(e => e.Id == s.CompanyId).Select(q=> q.Name).FirstOrDefault(),
                              CompanyName = (from i in _dataContext.Companies where s.CompanyId == i.Id select i.Name).FirstOrDefault(),
                              RoleId = s.RoleId,
                              RoleName = t.RoleName,
                              GovernmentId = s.GovernmentId,
                              FullName = s.FullName,
                              Email = s.Email,
                              Address = s.Address,
                              Password = s.Password,
                              MobileNumber = s.MobileNumber,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                          }).ToListAsync();
        }

        public async Task<UserDetailDto> GetByIdAsync(int id, int companyId)
        {

            return await (from s in _dataContext.User
                          where s.Status != Constants.RecordStatus.Deleted && s.Id == id
                          && s.CompanyId == companyId
                          select new UserDetailDto
                          {
                              Id = s.Id,
                              CompanyId = s.CompanyId,
                              RoleId = s.RoleId,
                              GovernmentId = s.GovernmentId,
                              FullName = s.FullName,
                              Email = s.Email,
                              Address = s.Address,
                              Password = s.Password,
                              MobileNumber = s.MobileNumber,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                          }).FirstOrDefaultAsync();

        }

        public async Task DeleteAsync(int id, int companyId)
        {
            var data = await _dataContext.User.Where(s => s.Id == id && s.CompanyId == companyId).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.User.Update(data);
            await _dataContext.SaveChangesAsync();

        }

        public async Task<UserDetailDto> GetUserByEmail(string email, int companyId)
        {

            return await (from s in _dataContext.User
                          where s.Status != Constants.RecordStatus.Deleted
                          && s.Email == email
                          && s.CompanyId == companyId

                          select new UserDetailDto
                          {
                              Id = s.Id,
                              CompanyId = s.CompanyId,
                              RoleId = s.RoleId,
                              GovernmentId = s.GovernmentId,
                              FullName = s.FullName,
                              Email = s.Email,
                              Address = s.Address,
                              Password = s.Password,
                              MobileNumber = s.MobileNumber,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                          }).AsNoTracking().FirstOrDefaultAsync();

        }


        public async Task<UserDetailDto> GetUserByGovernmentId(string governmentId, int companyId)
        {
            return await (from s in _dataContext.User
                          join t in _dataContext.UserRole on s.RoleId equals t.Id
                          where s.Status != Constants.RecordStatus.Deleted
                          && s.GovernmentId == governmentId
                          && s.CompanyId == companyId
                          select new UserDetailDto
                          {
                              Id = s.Id,
                              CompanyId = s.CompanyId,
                              RoleId = s.RoleId,
                              RoleName = t.RoleName,
                              GovernmentId = s.GovernmentId,
                              FullName = s.FullName,
                              Email = s.Email,
                              Address = s.Address,
                              Password = s.Password,
                              MobileNumber = s.MobileNumber,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
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

        public async Task UserLogOut(int userId, int companyId)
        {
            var data = await _dataContext.LoginModule.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (data != null)
            {
                _dataContext.LoginModule.Remove(data);
                await _dataContext.SaveChangesAsync();
            }

            var user = await _dataContext.User.Where(s => s.Id == userId && s.LastLogOutTime != null && s.Status != Constants.RecordStatus.Deleted).FirstOrDefaultAsync();
            if (user != null)
            {
                user.LastLogOutTime = DateTime.Now;
                _dataContext.User.Update(user);
                await _dataContext.SaveChangesAsync();
            }

        }
    }
}
