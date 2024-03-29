﻿using FHP.dtos.FHP;
using FHP.dtos.FHP.EmployeeDetail;
using FHP.entity.FHP;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IEmployeeDetailRepository
    {
        Task AddAsync(EmployeeDetail entity);
        Task<EmployeeDetail> GetAsync(int id);
        void Edit(EmployeeDetail entity);
        Task<(List<EmployeeDetailDto>employee, int totalCount)> GetAllAsync(int page,int pageSize,int userId, string? search);    
        Task<EmployeeDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<string> SetAvailabilityAsync(int id);
        Task<CompleteEmployeeDetailDto> GetAllByIdAsync(int id);

    }
}
