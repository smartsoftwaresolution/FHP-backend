﻿
using FHP.dtos.UserManagement;
using FHP.entity.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.UserManagement
{
    public interface ICityRepository
    {
        Task AddAsync(City entity);
        Task<City> GetAsync(int id);
        void Edit(City entity);
        Task<List<CityDetailDto>> GetAllAsync();
        Task<CityDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);

    }
}