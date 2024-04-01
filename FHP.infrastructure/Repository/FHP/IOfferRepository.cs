using FHP.dtos.FHP.Offer;
using FHP.entity.FHP;

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
