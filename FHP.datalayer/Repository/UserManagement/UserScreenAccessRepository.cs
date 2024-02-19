using DocumentFormat.OpenXml.Wordprocessing;
using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.UserManagement
{
    public class UserScreenAccessRepository: IUserScreenAccessRepository
    {
        private readonly DataContext _dataContext;

        public UserScreenAccessRepository(DataContext dataContext)
        {
                _dataContext= dataContext;
        }

        public async Task AddAsync(UserScreenAccess entity)
        {
           await _dataContext.UserScreenAccess.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(UserScreenAccess entity)
        {
            _dataContext.UserScreenAccess.Update(entity);
            _dataContext.SaveChanges();
        }

        public async Task<UserScreenAccess> GetAsync(int id)
        {
            return await _dataContext.UserScreenAccess.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(List<UserScreenAccessDto> userScreenAccess,int totalCount)> GetAllAsync(int page,int pageSize,int roleId)
        {
           var query = (from s in _dataContext.UserScreenAccess
                          where s.Status != utilities.Constants.RecordStatus.Deleted
                          select new { userScreenAccess = s});

            var totalCount = await query.CountAsync(x => x.userScreenAccess.Status != utilities.Constants.RecordStatus.Deleted);


            if (roleId > 0)
            {
                query = query.Where(s => s.userScreenAccess.RoleId == roleId);
            }
            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var userScreenAccess =    await query.Select(s => new UserScreenAccessDto{
                                                                    Id = s.userScreenAccess.Id,
                                                                    RoleId = s.userScreenAccess.RoleId,
                                                                    ScreenId = s.userScreenAccess.ScreenId,
                                                                    ScreenName = s.userScreenAccess.Screen.ScreenName,
                                                                    UserRoleName = s.userScreenAccess.UserRole.RoleName,
                                                                    CreatedOn = s.userScreenAccess.CreatedOn,
                                                                    Status = s.userScreenAccess.Status,
                                                                    })
                                                                      .AsNoTracking()
                                                                      .ToListAsync();

            return (userScreenAccess, totalCount);
            
        }
    }
}
