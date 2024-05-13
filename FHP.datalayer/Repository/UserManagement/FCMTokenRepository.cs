
using FHP.dtos.UserManagement.FcmToken;
using FHP.infrastructure.Repository.UserManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.Repository.UserManagement
{
    public class FCMTokenRepository : IFCMTokenRepository
    {
        private readonly DataContext _dataContext;
        public FCMTokenRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<FcmTokenDetailDto>> FcmTokenByRole(string roleName)
        {
            return await (from s in _dataContext.FCMTokens
                          join u in _dataContext.User on s.UserId equals u.Id
                          join r in _dataContext.UserRole on u.RoleId equals r.Id
                          where r.RoleName.ToLower() == roleName.ToLower()
                          select new FcmTokenDetailDto
                          {
                              Id = s.Id,
                              UserId = s.UserId,
                              TokenFCM = s.TokenFCM,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                          }).AsNoTracking().ToListAsync();
        }

    }
}
