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

        public async Task<List<PermissionDetailDto>> GetAllAsync(int companyId)
        {
            return await (from s in _dataContext.Permissions
                          where
                          s.Status != utilities.Constants.RecordStatus.Deleted &&
                          (s.CompanyId == companyId || companyId == 0)

                          select new PermissionDetailDto
                          {
                              Id=s.Id,
                              CompanyId=s.CompanyId,
                              Permissions = s.Permissions,
                              PermissionDescription = s.PermissionDescription,
                              PermissionCode = s.PermissionCode,    
                              ScreenCode = s.ScreenCode,
                              ScreenUrl = s.ScreenUrl,  
                              ScreenId = s.ScreenId,
                              Screen=s.Screen,
                              Status= s.Status,
                              CreatedOn = s.CreatedOn,  
                              UpdatedOn = s.UpdatedOn,
                              CreatedBy= s.CreatedBy,   

                          }).AsNoTracking().ToListAsync();
        }

        public async Task<PermissionDetailDto> GetByIdAsync(int id, int companyId)
        {
            return await (from s in _dataContext.Permissions
                          where
                          s.Status != utilities.Constants.RecordStatus.Deleted &&
                          s.Id == id && s.CompanyId == companyId
                          select new PermissionDetailDto
                          {
                              Id = s.Id,
                              CompanyId = s.CompanyId,
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

        public async Task DeleteAsync(int id, int companyId)
        {
            var data = await _dataContext.Permissions.Where(s => s.Id == id && s.CompanyId==companyId).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        
    }
}
