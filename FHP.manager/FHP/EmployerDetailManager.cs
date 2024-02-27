﻿using FHP.dtos.FHP;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.manager.FHP
{
    public class EmployerDetailManager : IEmployerDetailManager
    {
        private readonly IEmployerDetailRepository _repository;

        public EmployerDetailManager(IEmployerDetailRepository repository)
        {
            _repository=repository;
        }

        public async Task AddAsync(AddEmployerDetailModel model)
        {
           await _repository.AddAsync(EmployerDetailFactory.Create(model)); 
        }

        public async Task Edit(AddEmployerDetailModel model)
        {
            var data = await _repository.GetAsync(model.Id);
            EmployerDetailFactory.Update(data, model);
            _repository.Edit(data);
        }

        public async Task<(List<EmployerDetailDetailDto> employerDetail, int totalCount)> GetAllAsync(int page, int pageSize, string? search)
        {
          return await _repository.GetAllAsync(page, pageSize, search);
        }

        public async Task<EmployerDetailDetailDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
