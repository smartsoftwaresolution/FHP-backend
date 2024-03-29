﻿using FHP.dtos.UserManagement.State;
using FHP.models.UserManagement.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Manager.UserManagement
{
    public interface IStateManager
    {
        Task AddAsync(AddStateModel model);
        Task Edit(AddStateModel model);
        Task<(List<StateDetailDto> state, int totalCount)> GetAllAsync(int page,int pageSize,string search);
        Task<StateDetailDto> GetByIdAsync(int id);
        Task<List<StateDetailDto>> GetByCountryIdAsync(int countryId);
        Task DeleteAsync(int id);   
    }
}
