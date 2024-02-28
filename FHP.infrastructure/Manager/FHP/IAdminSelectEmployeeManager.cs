﻿using FHP.dtos.FHP;
using FHP.models.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IAdminSelectEmployeeManager
    {
        Task AddAsync(AddAdminSelectEmployeeModel model);
        Task Edit(AddAdminSelectEmployeeModel model);
        Task<(List<AdminSelectEmployeeDetailDto> adminSelect,int totalCount)> GetAllAsync(int page,int pageSize,int jobId,string? search);
        Task<AdminSelectEmployeeDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
