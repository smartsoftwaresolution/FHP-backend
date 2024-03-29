﻿using FHP.dtos.FHP.Offer;
using FHP.entity.FHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.Repository.FHP
{
    public interface IOfferRepository
    {
        Task AddAsync(Offer entity);
        void Edit(Offer entity);
        Task<Offer> GetAsync(int id);
        Task<(List<OfferDetailDto> offer, int totalCount)> GetAllAsync(int page, int pageSize, string? search);
        Task<OfferDetailDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);   
    }
}
