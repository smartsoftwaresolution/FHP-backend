
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

        public async Task<(List<UserDetailDto> user, int totalCount)> GetAllAsync(int page, int pageSize, string? search, string? roleName)
        {

            var query = from s in _dataContext.User
                        join t in _dataContext.UserRole on s.RoleId equals t.Id
                        where s.Status != Constants.RecordStatus.Deleted 
                        select new { user = s, t };

            var totalCount = await _dataContext.User.CountAsync(s => s.Status != Constants.RecordStatus.Deleted);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.user.FirstName.Contains(search) ||
                                      s.user.LastName.Contains(search) ||
                                      s.user.Email.Contains(search) ||
                                      s.user.GovernmentId.Contains(search));
            }

            if (!string.IsNullOrEmpty(roleName))
            {
                query = query.Where(s => s.t.RoleName == roleName);
            }


            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            query = query.OrderByDescending(s => s.user.Id);

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
                Status = s.user.Status,
                CreatedOn = s.user.CreatedOn,
                UpdatedOn = s.user.UpdatedOn,
            }).AsNoTracking().ToListAsync();

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
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                          }).FirstOrDefaultAsync();

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
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
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

        public async Task UserLogOut(int userId)
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
