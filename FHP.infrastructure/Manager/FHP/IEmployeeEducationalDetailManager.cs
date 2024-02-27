﻿using FHP.dtos.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.FHP
{
    public interface IEmployeeEducationalDetailManager
    {
       Task AddAsync(AddEmployeeEducationalDetailModel model);
       Task Edit(AddEmployeeEducationalDetailModel model);
       Task<(List<EmployeeEducationalDetailDetailDto> employeeeducationaldetail, int totalCount)> GetAllAsync(int page,int pageSize,string? search);
       Task<EmployeeEducationalDetailDetailDto> GetByIdAsync(int id);
       Task DeleteAsync(int id);
        
    }
}