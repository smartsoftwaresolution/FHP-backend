using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.UserManagement
{
    public class PermissionRepository:IPermissionRepository
    {
        private readonly DataContext _dataContext;

        public PermissionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Permission entity)
        {
            await _dataContext.Permissions.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(Permission entity)
        {
            _dataContext.Permissions.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<Permission> GetAsync(int id)
        {
           return await _dataContext.Permissions.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(List<PermissionDetailDto> permission,int totalCount)> GetAllAsync(int page ,int pageSize,string search)
        {
            var query = from s in _dataContext.Permissions
                        where s.Status != Constants.RecordStatus.Deleted
                        select new  { permission = s };




            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.permission.PermissionDescription.Contains(search) || 
                                         s.permission.PermissionCode.Contains(search) ||
                                         s.permission.Permissions.Contains(search));

            }

            var totalCount = await _dataContext.Permissions.CountAsync(s => s.Status != Constants.RecordStatus.Deleted);


            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            query = query.OrderByDescending(s => s.permission.Id);

            var data = await query.Select(s => new PermissionDetailDto
            {
                Id = s.permission.Id,
                Permissions = s.permission.Permissions,
                PermissionDescription = s.permission.PermissionDescription,
                PermissionCode = s.permission.PermissionCode,
                ScreenCode = s.permission.ScreenCode,
                ScreenUrl = s.permission.ScreenUrl,
                ScreenId = s.permission.ScreenId,
                Screen = s.permission.Screen,
                Status = s.permission.Status,
                CreatedOn = s.permission.CreatedOn,
                UpdatedOn = s.permission.UpdatedOn,
                CreatedBy = s.permission.CreatedBy,
            }).AsNoTracking().ToListAsync();


            return (data, totalCount);
        }

        public async Task<PermissionDetailDto> GetByIdAsync(int id)
        {
            return await (from s in _dataContext.Permissions
                          where
                          s.Status != utilities.Constants.RecordStatus.Deleted &&
                          s.Id == id 
                          select new PermissionDetailDto
                          {
                              Id = s.Id,
                           
                              Permissions = s.Permissions,
                              PermissionDescription = s.PermissionDescription,
                              PermissionCode = s.PermissionCode,
                              ScreenCode = s.ScreenCode,
                              ScreenUrl = s.ScreenUrl,
                              ScreenId = s.ScreenId,
                              Screen = s.Screen,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                              CreatedBy = s.CreatedBy,
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.Permissions.Where(s => s.Id == id ).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        
    }
}
