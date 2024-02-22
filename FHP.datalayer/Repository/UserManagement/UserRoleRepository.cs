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
    public class UserRoleRepository:IUserRoleRepository
    {
        private readonly DataContext _dataContext;

        public UserRoleRepository(DataContext dataContext)
        {
            _dataContext= dataContext;
        }

        public async Task AddAsync(UserRole entity)
        {
            await _dataContext.UserRole.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(UserRole entity)
        {
             _dataContext.UserRole.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<UserRole> GetAsync(int id)
        {
            return await _dataContext.UserRole.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<UserRoleDetailDto>> GetAllAsync()
        {
           return  await (from s in _dataContext.UserRole
                   where s.Status != utilities.Constants.RecordStatus.Deleted 
                   select new UserRoleDetailDto
                   {
                       Id=s.Id,
                       RoleName =s.RoleName,
                       Status = s.Status,
                       CreatedOn = s.CreatedOn, 
                       UpdatedOn = s.UpdatedOn,
                   }).ToListAsync();
        }

        public async Task<UserRoleDetailDto> GetByIdAsync(int id)
        {
           return  await (from s in _dataContext.UserRole
                   where s.Status != utilities.Constants.RecordStatus.Deleted && s.Id == id
                   select new UserRoleDetailDto
                   {
                       Id=s.Id,
                       RoleName =s.RoleName,
                       Status=s.Status,
                       CreatedOn=s.CreatedOn,
                       UpdatedOn=s.UpdatedOn,
                   }).FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.UserRole.Where(s => s.Id == id).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.UserRole.Update(data);
          await  _dataContext.SaveChangesAsync();
        }
    }
}
