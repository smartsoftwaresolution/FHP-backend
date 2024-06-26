﻿using FHP.dtos.FHP.EmployeeProfessionalDetail;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployeeProfessionalDetailRepository
    {
        Task AddAsync(EmployeeProfessionalDetail entity);
        Task<EmployeeProfessionalDetail> GetAsync(int id);
        void Edit(EmployeeProfessionalDetail entity);
        Task<(List<EmployeeProfessionalDetailDto> employeeProfessionalDetail, int totalCount)> GetAllAsync(int page, int pageSize,int userId, string? search, string? jobDescription, string? designation, string? yearOfExperience);
        Task<EmployeeProfessionalDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);

    }
}
