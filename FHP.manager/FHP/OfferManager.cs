﻿using FHP.dtos.FHP.Offer;
using FHP.factories.FHP;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Repository.FHP;
using FHP.models.FHP.Offer;

namespace FHP.manager.FHP
{
    public class OfferManager : IOfferManager
    {
        private readonly IOfferRepository _repository;

        public OfferManager(IOfferRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(AddOfferModel model)
        {
            await _repository.AddAsync(OfferFactory.Create(model));
        }

        
        public async Task Edit(AddOfferModel model)
        {
             var data = await _repository.GetAsync(model.Id);
             OfferFactory.update(data, model);
             _repository.Edit(data);
        }

        public async Task<(List<OfferDetailDto> offer, int totalCount)> GetAllAsync(int page, int pageSize, string? search, int employeeId, int employerId)
        {
            return await _repository.GetAllAsync(page, pageSize, search,employeeId,employerId);
        }

        public async Task<OfferDetailDto> GetByIdAsync(int id)
        {
           return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        /*public async Task<string> OfferAcceptRejectAsync(int id, int jobId, int employeeId)
        {
           return  await _repository.OfferAcceptRejectAsync(id, jobId, employeeId);
        }*/

        public async Task<string> OfferAcceptRejectAsync(SetOfferStatusModel model)
        {
            return await _repository.OfferAcceptRejectAsync(model);
        }
    }
}
