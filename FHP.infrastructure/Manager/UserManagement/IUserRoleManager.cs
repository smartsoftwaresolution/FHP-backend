﻿using FHP.dtos.UserManagement.UserRole;
using FHP.models.UserManagement.UserRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IUserRoleManager
    {
        Task AddAsync(AddUserRoleModel model);
        Task EditAsync(AddUserRoleModel model);
        Task<(List<UserRoleDetailDto> userRole,int totalCount)> GetAllAsync(int page,int pageSize,string search);
        Task<UserRoleDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
